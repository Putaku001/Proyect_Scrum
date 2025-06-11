<?php
session_start();
require_once 'db.php';
require_once __DIR__ . '/vendor/autoload.php';
$client = require 'drive_auth_admin.php';
$drive = new Google\Service\Drive($client);

const COVER_DIR = __DIR__ . '/imgs/covers/';
const COVER_WEB = 'imgs/covers/';

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
  header("Location: login.html");
  exit();
}

// --- Utilidades y ordenamiento especial de tomos ---
function extractDriveId($url)
{
  if (preg_match('~/file/d/([^/]+)~', $url, $m)) return $m[1];
  if (preg_match('/id=([^&]+)/', $url, $m)) return $m[1];
  return null;
}
function extractFolderId($url)
{
  return preg_match('~/folders/([^?]+)~', $url, $m) ? $m[1] : null;
}
function getParentFolder($fileId, $drive)
{
  try {
    $file = $drive->files->get($fileId, ['fields' => 'parents']);
    return $file->parents[0] ?? null;
  } catch (Exception $e) {
    return null;
  }
}
// Ordenamiento pro para tomos (por número de tomo natural, luego extras)
function customSortTomos($a, $b)
{
  $pattern = '/(\d+)(?!.*\d)/'; // último número en el nombre
  $na = $a->name;
  $nb = $b->name;
  // Priorizar premium
  $pa = str_starts_with($na, '[P]');
  $pb = str_starts_with($nb, '[P]');
  if ($pa && !$pb) return -1;
  if (!$pa && $pb) return 1;
  // Orden por número de tomo, extras al final
  $fa = preg_match($pattern, $na, $ma) ? (int)$ma[1] : 1e6;
  $fb = preg_match($pattern, $nb, $mb) ? (int)$mb[1] : 1e6;
  if ($fa != $fb) return $fa - $fb;
  return strnatcasecmp($na, $nb);
}
function listTomos($folderId, $drive)
{
  $result = [];
  $pageToken = null;
  do {
    $response = $drive->files->listFiles([
      'q' => "'$folderId' in parents and mimeType='application/pdf' and trashed=false",
      'fields' => 'nextPageToken, files(id, name, createdTime)',
      'pageToken' => $pageToken
    ]);
    foreach ($response->files as $file) $result[] = $file;
    $pageToken = $response->getNextPageToken();
  } while ($pageToken);
  usort($result, 'customSortTomos');
  return $result;
}

$id = filter_input(INPUT_GET, 'id', FILTER_VALIDATE_INT) ?: 0;
if ($id <= 0) die("ID inválido");

$sql = "SELECT M.*, TA.TituloAlternativo FROM Mangas M
        LEFT JOIN TitulosAlternativos TA ON M.MangaID = TA.MangaID
        WHERE M.MangaID = ?";
$stmt = sqlsrv_query($conn, $sql, [$id]);
$manga = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
if (!$manga) die("Manga no encontrado");

$generos = [];
$res = sqlsrv_query($conn, "SELECT GeneroID, Nombre FROM Generos");
while ($g = sqlsrv_fetch_array($res, SQLSRV_FETCH_ASSOC)) $generos[] = $g;

// --- EDICIÓN DE DATOS Y PORTADA ---
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['titulo']) && !isset($_POST['accion_tomo'])) {
  $titulo = trim($_POST['titulo']);
  $alt    = trim($_POST['titulo_alt']);
  $autor  = trim($_POST['autor']);
  $desc   = trim($_POST['descripcion']);
  $estado = trim($_POST['estado']);
  $fecha  = $_POST['fecha'];
  $genero = (int)$_POST['genero'];

  if (!$titulo || !$autor) {
    echo "<script>alert('Faltan campos requeridos');history.back();</script>";
    exit;
  }
  $urlPortDrive = $manga['URLPortada'];
  $urlPortWeb   = $manga['URLPortadaWeb'];
  $localPrev    = COVER_DIR . basename($urlPortWeb);

  // Subida segura de portada nueva
  if (!empty($_FILES['portada']['tmp_name'])) {
    $tmp  = $_FILES['portada']['tmp_name'];
    $info = getimagesize($tmp);
    if (!$info || !in_array($info[2], [IMAGETYPE_PNG, IMAGETYPE_JPEG])) {
      echo "<script>alert('La portada debe ser PNG o JPG.');history.back();</script>";
      exit;
    }
    $ext  = $info[2] === IMAGETYPE_PNG ? 'png' : 'jpg';
    $mime = $ext === 'png' ? 'image/png' : 'image/jpeg';

    $raizId   = extractFolderId($manga['URLMangaDrive']);
    $parentId = null;

    if ($raizId) {
      try {
        $folders = $drive->files->listFiles([
          'q' => "'$raizId' in parents and mimeType='application/vnd.google-apps.folder' and trashed=false",
          'fields' => 'files(id, name)'
        ]);
        foreach ($folders->files as $folder) {
          if (strtolower(trim($folder->name)) === 'portada') {
            $parentId = $folder->id;
            break;
          }
        }
      } catch (Exception $e) {
      }
    }

    $oldId = extractDriveId($urlPortDrive);
    if (!$parentId && $oldId) {
      $parentId = getParentFolder($oldId, $drive);
    }
    if (!$parentId) {
      echo "<script>alert('No se encontró carpeta Portada en Google Drive');history.back();</script>";
      exit;
    }

    if ($oldId) {
      try {
        $drive->files->delete($oldId);
      } catch (Exception $e) {
      }
    }
    if (file_exists($localPrev)) unlink($localPrev);

    $meta = new Google\Service\Drive\DriveFile([
      'name'    => "portada.$ext",
      'parents' => [$parentId]
    ]);
    $upload = $drive->files->create($meta, [
      'data'       => file_get_contents($tmp),
      'mimeType'   => $mime,
      'uploadType' => 'multipart',
      'fields'     => 'id'
    ]);
    $newId = $upload->id;
    $urlPortDrive = "https://drive.google.com/uc?export=view&id=$newId";

    if (!is_dir(COVER_DIR)) mkdir(COVER_DIR, 0755, true);
    $filename    = strtolower(preg_replace('/[^a-z0-9]+/i', '_', $titulo)) . '.' . $ext;
    $fullPath    = COVER_DIR . $filename;
    $urlPortWeb  = COVER_WEB . $filename;
    move_uploaded_file($tmp, $fullPath);
  }

  sqlsrv_query(
    $conn,
    "UPDATE Mangas SET
        Titulo=?, Autor=?, Descripcion=?, Estado=?, FechaPublicacion=?,
        GeneroID=?, URLPortada=?, URLPortadaWeb=? WHERE MangaID=?",
    [$titulo, $autor, $desc, $estado, $fecha, $genero, $urlPortDrive, $urlPortWeb, $id]
  );

  sqlsrv_query($conn, "DELETE FROM TitulosAlternativos WHERE MangaID=?", [$id]);
  if ($alt !== '') {
    sqlsrv_query($conn, "INSERT INTO TitulosAlternativos (MangaID, TituloAlternativo) VALUES (?,?)", [$id, $alt]);
  }
  echo "<script>alert('✅ Cambios guardados.');location='editar_manga.php?id=$id';</script>";
  exit;
}

// --- ACCIONES DE TOMOS ---
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['accion_tomo'])) {
  $accion = $_POST['accion_tomo'];
  $fileId = $_POST['file_id'] ?? '';
  $nombre = $_POST['file_name'] ?? '';

  if ($accion === 'eliminar' && $fileId) {
    try {
      $drive->files->delete($fileId);
    } catch (Exception $e) {
    }
  } elseif ($accion === 'marcar_premium' && $fileId) {
    $newName = "[P] " . preg_replace('/^\[P\]\s*/', '', $nombre);
    $drive->files->update($fileId, new Google\Service\Drive\DriveFile(['name' => $newName]));
  } elseif ($accion === 'quitar_premium' && $fileId) {
    $newName = preg_replace('/^\[P\]\s*/', '', $nombre);
    $drive->files->update($fileId, new Google\Service\Drive\DriveFile(['name' => $newName]));
  }
  header("Location: editar_manga.php?id=$id");
  exit;
}

// --- SUBIDA DE TOMO NUEVO ---
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_FILES['nuevo_tomo'])) {
  $folderId = extractFolderId($manga['URLMangaDrive']);
  if ($folderId && $_FILES['nuevo_tomo']['error'] === 0) {
    $name = basename($_FILES['nuevo_tomo']['name']);
    if (mime_content_type($_FILES['nuevo_tomo']['tmp_name']) !== 'application/pdf') {
      echo "<script>alert('El archivo debe ser PDF.');history.back();</script>";
      exit;
    }
    $fileMeta = new Google\Service\Drive\DriveFile([
      'name' => $name,
      'parents' => [$folderId]
    ]);
    $drive->files->create($fileMeta, [
      'data' => file_get_contents($_FILES['nuevo_tomo']['tmp_name']),
      'mimeType' => 'application/pdf',
      'uploadType' => 'multipart'
    ]);
  }
  header("Location: editar_manga.php?id=$id");
  exit;
}

$tomos = listTomos(extractFolderId($manga['URLMangaDrive']), $drive);
?>

<!DOCTYPE html>
<html lang="es">

<head>
  <meta charset="UTF-8">
  <title>Editar Manga</title>
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="/css/style.css">
  <style>
    html,
    body {
      height: 100%;
      margin: 0;
      padding: 0;
    }

    body {
      background: linear-gradient(135deg, #23264a 65%, #18192b 100%);
      color: #f3f5fc;
      font-family: 'Segoe UI', Arial, sans-serif;
      min-height: 100vh;
    }

    .container-main {
      max-width: 750px;
      margin: 32px auto 0;
      padding: 30px 25px 16px;
      background: #232544;
      border-radius: 22px;
      box-shadow: 0 6px 40px #000a;
      display: flex;
      flex-wrap: wrap;
      gap: 32px 38px;
      justify-content: space-between;
    }

    .col-data {
      width: 320px;
      min-width: 260px;
    }

    .col-portada {
      flex: 1;
      min-width: 210px;
    }

    @media (max-width:900px) {
      .container-main {
        flex-direction: column;
        gap: 20px;
      }

      .col-data,
      .col-portada {
        width: 100%;
      }
    }

    h1 {
      text-align: left;
      color: #0ed6e7;
      letter-spacing: 1px;
      font-size: 2em;
      margin-bottom: 15px;
    }

    .form-row {
      margin-bottom: 17px;
    }

    label {
      font-weight: bold;
      display: block;
      margin-bottom: 3px;
      font-size: 1.04em;
    }

    input,
    select,
    textarea {
      width: 100%;
      padding: 10px 10px;
      border-radius: 9px;
      border: none;
      background: #1b1c2a;
      color: #fff;
      font-size: 1.08em;
      transition: outline .17s, box-shadow .17s;
    }

    input:focus,
    select:focus,
    textarea:focus {
      outline: 2px solid #00eaff;
      box-shadow: 0 0 0 2px #00eaff55;
    }

    input[type="file"] {
      background: transparent;
      color: #00eaff;
    }

    .portada-box {
      background: #18192b;
      border-radius: 11px;
      padding: 16px 12px 15px;
      box-shadow: 0 1px 10px #0003;
    }

    .portada-box img {
      display: block;
      max-width: 100%;
      max-height: 320px;
      margin: auto;
      border-radius: 10px;
      box-shadow: 0 2px 17px #0028;
    }

    .form-actions {
      margin-top: 26px;
      display: flex;
      gap: 20px;
      justify-content: space-between;
    }

    .form-actions button {
      padding: 11px 0;
      border-radius: 11px;
      font-weight: bold;
      width: 48%;
      cursor: pointer;
      border: none;
      background: #0ed6e7;
      color: #151a23;
      font-size: 1.1em;
      box-shadow: 0 2px 12px #0012;
      transition: background .13s, transform .10s;
    }

    .form-actions button:active {
      transform: scale(.97);
    }

    .form-actions button:hover {
      background: #03b8d2;
    }

    .seccion-tomos {
      max-width: 750px;
      margin: 42px auto 0;
    }

    .tomos-card {
      background: #18192b;
      border-radius: 18px;
      box-shadow: 0 2px 14px #0009;
      padding: 22px 24px 18px;
      margin-bottom: 16px;
    }

    .tomos-title {
      color: #0ed6e7;
      font-size: 1.32em;
      margin-bottom: 19px;
      letter-spacing: .7px;
    }

    .tomo {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 14px;
      padding: 12px 0;
      border-bottom: 1px solid #29304b;
    }

    .tomo:last-child {
      border-bottom: none;
    }

    .tomo-info {
      display: flex;
      align-items: center;
      min-width: 0;
      flex: 1;
      gap: 10px;
    }

    .tomo-premium-badge {
      display: inline-block;
      font-size: .98em;
      font-weight: 600;
      padding: 2px 10px 2px 8px;
      border-radius: 7px;
      margin-right: 7px;
      letter-spacing: .3px;
      vertical-align: middle;
    }

    .tomo-premium-badge.premium {
      background: #ffe381;
      color: #a17d08;
    }

    .tomo-premium-badge.no-premium {
      background: #b6eaff;
      color: #0b5d7d;
    }

    .tomo-name {
      max-width: 220px;
      min-width: 0;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      font-size: 1.08em;
      letter-spacing: .2px;
      cursor: pointer;
    }

    .tomo-name[title]:hover {
      text-decoration: underline;
    }

    .tomo-actions form {
      display: inline;
    }

    .tomo-actions button {
      font-size: 1.09em;
      padding: 7px 12px;
      border-radius: 8px;
      border: none;
      cursor: pointer;
      font-weight: 600;
      margin-left: 7px;
      background: #0ed6e7;
      color: #191c24;
      transition: background .11s;
    }

    .tomo-actions button[title="Eliminar"] {
      background: #e13a42;
      color: #fff;
    }

    .tomo-actions button[title="Eliminar"]:hover {
      background: #c91c26;
    }

    .tomo-actions button[title="Marcar Premium"] {
      background: #ffe381;
      color: #a17d08;
    }

    .tomo-actions button[title="Marcar Premium"]:hover {
      background: #ffce38;
    }

    .tomo-actions button[title="Quitar Premium"] {
      background: #5feec2;
      color: #0d4233;
    }

    .tomo-actions button[title="Quitar Premium"]:hover {
      background: #16d49e;
    }

    .nuevo-tomo {
      margin-top: 24px;
      background: #222641;
      padding: 14px 13px 9px;
      border-radius: 13px;
      box-shadow: 0 1px 10px #0003;
    }

    @media (max-width:600px) {

      .container-main,
      .seccion-tomos {
        max-width: 99vw;
        padding: 2vw;
      }

      .tomos-card {
        padding: 12px 6vw;
      }

      .tomo-name {
        max-width: 105px;
      }

      .portada-box {
        padding: 10px;
      }
    }

    .btn-volver {
      position: fixed;
      top: 32px;
      left: 32px;
      background: #232544;
      color: #00eaff;
      border-radius: 11px;
      padding: 11px 24px 11px 18px;
      font-weight: 700;
      font-size: 1.16em;
      text-decoration: none;
      box-shadow: 0 4px 22px #0005;
      border: 2px solid #00eaff22;
      transition: background .15s, color .15s, border .14s, box-shadow .12s, transform .11s;
      z-index: 10;
      display: inline-block;
    }

    .btn-volver:hover,
    .btn-volver:focus {
      background: #00eaff;
      color: #232544;
      border: 2.5px solid #00eaff;
      box-shadow: 0 8px 24px #00eaff66;
      outline: none;
      transform: scale(1.04);
    }

    @media (max-width:600px) {
      .btn-volver {
        top: 10px;
        left: 8px;
        font-size: 1em;
        padding: 9px 13px;
      }
    }

        /* Estilos para el theme switcher */
    .theme-switcher-btn {
      position: fixed;
      top: 32px;
      right: 32px;
      background: #232544;
      color: #00eaff;
      border-radius: 50%;
      width: 50px;
      height: 50px;
      font-size: 1.4em;
      cursor: pointer;
      border: 2px solid #00eaff22;
      box-shadow: 0 4px 22px #0005;
      transition: all 0.3s;
      z-index: 10;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .theme-switcher-btn:hover {
      background: #00eaff;
      color: #232544;
      border: 2px solid #00eaff;
      box-shadow: 0 8px 24px #00eaff66;
      transform: scale(1.08);
    }

    .light-icon {
      display: none;
    }

    [data-theme="light"] .dark-icon {
      display: none;
    }

    [data-theme="light"] .light-icon {
      display: block;
    }

    @media (max-width:600px) {
      .theme-switcher-btn {
        top: 12px;
        right: 12px;
        width: 42px;
        height: 42px;
        font-size: 1.2em;
      }
    }

    /* Tema claro - Versión mejorada */
    [data-theme="light"] body {
      background: #f8f9fa;
      color: #2d3748;
    }

    [data-theme="light"] .container-main,
    [data-theme="light"] .tomos-card,
    [data-theme="light"] .portada-box,
    [data-theme="light"] .nuevo-tomo {
      background: #ffffff;
      color: #2d3748;
      box-shadow: 0 2px 15px rgba(0, 0, 0, 0.08);
      border: 1px solid #e2e8f0;
    }

    [data-theme="light"] input,
    [data-theme="light"] select,
    [data-theme="light"] textarea {
      background: #f7fafc;
      color: #4a5568;
      border: 1px solid #e2e8f0;
    }

    [data-theme="light"] .btn-volver {
      background: #ffffff;
      color: #3182ce;
      border: 2px solid #bee3f8;
    }

    [data-theme="light"] .theme-switcher-btn {
      background: #ffffff;
      color: #3182ce;
      border: 2px solid #bee3f8;
    }

    [data-theme="light"] .form-actions button {
      background: #3182ce;
      color: white;
    }

    [data-theme="light"] .tomo-premium-badge.premium {
      background: #f6e05e;
      color: #975a16;
    }

    [data-theme="light"] .tomo-premium-badge.no-premium {
      background: #90cdf4;
      color: #2c5282;
    }

    [data-theme="light"] .tomo-actions button[title="Marcar Premium"] {
      background: #f6e05e;
      color: #975a16;
    }

    [data-theme="light"] .tomo-actions button[title="Quitar Premium"] {
      background: #68d391;
      color: #276749;
    }

    [data-theme="light"] .tomo-actions button[title="Eliminar"] {
      background: #fc8181;
      color: #9b2c2c;
    }

    [data-theme="light"] .portada-box {
      background: #f7fafc;
    }

    [data-theme="light"] .tomos-title,
    [data-theme="light"] h1 {
      color: #3182ce;
    }
  </style>
  <script>
    function previewPortada(e) {
      const input = e.target,
        file = input.files[0];
      if (!file) return;
      const reader = new FileReader();
      reader.onload = function(ev) {
        document.getElementById('previewPortada').src = ev.target.result;
      }
      reader.readAsDataURL(file);
    }
    // Tooltip elegante para nombres de tomo largos
    document.addEventListener('DOMContentLoaded', function() {
      document.querySelectorAll('.tomo-name').forEach(el => {
        if (el.scrollWidth > el.clientWidth) el.setAttribute('title', el.textContent);
      });
    });
  </script>
  <script src="js/theme-switcher.js"></script>
</head>

<body>
  <a href="catalogo_admin.php" class="btn-volver" title="Volver al catálogo">
    <span style="font-size:1.6em;vertical-align:-2px;">←</span> Volver
  </a>

  <button id="theme-toggle" class="theme-switcher-btn" title="Cambiar tema">
  <span class="dark-icon">🌙</span>
  <span class="light-icon">☀️</span>
  </button>

  <form method="POST" enctype="multipart/form-data" autocomplete="off">
    <div class="container-main">
      <!-- Datos del Manga -->
      <div class="col-data">
        <h1>Editar Manga</h1>
        <div class="form-row">
          <label>Título:</label>
          <input type="text" name="titulo" value="<?= htmlspecialchars($manga['Titulo']) ?>" required maxlength="100">
        </div>
        <div class="form-row">
          <label>Título alternativo:</label>
          <input type="text" name="titulo_alt" value="<?= htmlspecialchars($manga['TituloAlternativo'] ?? '') ?>" maxlength="100">
        </div>
        <div class="form-row">
          <label>Autor:</label>
          <input type="text" name="autor" value="<?= htmlspecialchars($manga['Autor']) ?>" required maxlength="80">
        </div>
        <div class="form-row">
          <label>Descripción:</label>
          <textarea name="descripcion" rows="4" maxlength="600"><?= htmlspecialchars($manga['Descripcion']) ?></textarea>
        </div>
        <div class="form-row">
          <label>Estado:</label>
          <select name="estado">
            <option value="En emisión" <?= $manga['Estado'] == 'En emisión' ? 'selected' : '' ?>>En emisión</option>
            <option value="Finalizado" <?= $manga['Estado'] == 'Finalizado' ? 'selected' : '' ?>>Finalizado</option>
            <option value="Pausado" <?= $manga['Estado'] == 'Pausado' ? 'selected' : '' ?>>Pausado</option>
          </select>
        </div>
        <div class="form-row">
          <label>Fecha publicación:</label>
          <input type="date" name="fecha" value="<?= $manga['FechaPublicacion']->format('Y-m-d') ?>">
        </div>
        <div class="form-row">
          <label>Género:</label>
          <select name="genero">
            <?php foreach ($generos as $g): ?>
              <option value="<?= $g['GeneroID'] ?>" <?= $manga['GeneroID'] == $g['GeneroID'] ? 'selected' : '' ?>>
                <?= htmlspecialchars($g['Nombre']) ?>
              </option>
            <?php endforeach; ?>
          </select>
        </div>
        <div class="form-actions">
          <button type="submit">💾 Guardar</button>
          <button type="button" onclick="location.href='catalogo_admin.php'">✖ Cancelar</button>
        </div>
      </div>
      <!-- Portada -->
      <div class="col-portada">
        <div class="portada-box">
          <label>Cambiar portada (opcional):</label>
          <input type="file" name="portada" accept="image/png, image/jpeg" onchange="previewPortada(event)">
          <img class="preview" id="previewPortada" src="<?= htmlspecialchars($manga['URLPortadaWeb']) ?>" alt="Portada actual">
        </div>
      </div>
    </div>
  </form>

  <!-- Tomos -->
  <div class="seccion-tomos">
    <div class="tomos-card">
      <div class="tomos-title">Tomos del Manga (ordenados):</div>
      <?php foreach ($tomos as $t): ?>
        <div class="tomo">
          <div class="tomo-info">
            <?php if (str_starts_with($t->name, '[P]')): ?>
              <span class="tomo-premium-badge premium" title="Tomo premium">Premium</span>
            <?php else: ?>
              <span class="tomo-premium-badge no-premium" title="No premium">No premium</span>
            <?php endif; ?>
            <span class="tomo-name"><?= htmlspecialchars($t->name) ?></span>
          </div>
          <div class="tomo-actions">
            <form method="POST" onsubmit="return confirm('¿Eliminar este tomo?');">
              <input type="hidden" name="file_id" value="<?= $t->id ?>">
              <input type="hidden" name="file_name" value="<?= $t->name ?>">
              <input type="hidden" name="accion_tomo" value="eliminar">
              <button type="submit" title="Eliminar">🗑</button>
            </form>
            <?php if (str_starts_with($t->name, '[P]')): ?>
              <form method="POST" style="display:inline;">
                <input type="hidden" name="file_id" value="<?= $t->id ?>">
                <input type="hidden" name="file_name" value="<?= $t->name ?>">
                <input type="hidden" name="accion_tomo" value="quitar_premium">
                <button type="submit" title="Quitar Premium">🔓</button>
              </form>
            <?php else: ?>
              <form method="POST" style="display:inline;">
                <input type="hidden" name="file_id" value="<?= $t->id ?>">
                <input type="hidden" name="file_name" value="<?= $t->name ?>">
                <input type="hidden" name="accion_tomo" value="marcar_premium">
                <button type="submit" title="Marcar Premium">🔒</button>
              </form>
            <?php endif; ?>
          </div>
        </div>
      <?php endforeach; ?>

      <form method="POST" enctype="multipart/form-data" class="nuevo-tomo">
        <label style="margin-top:18px;">📤 Subir nuevo tomo (PDF):</label>
        <input type="file" name="nuevo_tomo" accept="application/pdf" required style="margin-bottom:10px;">
        <div style="margin-top:7px;">
          <button type="submit" style="width:100%;padding:11px 0; background:#0ed6e7; color:#23264a; font-weight:600; border-radius:8px;">Subir</button>
        </div>
      </form>
    </div>
  </div>

</body>

</html>