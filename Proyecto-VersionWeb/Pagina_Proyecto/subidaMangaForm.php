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
<html lang="es">

<head>
    <meta charset="UTF-8">
    <title>Subir Manga (Admin)</title>
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            background-color: #0d1530;
            color: #f5f6fa;
            padding: 2rem;
        }

        h1, h2 {
            color: #00ffff;
            text-align: center;
        }

        form {
            background: #1c2541;
            padding: 2rem;
            border-radius: 12px;
            max-width: 700px;
            margin-inline: auto;
            box-shadow: 0 0 12px #0006;
        }

        input,
        textarea,
        select {
            width: 100%;
            margin-bottom: 12px;
            padding: 10px;
            border-radius: 6px;
            border: none;
            background: #1e2a4a;
            color: #f5f6fa;
        }

        button {
            background: #00bcd4;
            color: white;
            border: none;
            padding: 10px 20px;
            font-weight: bold;
            font-size: 15px;
            border-radius: 8px;
            cursor: pointer;
            transition: 0.2s;
        }

        button:hover {
            background: #1de9b6;
        }

        input[type="file"] {
            margin-top: 10px;
        }

        .volver {
            position: absolute;
            top: 20px;
            left: 30px;
            background: #0f213b;
            color: #00eaff;
            padding: 10px 18px;
            text-decoration: none;
            border-radius: 8px;
            font-weight: bold;
            box-shadow: 0 0 8px #00eaff55;
            transition: 0.3s;
        }

        .volver:hover {
            background: #173153;
            color: #66ffff;
        }
    </style>
</head>

<body>
    <a href="catalogo_admin.php" class="volver">← Volver</a>

    <h2>📤 Subir nuevo manga</h2>
    <form method="POST" enctype="multipart/form-data">
        <label>Título</label> <input name="titulo" required>
        <label>Autor</label> <input name="autor" required>
        <label>Descripción</label> <textarea name="descripcion" required></textarea>
        <label>Estado</label>
        <select name="estado">
            <option>En emisión</option>
            <option>Finalizado</option>
            <option>Cancelado</option>
        </select>
        <label>Fecha de publicación</label><input type="date" name="fecha">
        <label>Género</label>
        <select name="genero" required>
            <?php
            $r = sqlsrv_query($conn, "SELECT GeneroID,Nombre FROM Generos");
            while ($g = sqlsrv_fetch_array($r, SQLSRV_FETCH_ASSOC)) {
                echo "<option value='{$g['GeneroID']}'>{$g['Nombre']}</option>";
            }
            ?>
        </select>
        <label>Portada (.jpg / .png)</label><input type="file" name="portada" accept="image/*" required>
        <label>Tomos (.pdf)</label> <input type="file" name="tomos[]" accept="application/pdf" multiple required>
        <button>📥 Subir manga</button>
    </form>
</body>

</html>