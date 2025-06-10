<?php
session_start();
require_once 'db.php';
require_once 'drive_auth.php';

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: login.html");
    exit();
}

if (!isset($_GET['id'])) die('Manga no especificado.');
$mangaId = (int)$_GET['id'];

// ───── Obtener info manga ─────
$sql = "SELECT M.Titulo, M.Autor, M.Descripcion, M.FechaPublicacion,
               M.URLMangaDrive, M.URLPortada, M.URLPortadaWeb,
               G.Nombre AS Genero
        FROM Mangas M
        LEFT JOIN Generos G ON M.GeneroID = G.GeneroID
        WHERE M.MangaID = ?";
$stmt = sqlsrv_query($conn, $sql, [$mangaId]);
if (!$stmt || !sqlsrv_fetch($stmt)) die('Manga no encontrado.');

$titulo           = sqlsrv_get_field($stmt, 0);
$autor            = sqlsrv_get_field($stmt, 1);
$descripcion      = sqlsrv_get_field($stmt, 2);
$fechaPublicacion = sqlsrv_get_field($stmt, 3);
$urlDrive         = sqlsrv_get_field($stmt, 4);
$urlPortadaDrive  = sqlsrv_get_field($stmt, 5);
$urlPortadaWeb    = sqlsrv_get_field($stmt, 6);
$genero           = sqlsrv_get_field($stmt, 7);

// ───── Portada ─────
$urlPortada = './imgs/no_portada.png';
if (!empty($urlPortadaWeb) && file_exists($urlPortadaWeb)) {
    $urlPortada = $urlPortadaWeb;
} elseif (!empty($urlPortadaDrive)) {
    $urlPortada = $urlPortadaDrive;
}

// ───── Carpeta Google Drive ─────
function folderId($url) {
    if (preg_match('/\/folders\/([a-zA-Z0-9_-]+)/', $url, $m)) return $m[1];
    if (preg_match('/[?&]id=([a-zA-Z0-9_-]+)/', $url, $m))    return $m[1];
    return null;
}
$folder_id = folderId($urlDrive) ?: die('Carpeta Drive inválida.');

// ───── Listar PDFs ─────
$ch = curl_init(
    'https://www.googleapis.com/drive/v3/files?' . http_build_query([
        'q'        => sprintf("'%s' in parents and mimeType='application/pdf' and trashed=false", $folder_id),
        'fields'   => 'files(id,name,webViewLink)',
        'pageSize' => 100
    ])
);
curl_setopt_array($ch, [
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_HTTPHEADER     => ["Authorization: Bearer $access_token"]
]);
$data     = json_decode(curl_exec($ch), true) ?: [];
$archivos = $data['files'] ?? [];
curl_close($ch);

usort($archivos, function ($a, $b) {
    preg_match('/(\d+)/', $a['name'], $na);
    preg_match('/(\d+)/', $b['name'], $nb);
    return ($na[1] ?? 0) - ($nb[1] ?? 0);
});
?>
<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title><?= htmlspecialchars($titulo) ?> - Admin Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <style>
    body {
      background: #181b2e;
      color: #ecf6fb;
      font-family: 'Segoe UI', sans-serif;
      margin: 0;
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
    .btn-volver:hover, .btn-volver:focus {
      background: #00eaff;
      color: #232544;
      border: 2.5px solid #00eaff;
      box-shadow: 0 8px 24px #00eaff66;
      outline: none;
      transform: scale(1.04);
    }
    .detalle-manga-admin-wrap {
      position: relative;
      max-width: 1000px;
      margin: 80px auto 48px;
      padding-right: 110px;
    }
    .card-btns-externos {
      position: absolute;
      top: 50%;
      right: -90px;
      transform: translateY(-50%);
      display: flex;
      flex-direction: column;
      gap: 22px;
      z-index: 4;
    }
    .fab-btn {
      width: 56px;
      height: 56px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      border: none;
      color: #fff;
      font-size: 2.1em;
      background: linear-gradient(135deg,#00eaff 60%,#8f4fff 100%);
      box-shadow: 0 6px 22px #0006;
      cursor: pointer;
      transition: transform .16s, box-shadow .13s, background .14s;
      outline: none;
    }
    .fab-btn:hover {
      background: linear-gradient(135deg,#02ffe3,#7043ff 90%);
      color: #232544;
      transform: scale(1.09);
      box-shadow: 0 10px 30px #00eaff88;
    }
    .fab-btn.delete {
      background: linear-gradient(135deg,#ff2951 55%,#4f0034 100%);
    }
    .fab-btn.delete:hover {
      background: linear-gradient(135deg,#ff2951 55%,#f91e1e 100%);
      color: #fff;
      box-shadow: 0 10px 38px #ff295188;
    }
    .detalle-manga-admin-card {
      max-width: 900px;
      margin: 0 auto;
      background: #21244c;
      border-radius: 18px;
      box-shadow: 0 0 30px #0009;
      padding: 0 0 34px 0;
      position: relative;
      overflow: visible;
      min-height: 420px;
    }
    .detalle-manga-top {
      display: flex;
      flex-wrap: wrap;
      gap: 34px;
      padding: 38px 40px 0 38px;
    }
    .detalle-manga-portada {
      width: 310px;
      height: 460px;
      object-fit: cover;
      border-radius: 14px;
      box-shadow: 0 6px 34px #0007;
      background: #131628;
    }
    .detalle-manga-info {
      flex: 1;
      display: flex;
      flex-direction: column;
      justify-content: flex-start;
      align-items: flex-start;
      min-width: 270px;
    }
    .detalle-manga-info h1 {
      font-size: 2.6rem;
      margin: 0 0 14px;
      font-weight: 700;
      letter-spacing: 0.03em;
      color: #00eaff;
      line-height: 1.14;
    }
    .badge-genero {
      background: linear-gradient(135deg, #00eaff 60%, #8f4fff 100%);
      color: #232544;
      display: inline-block;
      padding: 8px 20px;
      border-radius: 21px;
      font-size: 1.06rem;
      margin-bottom: 20px;
      font-weight: 600;
      box-shadow: 0 2px 10px #00eaff33;
    }
    .detalle-manga-info p {
      font-size: 1.11rem;
      line-height: 1.57;
      margin: 0 0 12px;
      color: #d3e2ef;
    }
    .detalle-manga-info h3 {
      font-size: 1.22rem;
      color: #8f4fff;
      margin-top: 18px;
      margin-bottom: 9px;
      font-weight: 600;
    }
    .detalle-manga-info .datos-sec {
      font-size: 1rem;
      margin-bottom: 9px;
      color: #6ff6ff;
      font-weight: 500;
    }
    .tomos-section {
      margin: 44px 0 0 0;
      padding: 0 38px;
    }
    .tomos-section h2 {
      font-size: 1.41rem;
      margin-bottom: 17px;
      color: #49f5ff;
      font-weight: 700;
      letter-spacing: 0.03em;
    }
    .tomo-card {
      background: #252e57;
      color: #ffe072;
      padding: 16px 22px 15px 22px;
      margin-bottom: 12px;
      border-radius: 9px;
      font-size: 1.08rem;
      box-shadow: 0 2px 12px #0004;
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 18px;
      transition: box-shadow 0.18s;
    }
    .tomo-card:hover {
      box-shadow: 0 8px 24px #00eaff33;
    }
    .tomo-info {
      display: flex;
      align-items: center;
      gap: 13px;
    }
    .tomo-title {
      font-weight: bold;
      font-size: 1.05em;
      color: #ffea77;
      letter-spacing: 0.04em;
    }
    .premium-badge {
      display: inline-block;
      padding: 4px 13px 4px 9px;
      background: linear-gradient(90deg,#f43fff 60%,#8e24aa 100%);
      color: #fff;
      border-radius: 18px;
      font-size: 0.93em;
      font-weight: 600;
      margin-left: 9px;
      box-shadow: 0 1px 7px #8e24aa44;
    }
    .nopremium-badge {
      display: inline-block;
      padding: 4px 14px 4px 9px;
      background: linear-gradient(90deg,#1de9b6 70%,#40c4ff 100%);
      color: #23305c;
      border-radius: 18px;
      font-size: 0.93em;
      font-weight: 600;
      margin-left: 9px;
      box-shadow: 0 1px 7px #49f5ff33;
    }
    .tomo-links a {
      color: #00eaff;
      text-decoration: none;
      margin-right: 15px;
      font-weight: 600;
      font-size: 1.05em;
      transition: color .16s;
    }
    .tomo-links a:hover {
      color: #ffe072;
      text-shadow: 0 1px 4px #00eaff55;
    }
    @media (max-width:1100px){
      .card-btns-externos {
        right: 10px;
      }
      .detalle-manga-admin-wrap {
        padding-right: 80px;
      }
    }
    @media (max-width:900px) {
      .detalle-manga-top { flex-direction:column; align-items:center; gap:24px;padding:30px 10px 0 10px; }
      .detalle-manga-portada { width: 210px;height: 320px; }
      .detalle-manga-info { min-width:180px;width:100%; }
      .tomos-section {padding:0 11px;}
    }
    @media (max-width:700px){
      .card-btns-externos {
        position: static;
        transform: none;
        flex-direction: row;
        gap: 13px;
        margin: 14px 0 18px 0;
        justify-content: flex-end;
      }
      .detalle-manga-admin-wrap {
        padding-right: 0;
        margin: 40px 0 32px 0;
      }
    }
    @media (max-width:600px){
      .btn-volver{top:10px;left:8px;font-size:1em;padding:9px 13px;}
    }
  </style>
</head>
<body>
<a href="catalogo_admin.php" class="btn-volver" title="Volver">
  <span style="font-size:1.4em;vertical-align:-2px;">←</span> Volver
</a>

<div class="detalle-manga-admin-wrap">
  <!-- BOTONES FUERA DE LA CARD PERO ALINEADOS -->
  <div class="card-btns-externos">
    <a href="editar_manga.php?id=<?= $mangaId ?>" class="fab-btn" title="Editar manga">✏️</a>
    <form action="eliminar_manga.php" method="POST" style="margin:0;" onsubmit="return confirm('¿Seguro que deseas eliminar este manga? Esta acción no se puede deshacer.');">
      <input type="hidden" name="manga_id" value="<?= $mangaId ?>">
      <button type="submit" class="fab-btn delete" title="Eliminar manga">🗑️</button>
    </form>
  </div>
  <div class="detalle-manga-admin-card">
    <div class="detalle-manga-top">
      <img src="<?= htmlspecialchars($urlPortada); ?>" alt="Portada de <?= htmlspecialchars($titulo); ?>" class="detalle-manga-portada">
      <div class="detalle-manga-info">
        <h1><?= htmlspecialchars($titulo); ?></h1>
        <span class="badge-genero"><?= htmlspecialchars($genero); ?></span>
        <div class="datos-sec">
          <span>Autor: <?= htmlspecialchars($autor); ?></span><br>
          <span>Publicado: <?= $fechaPublicacion ? $fechaPublicacion->format('d/m/Y') : 'Desconocida'; ?></span>
        </div>
        <h3>Sinopsis</h3>
        <p><?= nl2br(htmlspecialchars($descripcion)); ?></p>
      </div>
    </div>

    <div class="tomos-section">
      <h2>Tomos disponibles</h2>
      <?php if (!empty($archivos)): ?>
        <?php foreach ($archivos as $idx => $file):
          $nombre = htmlspecialchars($file['name']);
          $idPDF  = $file['id'];
          $linkGV = htmlspecialchars($file['webViewLink']);
          $esPremiumTomo = str_starts_with($file['name'], '[P]');
        ?>
          <div class="tomo-card">
            <div class="tomo-info">
              <span class="tomo-title"><?= $nombre ?></span>
              <?php if ($esPremiumTomo): ?>
                <span class="premium-badge">Premium</span>
              <?php else: ?>
                <span class="nopremium-badge">No Premium</span>
              <?php endif; ?>
            </div>
            <div class="tomo-links">
              <a href="<?= $linkGV ?>" target="_blank">Ver en Google Drive</a>
              <a href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $idx ?>&id=<?= $idPDF ?>">Leer aquí</a>
            </div>
          </div>
        <?php endforeach; ?>
      <?php else: ?>
        <p style="color:#ccc;">No hay tomos disponibles en esta carpeta.</p>
      <?php endif; ?>
    </div>
  </div>
</div>
<footer>
  <p style="text-align:center;color:#aaa;padding:42px 0 8px;">&copy; 2025 Manga Verse — Panel Admin</p>
</footer>
</body>
</html>
