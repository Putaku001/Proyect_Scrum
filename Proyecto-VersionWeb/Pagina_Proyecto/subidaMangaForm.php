<?php
/*-------------------------------------------------------------
 * subidaMangaForm.php  – CRUD Mangas  (admin)
 *   • SQL Server (sqlsrv)
 *   • Google Drive API v3   (google/apiclient)
 *------------------------------------------------------------*/
if (session_status() === PHP_SESSION_NONE)
    session_start();

require_once 'db.php';                               // $conn
require_once __DIR__ . '/vendor/autoload.php';         // Google SDK
$client = require 'drive_auth_admin.php';            // ← tu Google_Client
$drive = new Google\Service\Drive($client);

/*─────────── CONSTANTES ────────────────────────────────────*/
const COVER_DIR = __DIR__ . '/imgs/covers';          // destino local
const COVER_PATH = 'imgs/covers/';                  // ruta que guarda la BD
const DRIVE_ROOT = '1LgM-Yh70-ShdG4jT96DuxMEGn1L3MZPe';   // carpeta “mangas app”

/*─────────── UTIL ─────────────────────────────────────────*/
function slug(string $txt): string
{
    $txt = preg_replace('~[^\pL\d]+~u', '-', $txt);
    $txt = iconv('utf-8', 'ascii//TRANSLIT', $txt);
    return strtolower(preg_replace('~[^-\w]+~', '', $txt));
}
function mime_from_ext(string $file): string
{
    $ext = strtolower(pathinfo($file, PATHINFO_EXTENSION));
    return $ext === 'png' ? 'image/png' : 'image/jpeg';
}

/*─────────── SUBIDA ──────────────────────────────────────*/
if ($_SERVER['REQUEST_METHOD'] === 'POST') {

    /* 1. datos */
    $titulo = trim($_POST['titulo'] ?? '');
    
    $autor = trim($_POST['autor'] ?? '');
    $desc = trim($_POST['descripcion'] ?? '');
    $estado = trim($_POST['estado'] ?? 'En emisión');
    $genero = (int) ($_POST['genero'] ?? 0);
    $fecha = $_POST['fecha'] ?: date('Y-m-d');

    /* 2. crear carpetas Drive */
    $idManga = $drive->files->create(
        new Google\Service\Drive\DriveFile([
            'name' => $titulo,
            'mimeType' => 'application/vnd.google-apps.folder',
            'parents' => [DRIVE_ROOT]
        ]),
        ['fields' => 'id']
    )->id;

    $idPortada = $drive->files->create(
        new Google\Service\Drive\DriveFile([
            'name' => 'Portada',
            'mimeType' => 'application/vnd.google-apps.folder',
            'parents' => [$idManga]
        ]),
        ['fields' => 'id']
    )->id;

    $idVols = $drive->files->create(
        new Google\Service\Drive\DriveFile([
            'name' => 'Volumenes',
            'mimeType' => 'application/vnd.google-apps.folder',
            'parents' => [$idManga]
        ]),
        ['fields' => 'id']
    )->id;

    /* 3. ─── PORTADA (Drive + local) ───────────────────────*/
    $tmpPortada = $_FILES['portada']['tmp_name'];
    $ext = strtolower(pathinfo($_FILES['portada']['name'], PATHINFO_EXTENSION));
    $mimeImg = mime_from_ext($ext);

    /* 3A)   Drive */
    $metaImg = new Google\Service\Drive\DriveFile([
        'name' => 'cover.' . $ext,
        'parents' => [$idPortada]
    ]);
    $driveRes = $drive->files->create(
        $metaImg,
        [
            'data' => file_get_contents($tmpPortada),
            'mimeType' => $mimeImg,
            'uploadType' => 'multipart',
            'fields' => 'id'
        ]
    );
    $urlPortDrive = "https://drive.google.com/uc?export=view&id={$driveRes->id}";

    /* 3B)   Local  (/imgs/covers/slug.jpg) */
    if (!is_dir(COVER_DIR))
        mkdir(COVER_DIR, 0775, true);
    $fileLocal = slug($titulo) . '.' . $ext;
    $destLocal = COVER_DIR . '/' . $fileLocal;
    move_uploaded_file($tmpPortada, $destLocal);
    $urlPortWeb = COVER_PATH . $fileLocal;            // lo que verá la web

    /* 4. ─── TOMOS  (solo Drive) ──────────────────────────*/
    foreach ($_FILES['tomos']['tmp_name'] as $k => $tmp) {
        if (!is_uploaded_file($tmp))
            continue;
        $meta = new Google\Service\Drive\DriveFile([
            'name' => $_FILES['tomos']['name'][$k],
            'parents' => [$idVols]
        ]);
        $drive->files->create($meta, [
            'data' => file_get_contents($tmp),
            'mimeType' => 'application/pdf',
            'uploadType' => 'multipart'
        ]);
    }
    $urlVols = "https://drive.google.com/drive/folders/{$idVols}?usp=sharing";

    /* 5. ─── INSERT BD  (incluye URLPortadaWeb) ───────────*/
    $sql = "INSERT INTO Mangas
            (Titulo,Autor,Descripcion,Estado,FechaPublicacion,
             URLMangaDrive,URLPortada,URLPortadaWeb,GeneroID)
            VALUES (?,?,?,?,?,?,?,?,?)";
    $ok = sqlsrv_query(
        $conn,
        $sql,
        [
            $titulo,
            $autor,
            $desc,
            $estado,
            $fecha,
            $urlVols,
            $urlPortDrive,
            $urlPortWeb,
            $genero
        ]
    );

    if ($ok) {
        echo "<script>alert('✅ Manga subido correctamente');location='catalogo_admin.php';</script>";
    } else {
        echo "<script>alert('❌ Error al guardar en la base de datos');history.back();</script>";
    }
    exit;
}
?>
<!-- ──────────────────  FORMULARIO  ─────────────────────── -->
<!DOCTYPE html>
<html lang="es" data-theme="dark">
<head>
  <meta charset="UTF-8">
  <title>Subir Manga - Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css"/>
  <style>
    .catalog-container {
      max-width: 720px;
      margin: 100px auto 50px;
      padding: 0 20px;
      flex: 1;
    }

    .catalog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
      flex-wrap: wrap;
    }

    .catalog-header h2 {
      color: var(--accent-color);
      font-size: 1.8rem;
      margin: 0;
    }

    form {
      background: var(--bg-card);
      padding: 2rem;
      border-radius: 16px;
      box-shadow: 0 8px 24px rgba(0,0,0,0.2);
      border: 1px solid var(--input-border);
    }

    label {
      display: block;
      font-weight: bold;
      margin-top: 18px;
      margin-bottom: 6px;
      color: var(--text-secondary);
    }

    input, textarea, select {
      width: 100%;
      padding: 12px;
      border-radius: 10px;
      border: 1px solid var(--input-border);
      background: var(--input-bg);
      color: var(--text-primary);
      font-size: 1rem;
      transition: border 0.3s;
    }

    input:focus, textarea:focus, select:focus {
      border-color: var(--accent-color);
      outline: none;
      box-shadow: 0 0 0 2px rgba(138, 43, 226, 0.2);
    }

    button {
      background: var(--button-primary);
      color: white;
      padding: 12px 24px;
      margin-top: 30px;
      font-weight: bold;
      font-size: 1rem;
      border-radius: 10px;
      border: none;
      cursor: pointer;
      transition: all 0.3s ease;
    }

    button:hover {
      transform: translateY(-2px);
      box-shadow: 0 8px 16px rgba(138, 43, 226, 0.4);
    }

    #theme-toggle {
      background: var(--accent-color);
      border: none;
      border-radius: 12px;
      width: 44px;
      height: 44px;
      font-size: 1.2rem;
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      box-shadow: 0 4px 12px rgba(138, 43, 226, 0.3);
      transition: all 0.2s ease-in-out;
      padding: 0;
      margin: 0;
    }

    #theme-toggle:hover {
      transform: translateY(-1px);
      box-shadow: 0 6px 20px rgba(138, 43, 226, 0.5);
    }

    header {
      background: var(--bg-card);
      padding: 16px 24px;
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-bottom: 1px solid var(--input-border);
      z-index: 10;
      box-shadow: none !important;
    }

    .header-right,
    .header-left {
      display: flex;
      align-items: center;
      gap: 12px;
      height: 44px; /* Añadido para igualar altura */
    }

    .btn-volver {
      background: var(--bg-card);
      color: var(--accent-color);
      border-radius: 12px;
      padding: 10px 18px;
      font-weight: 700;
      font-size: 1rem;
      text-decoration: none;
      border: 2px solid var(--accent-color);
      transition: all 0.3s ease;
      height: 44px; /* Añadido para igualar altura */
      display: flex;
      align-items: center;
    }

    .btn-volver:hover {
      background: var(--accent-color);
      color: var(--button-text-color);
      box-shadow: 0 8px 24px rgba(138, 43, 226, 0.3);
      transform: translateY(-2px);
    }

    footer {
      margin-top: auto;
      text-align: center;
      color: var(--text-secondary);
      padding: 20px;
      border-top: 1px solid var(--input-border);
    }

    html, body {
      height: 100%;
      display: flex;
      flex-direction: column;
    }

    body::before {
      content: "";
      position: absolute;
      top: 0;
      left: 0;
      height: 80px;
      width: 100%;
      background: linear-gradient(to bottom, rgba(138,43,226,0.4), transparent);
      z-index: 1;
    }
  </style>
</head>
<body>
<header>
  <div class="header-left">
    <a href="catalogo_admin.php" class="btn-volver">
      <i class="fas fa-arrow-left"></i> Volver
    </a>
  </div>
  <div class="header-right">
    <button id="theme-toggle" title="Cambiar tema">🌙</button>
  </div>
</header>

  <div class="catalog-container">
    <div class="catalog-header">
      <h2>📤 Subir nuevo manga</h2>
    </div>

    <form method="POST" enctype="multipart/form-data" action="subidaMangaForm.php">
      <label>Título</label>
      <input name="titulo" required>

      <label>Autor</label>
      <input name="autor" required>

      <label>Descripción</label>
      <textarea name="descripcion" required></textarea>

      <label>Estado</label>
      <select name="estado">
        <option>En emisión</option>
        <option>Finalizado</option>
        <option>Cancelado</option>
      </select>

      <label>Fecha de publicación</label>
      <input type="date" name="fecha">

      <label>Género</label>
      <select name="genero" required>
        <?php
        $r = sqlsrv_query($conn, "SELECT GeneroID, Nombre FROM Generos");
        while ($g = sqlsrv_fetch_array($r, SQLSRV_FETCH_ASSOC)) {
          echo "<option value='{$g['GeneroID']}'>{$g['Nombre']}</option>";
        }
        ?>
      </select>

      <label>Portada (.jpg / .png)</label>
      <input type="file" name="portada" accept="image/*" required>

      <label>Tomos (.pdf)</label>
      <input type="file" name="tomos[]" accept="application/pdf" multiple required>

      <button type="submit">📥 Subir manga</button>
    </form>
  </div>

  <footer>
    <p>&copy; 2025 Manga Verse — Panel Admin</p>
  </footer>

  <script>
  const toggleBtn = document.getElementById('theme-toggle');
  toggleBtn.onclick = () => {
    const html = document.documentElement;
    const dark = html.dataset.theme === 'dark';
    html.dataset.theme = dark ? 'light' : 'dark';
    toggleBtn.textContent = dark ? '☀️' : '🌙';
  };
</script>

</body>
</html>