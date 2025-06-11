<?php
/*─────────────────────────────────────────────────────────────
 * admin_datos.php   –   Exportar / Importar mangas
 *     • SQL Server (sqlsrv)
 *     • Google Drive API v3 (cuenta de servicio)
 *────────────────────────────────────────────────────────────*/
session_start();
require_once '../../Config/db.php';
require_once __DIR__ . '/../../vendor/autoload.php';
$client = require __DIR__ . '/../../drive_auth_admin.php';
$drive  = new Google\Service\Drive($client);

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: ../../Public/login.html");
    exit();
}

/*──────── Rutas de portadas ─────────────────────────────────*/
const COVER_DIR = __DIR__ . '/../../assets/imgs/covers/';   // disco
const COVER_WEB = '/Pagina_Proyecto/assets/imgs/covers/';   // url pública

/*──────── Utilidades genéricas ─────────────────────────────*/
function slug(string $txt): string
{
    $txt = preg_replace('~[^\pL\d]+~u', '-', $txt);
    $txt = iconv('utf-8', 'ascii//TRANSLIT', $txt);
    return strtolower(trim(preg_replace('~[^-\w]+~', '', $txt), '-'));
}
function esc($txt) { return str_replace("'", "''", $txt ?? ''); }

/*──────── Saber si ya existe ───────────────────────────────*/
function mangaYaExiste(string $titulo, $conn): bool
{
    $stmt = sqlsrv_query(
        $conn,
        "SELECT 1 FROM Mangas WHERE LOWER(RTRIM(LTRIM(Titulo))) = ?",
        [strtolower(trim($titulo))]
    );
    return (bool) sqlsrv_fetch_array($stmt);
}

/*──────── Sacar ID de Drive de cualquier URL ───────────────*/
function driveIdFromUrl(string $url): ?string
{
    // https://drive.google.com/uc?export=view&id=ID
    if (preg_match('/[?&]id=([^&]+)/', $url, $m))           return $m[1];

    // https://drive.google.com/file/d/ID/view
    if (preg_match('~/file/d/([^/]+)~', $url, $m))          return $m[1];

    // https://drive.google.com/open?id=ID
    if (preg_match('~/open\?id=([^&]+)~', $url, $m))        return $m[1];

    // Compartido desde la app móvil: https://drive.google.com/uc?id=ID&export=download
    if (preg_match('/\/uc\?.*id=([^&]+)/', $url, $m))       return $m[1];

    return null;
}

/*──────── Descargar imagen desde Drive ─────────────────────*/
/**
 * Descarga una imagen de Drive.
 *  • 1º  intenta vía API (requiere permiso de la cuenta de servicio)
 *  • 2º  si la API responde 404/403, intenta con el enlace público
 *        https://drive.google.com/uc?export=download&id=ID
 *  • 3º  si el enlace público tampoco sirve, lanza excepción
 *
 * Devuelve la extensión real ('jpg' o 'png').
 */
function descargarImagenDrive(string $url, string $dest): string
{
    global $drive;

    /* a)  Detectar enlace ya “lh3.googleusercontent.com” ------------- */
    if (preg_match('~^(https://)?lh3\.googleusercontent\.com/~', $url)) {
        $raw = @file_get_contents($url);
        if ($raw === false) throw new Exception('No se pudo descargar imagen pública.');
        file_put_contents($dest . '.jpg', $raw);
        return 'jpg';
    }

    /* b)  Obtener ID -------------------------------------------------- */
    $id = driveIdFromUrl($url);
    if (!$id) throw new Exception('ID de Drive no válido: ' . $url);

    /* c)  Intentar API de Drive -------------------------------------- */
    try {
        $meta = $drive->files->get($id, ['fields' => 'mimeType']);
        $mime = $meta->mimeType;
        if (!in_array($mime, ['image/jpeg', 'image/png'])) {
            throw new Exception("El archivo no es JPG/PNG (mime=$mime).");
        }
        $raw = $drive->files->get($id, ['alt' => 'media'])->getBody()->getContents();
        if (!$raw || strlen($raw) < 500) {
            throw new Exception('Descarga vacía o muy pequeña por API.');
        }
    } catch (\Google\Service\Exception $e) {
        /* 404 o 403 → probamos enlace público ------------------------ */
        if (in_array($e->getCode(), [403, 404])) {
           $public = 'https://drive.google.com/uc?export=download&id=' . $id;
            $raw    = @file_get_contents($public);

            if (!$raw || strlen($raw) < 500) {
                /* ——— FALLBACK: usar la portada genérica ——— */
                $def = COVER_DIR . 'no_portada.png';         // asegúrate de que exista
                if (!is_file($def)) {
                    throw new Exception('Portada genérica no encontrada: ' . $def);
                }
                copy($def, $dest . '.png');
                return 'png';
            }
            /* Necesitamos averiguar mime a partir del binario */
            $mime = finfo_buffer(finfo_open(FILEINFO_MIME_TYPE), $raw);
        } else {
            throw $e;   // error distinto → lo re-lanzamos
        }
    }

    /* d)  Elegir extensión y guardar --------------------------------- */
    $ext = ($mime === 'image/png') ? 'png' : 'jpg';
    if (!is_dir(dirname($dest))) mkdir(dirname($dest), 0775, true);
    file_put_contents($dest . '.' . $ext, $raw);
    return $ext;
}

/*──────── Extraer Título y URLPortada de un script SQL ─────
 * Usa str_getcsv para soportar comas y saltos de línea dentro
 * de las columnas tipo texto.                                */
function extraerTituloYPortada(string $sql): array
{
    $pat = '/INSERT\s+INTO\s+\[?Mangas\]?\s*\((.*?)\)\s*VALUES\s*\((.*?)\)/is';
    if (!preg_match($pat, $sql, $m)) return [null, null];

    $cols = array_map('trim', explode(',', $m[1]));
    $vals = str_getcsv($m[2], ',', "'");

    $tit = $port = null;
    foreach ($cols as $i => $c) {
        $c = strtolower(trim($c, '[] '));
        $v = isset($vals[$i]) ? trim($vals[$i]) : '';
        if (stripos($v, "N'") === 0) $v = substr($v, 2);
        $v = trim($v, "'");
        if ($c === 'titulo')       $tit  = $v;
        if ($c === 'urlportada')   $port = $v;
    }
    return [$tit, $port];
}

/*──────── Ejecutar script por bloques (;) ───────────────────*/
function ejecutarScriptPorBloques(string $sql, $conn): void
{
    $find = [
        '/DECLARE\s+@NewMangaID\s+INT\s*;/i',
        '/SET\s+@NewMangaID\s+=\s+SCOPE_IDENTITY\(\)\s*;/i',
        '/@NewMangaID/i',
        '/IF EXISTS\s*\(.*?\)\s*BEGIN\s*.*?RETURN;.*?END\s*/is',
    ];
    $repl = ['', '', 'SCOPE_IDENTITY()', ''];
    $sql  = preg_replace($find, $repl, $sql);

    foreach (array_filter(array_map('trim', explode(';', $sql))) as $b) {
        if ($b !== '') {
            if (!sqlsrv_query($conn, $b)) {
                throw new Exception(print_r(sqlsrv_errors(), true));
            }
        }
    }
}

/*──────── Generar script de exportación ─────────────────────*/
function scriptManga(int $id, $conn): string
{
    $q = "SELECT Titulo, Autor, Descripcion, Estado,
                 CONVERT(date, FechaPublicacion) AS Fec,
                 URLMangaDrive, URLPortada, GeneroID
          FROM Mangas WHERE MangaID = ?";
    $st = sqlsrv_query($conn, $q, [$id]);
    if (!$st || !($r = sqlsrv_fetch_array($st, SQLSRV_FETCH_ASSOC))) return '';

    extract($r);
    $titulo = esc($Titulo);
    $autor  = esc($Autor);
    $desc   = esc($Descripcion);
    $estado = esc($Estado);
    $fecha  = $Fec->format('Y-m-d');
    $drive  = esc($URLMangaDrive);
    $port   = esc($URLPortada);
    $genId  = (int) $GeneroID;

    $s  = "\n/* === $titulo === */\n";
    $s .= "IF EXISTS (SELECT 1 FROM Mangas WHERE Titulo = N'$titulo')\nBEGIN\n";
    $s .= "  PRINT 'Manga \"$titulo\" ya existe — se omitió.';\n  RETURN;\nEND\n";
    $s .= "DECLARE @NewMangaID INT;\n";
    $s .= "INSERT INTO Mangas (Titulo, Autor, Descripcion, Estado, FechaPublicacion,\n";
    $s .= "                     URLMangaDrive, URLPortada, GeneroID)\n";
    $s .= "VALUES (N'$titulo', N'$autor', N'$desc', N'$estado', '$fecha',\n";
    $s .= "        N'$drive', N'$port', $genId);\n";
    $s .= "SET @NewMangaID = SCOPE_IDENTITY();\n";

    $alts = sqlsrv_query($conn,
        "SELECT TituloAlternativo FROM TitulosAlternativos WHERE MangaID = ?", [$id]);
    while ($a = sqlsrv_fetch_array($alts, SQLSRV_FETCH_ASSOC)) {
        $ta = esc($a['TituloAlternativo']);
        $s .= "INSERT INTO TitulosAlternativos (MangaID, TituloAlternativo)\n";
        $s .= "VALUES (@NewMangaID, N'$ta');\n";
    }
    return $s;
}

/*─────────────────────── EXPORTAR ───────────────────────────*/
if (isset($_POST['export_all'])) {
    $big = "/* EXPORT MASIVO */\nBEGIN TRANSACTION;\n";
    $lst = sqlsrv_query($conn, "SELECT MangaID FROM Mangas");
    while ($r = sqlsrv_fetch_array($lst, SQLSRV_FETCH_ASSOC)) {
        $big .= scriptManga((int) $r['MangaID'], $conn);
    }
    $big .= "COMMIT;\n";
    header('Content-Type: text/sql');
    header('Content-Disposition: attachment; filename="mangas_' . date('Ymd_His') . '.sql"');
    echo $big;
    exit;
}
if (isset($_POST['export_id'])) {
    $id  = (int) $_POST['export_id'];
    $sql = scriptManga($id, $conn);
    header('Content-Type: text/sql');
    header("Content-Disposition: attachment; filename=\"manga_$id.sql\"");
    echo $sql;
    exit;
}

/*─────────────────────── IMPORTAR ───────────────────────────*/
if ($_SERVER['REQUEST_METHOD'] === 'POST'
    && isset($_POST['importar'])
    && isset($_FILES['sql_file'])) {

    $tmp  = $_FILES['sql_file']['tmp_name'];
    $name = $_FILES['sql_file']['name'];
    $sql  = file_get_contents($tmp);
    [$titulo, $urlPort] = extraerTituloYPortada($sql);

    $flash = '';
    if (!$titulo || !$urlPort) {
        $flash = "❌ Script inválido: falta Título o URLPortada.";
    } elseif (mangaYaExiste($titulo, $conn)) {
        $flash = "⚠️ El manga '$titulo' ya existe — se omitió.";
    } else {
        try {
            /* 1. Descargar portada */
            $slug = slug($titulo);
            $dest = COVER_DIR . $slug;           // sin extensión aún
            $ext  = descargarImagenDrive($urlPort, $dest);
            $web  = COVER_WEB . $slug . '.' . $ext;

            /* 2. Ejecutar script */
            ejecutarScriptPorBloques($sql, $conn);

            /* 3. Actualizar URLPortadaWeb */
            $st = sqlsrv_query(
                $conn,
                "SELECT MangaID FROM Mangas WHERE LOWER(RTRIM(LTRIM(Titulo))) = ?",
                [strtolower(trim($titulo))]
            );
            if ($m = sqlsrv_fetch_array($st, SQLSRV_FETCH_ASSOC)) {
                sqlsrv_query(
                    $conn,
                    "UPDATE Mangas SET URLPortadaWeb = ? WHERE MangaID = ?",
                    [$web, $m['MangaID']]
                );
            }
            $flash = "✅ '$name' importado correctamente.";
        } catch (Exception $e) {
            $flash = "❌ Error al importar: " . $e->getMessage();
        }
    }

    $_SESSION['flash'] = $flash;
    header("Location: " . $_SERVER['PHP_SELF']);
    exit;
}

/*────────── Mensaje Flash ──────────*/
if (!empty($_SESSION['flash'])) {
    echo "<script>alert(" . json_encode($_SESSION['flash']) . ");</script>";
    unset($_SESSION['flash']);
}
?>
<!DOCTYPE html>
<html lang="es" data-theme="dark">
<head>
    <meta charset="UTF-8">
    <title>Exportar / Importar Mangas - Manga Verse</title>
    <link rel="stylesheet" href="../../assets/css/style.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css"/>
    <style>
         body {
      min-height: 100vh;
      margin: 0;
      padding: 0;
      box-sizing: border-box;
      font-family: 'Roboto', Arial, sans-serif;
      background: var(--bg-primary);
      color: var(--text-primary);
        }
        .admin-container {
            max-width: 800px;
            margin: 80px auto 50px;
            padding: 0 20px;
        }

        h1 {
            text-align: center;
            color: var(--accent-color);
            margin-bottom: 30px;
            font-size: 2.2rem;
        }

        h2 {
            color: var(--accent-color);
            text-align: center;
            margin-bottom: 20px;
            font-size: 1.5rem;
        }

        .panel {
            background: var(--bg-card);
            padding: 25px;
            border-radius: 12px;
            margin-bottom: 30px;
            box-shadow: 0 8px 16px rgba(0,0,0,0.1);
            border: 1px solid var(--input-border);
        }

        button {
            background: var(--button-primary);
            color: white;
            border: none;
            padding: 12px 24px;
            font-weight: bold;
            font-size: 1rem;
            border-radius: 8px;
            cursor: pointer;
            transition: all 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }

        button:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        }

        input[type="file"] {
            width: 100%;
            margin: 15px 0;
            padding: 12px;
            border-radius: 8px;
            border: 1px solid var(--input-border);
            background: var(--input-bg);
            color: var(--text-primary);
            transition: border-color 0.3s ease;
        }

        input[type="file"]:focus {
            outline: none;
            border-color: var(--accent-color);
            box-shadow: 0 0 0 2px rgba(138, 43, 226, 0.2);
        }

        .manga-list {
            max-height: 400px;
            overflow-y: auto;
            margin: 20px 0;
            padding-right: 10px;
        }

        .manga-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 15px;
            margin-bottom: 10px;
            background: var(--bg-secondary);
            border-radius: 8px;
            border: 1px solid var(--input-border);
            transition: all 0.3s ease;
        }

        .manga-item:hover {
            background: var(--bg-secondary);
            transform: translateX(5px);
        }

        .manga-title {
            flex-grow: 1;
            color: var(--text-primary);
            font-weight: 500;
        }

        .btn-volver {
            position: fixed;
            top: 30px;
            left: 30px;
            background: var(--bg-card);
            color: var(--accent-color);
            border-radius: 12px;
            padding: 12px 24px 12px 18px;
            font-weight: 700;
            font-size: 1.1rem;
            text-decoration: none;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
            border: 2px solid var(--accent-color);
            transition: all 0.3s ease;
            z-index: 10;
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }

        .btn-volver:hover, .btn-volver:focus {
            background: var(--accent-color);
            color: var(--button-text-color);
            box-shadow: 0 8px 24px rgba(138, 43, 226, 0.3);
            transform: translateY(-2px);
            outline: none;
        }

        .theme-switcher {
            position: fixed;
            top: 30px;
            right: 30px;
            z-index: 10;
        }

        .import-form {
            display: flex;
            flex-direction: column;
            gap: 15px;
        }

        .panel-actions {
            display: flex;
            justify-content: center;
            gap: 15px;
            margin-top: 20px;
        }

        @media (max-width: 768px) {
            .admin-container {
                margin-top: 100px;
            }
            
            .btn-volver, .theme-switcher {
                top: 15px;
            }
            
            .btn-volver {
                left: 15px;
                padding: 10px 15px;
                font-size: 1rem;
            }
            
            .theme-switcher {
                right: 15px;
            }
            
            .manga-item {
                flex-direction: column;
                align-items: flex-start;
                gap: 10px;
            }
            
            .panel-actions {
                flex-direction: column;
                gap: 10px;
            }
        }
    </style>
</head>
<body>
<a href="admin_dashboard.php" class="btn-volver" title="Volver al dashboard">
    <i class="fas fa-arrow-left"></i> Volver
</a>

<div class="theme-switcher">
    <button id="theme-toggle" aria-label="Cambiar tema">
        <span class="dark-icon">🌙</span><span class="light-icon">☀️</span>
    </button>
</div>

<div class="admin-container">
    <h1><i class="fas fa-database"></i> Gestión de Datos de Mangas</h1>

    <div class="panel">
        <h2><i class="fas fa-file-export"></i> Exportar todos los mangas</h2>
        <p style="text-align: center; color: var(--text-secondary); margin-bottom: 20px;">
            Genera un archivo SQL con todos los mangas del sistema
        </p>
        <div style="text-align: center;">
            <form method="post">
                <button type="submit" name="export_all">
                    <i class="fas fa-file-export"></i> Exportar Todo
                </button>
            </form>
        </div>
    </div>

    <div class="panel">
        <h2><i class="fas fa-file-export"></i> Exportar manga individual</h2>
        <p style="text-align: center; color: var(--text-secondary); margin-bottom: 20px;">
            Selecciona un manga para exportar su estructura completa
        </p>
        <div class="manga-list">
            <?php
            $stmt = sqlsrv_query($conn, "SELECT MangaID, Titulo FROM Mangas ORDER BY Titulo");
            while ($m = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
                $id = $m['MangaID'];
                $titulo = htmlspecialchars($m['Titulo']);
                echo "<div class='manga-item'>
                        <span class='manga-title'>{$titulo}</span>
                        <form method='post'>
                            <input type='hidden' name='export_id' value='{$id}'>
                            <button type='submit'>
                                <i class='fas fa-download'></i> Exportar
                            </button>
                        </form>
                      </div>";
            }
            ?>
        </div>
    </div>

    <div class="panel">
        <h2><i class="fas fa-file-import"></i> Importar script SQL</h2>
        <p style="text-align: center; color: var(--text-secondary); margin-bottom: 20px;">
            Importa un manga desde un archivo SQL con su estructura completa
        </p>
        <form method="post" enctype="multipart/form-data" class="import-form">
            <input type="file" name="sql_file" accept=".sql" required>
            <div class="panel-actions">
                <button type="submit" name="importar">
                    <i class="fas fa-upload"></i> Importar Manga
                </button>
            </div>
        </form>
    </div>
</div>

<script src="../../assets/js/theme-switcher.js"></script>
</body>
</html>
