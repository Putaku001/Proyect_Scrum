<?php
session_start();
require_once '../Config/db.php';
require_once '../drive_auth.php';

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: ../Public/login.html");
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
$urlPortada = '../assets/imgs/no_portada.png';
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
  <link rel="stylesheet" href="../assets/css/style.css">
  <style>
    body {
      background: var(--bg-primary);
      color: var(--text-primary);
      font-family: 'Roboto', sans-serif;
      margin: 0;
      padding: 0;
    }
    
    .btn-volver {
      position: fixed;
      top: 20px;
      left: 20px;
      background: var(--bg-card);
      color: var(--accent-color);
      border-radius: 50px;
      padding: 12px 24px;
      font-weight: 600;
      font-size: 1rem;
      text-decoration: none;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
      border: 2px solid var(--accent-color);
      transition: all 0.3s ease;
      z-index: 100;
      display: flex;
      align-items: center;
      gap: 8px;
    }
    
    .btn-volver:hover, .btn-volver:focus {
      background: var(--accent-color);
      color: var(--button-text-color);
      box-shadow: 0 6px 24px rgba(138, 43, 226, 0.4);
      transform: translateY(-2px);
    }
    
    .btn-volver span {
      font-size: 1.2em;
    }
    
    /* NUEVO CONTENEDOR PARA BOTONES SUPERIORES */
    .top-action-buttons {
      position: fixed;
      top: 20px;
      right: 20px;
      display: flex;
      gap: 15px;
      z-index: 100;
      background: var(--bg-card);
      padding: 10px 15px;
      border-radius: 50px;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
      border: 1px solid var(--input-border);
    }
    
    .action-btn {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      border: none;
      color: white;
      font-size: 1.2em;
      background: var(--button-primary);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
      cursor: pointer;
      transition: all 0.3s ease;
      outline: none;
    }
    
    .action-btn:hover {
      transform: scale(1.1) translateY(-2px);
      box-shadow: 0 6px 20px rgba(138, 43, 226, 0.4);
    }
    
    .action-btn.delete {
      background: linear-gradient(135deg, #ff4d4d, #d00000);
    }
    
    .action-btn.delete:hover {
      background: linear-gradient(135deg, #ff3333, #b80000);
    }
    
    .detalle-manga-admin-wrap {
      max-width: 1200px;
      margin: 100px auto 60px;
      padding: 0 20px;
    }
    
    .detalle-manga-admin-card {
      background: var(--bg-card);
      border-radius: 16px;
      box-shadow: 0 8px 32px rgba(0, 0, 0, 0.2);
      padding: 30px;
      position: relative;
      overflow: hidden;
      border: 1px solid var(--input-border);
    }
    
    .detalle-manga-top {
      display: flex;
      flex-wrap: wrap;
      gap: 40px;
      margin-bottom: 40px;
    }
    
    .detalle-manga-portada {
      width: 300px;
      height: 450px;
      object-fit: cover;
      border-radius: 12px;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.3);
      transition: transform 0.3s ease;
    }
    
    .detalle-manga-portada:hover {
      transform: scale(1.02);
    }
    
    .detalle-manga-info {
      flex: 1;
      min-width: 300px;
    }
    
    .detalle-manga-info h1 {
      font-size: 2.5rem;
      margin: 0 0 15px;
      color: var(--accent-color);
      line-height: 1.2;
      text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }
    
    .badge-genero {
      background: var(--button-primary);
      color: white;
      padding: 8px 20px;
      border-radius: 50px;
      font-size: 1rem;
      margin-bottom: 20px;
      font-weight: 600;
      display: inline-block;
      box-shadow: 0 4px 12px rgba(138, 43, 226, 0.3);
    }
    
    .detalle-manga-info p {
      font-size: 1.1rem;
      line-height: 1.6;
      margin: 0 0 15px;
      color: var(--text-primary);
    }
    
    .detalle-manga-info h3 {
      font-size: 1.3rem;
      color: var(--accent-color);
      margin: 25px 0 10px;
      font-weight: 600;
    }
    
    .detalle-manga-info .datos-sec {
      font-size: 1rem;
      margin-bottom: 10px;
      color: var(--text-secondary);
      font-weight: 500;
    }
    
    .tomos-section {
      margin-top: 40px;
    }
    
    .tomos-section h2 {
      font-size: 1.5rem;
      margin-bottom: 20px;
      color: var(--accent-color);
      font-weight: 600;
      position: relative;
      padding-bottom: 10px;
    }
    
    .tomos-section h2::after {
      content: '';
      position: absolute;
      bottom: 0;
      left: 0;
      width: 100px;
      height: 3px;
      background: var(--button-primary);
      border-radius: 3px;
    }
    
    .tomo-card {
      background: var(--bg-secondary);
      padding: 18px 25px;
      margin-bottom: 15px;
      border-radius: 10px;
      font-size: 1.1rem;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 20px;
      transition: all 0.3s ease;
      border: 1px solid var(--input-border);
    }
    
    .tomo-card:hover {
      transform: translateY(-3px);
      box-shadow: 0 8px 20px rgba(138, 43, 226, 0.2);
    }
    
    .tomo-info {
      display: flex;
      align-items: center;
      gap: 15px;
      flex-wrap: wrap;
    }
    
    .tomo-title {
      font-weight: 600;
      color: var(--text-primary);
    }
    
    .premium-badge {
      padding: 5px 15px;
      background: linear-gradient(135deg, #ff4dff, #8e24aa);
      color: white;
      border-radius: 50px;
      font-size: 0.9em;
      font-weight: 600;
      box-shadow: 0 4px 12px rgba(142, 36, 170, 0.3);
    }
    
    .nopremium-badge {
      padding: 5px 15px;
      background: linear-gradient(135deg, #1de9b6, #40c4ff);
      color: var(--bg-primary);
      border-radius: 50px;
      font-size: 0.9em;
      font-weight: 600;
      box-shadow: 0 4px 12px rgba(29, 233, 182, 0.3);
    }
    
    .tomo-links {
      display: flex;
      gap: 15px;
      flex-wrap: wrap;
    }
    
    .tomo-links a {
      color: var(--accent-color);
      text-decoration: none;
      font-weight: 600;
      font-size: 1em;
      transition: all 0.3s ease;
      padding: 8px 15px;
      border-radius: 6px;
      background: rgba(138, 43, 226, 0.1);
      border: 1px solid rgba(138, 43, 226, 0.3);
    }
    
    .tomo-links a:hover {
      background: rgba(138, 43, 226, 0.2);
      color: var(--text-primary);
      box-shadow: 0 4px 12px rgba(138, 43, 226, 0.2);
    }
    
    footer {
      text-align: center;
      padding: 30px;
      background: var(--bg-tertiary);
      color: var(--text-secondary);
      margin-top: 60px;
      box-shadow: 0 -8px 32px rgba(137, 129, 248, 0.1);
    }
    
    @media (max-width: 900px) {
      .detalle-manga-top {
        flex-direction: column;
        align-items: center;
        gap: 30px;
      }
      
      .detalle-manga-portada {
        width: 250px;
        height: 375px;
      }
      
      .detalle-manga-info {
        min-width: 100%;
      }
    }
    
    @media (max-width: 700px) {
      .top-action-buttons {
        top: 70px;
        right: 50%;
        transform: translateX(50%);
        padding: 8px 12px;
      }
      
      .detalle-manga-admin-wrap {
        margin: 140px auto 40px;
      }
      
      .btn-volver {
        top: 10px;
        left: 10px;
        padding: 10px 15px;
        font-size: 0.9rem;
      }
      
      .tomo-card {
        flex-direction: column;
        align-items: flex-start;
        gap: 15px;
      }
      
      .tomo-links {
        width: 100%;
        justify-content: flex-end;
      }
    }
    
    @media (max-width: 500px) {
      .detalle-manga-admin-card {
        padding: 20px;
      }
      
      .detalle-manga-info h1 {
        font-size: 2rem;
      }
      
      .tomo-links {
        flex-direction: column;
        gap: 10px;
      }
      
      .tomo-links a {
        width: 100%;
        text-align: center;
      }
      
      .top-action-buttons {
        gap: 10px;
      }
      
      .action-btn {
        width: 36px;
        height: 36px;
        font-size: 1.1em;
      }
    }
  </style>
</head>
<body>
<a href="catalogo_admin.php" class="btn-volver" title="Volver">
  <span style="font-size:1.4em;vertical-align:-2px;">←</span> Volver
</a>

<div class="detalle-manga-admin-wrap">
  <!-- Botones de acción en la parte superior derecha -->
    <div class="top-action-buttons">
      <a href="./Admin/editar_manga.php?id=<?= $mangaId ?>" class="action-btn" title="Editar manga">✏️</a>
      <form action="./Admin/eliminar_manga.php" method="POST" style="margin:0;" onsubmit="return confirm('¿Seguro que deseas eliminar este manga? Esta acción no se puede deshacer.');">
        <input type="hidden" name="manga_id" value="<?= $mangaId ?>">
        <button type="submit" class="action-btn delete" title="Eliminar manga">🗑️</button>
      </form>
      <button id="theme-toggle" class="action-btn" aria-label="Cambiar tema">
        <span class="dark-icon">🌙</span>
        <span class="light-icon">☀️</span>
      </button>
    </div>
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

<script src="../assets/js/theme-switcher.js"></script>
