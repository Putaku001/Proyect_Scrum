<?php
session_start();
require_once 'db.php';

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: login.html");
    exit();
}

function slug(string $txt): string {
    $txt = preg_replace('~[^\pL\d]+~u', '-', $txt);
    $txt = iconv('utf-8', 'ascii//TRANSLIT', $txt);
    return strtolower(preg_replace('~[^-\w]+~', '', $txt));
}

function mangaYaExiste($titulo, $conn): bool {
    $stmt = sqlsrv_query($conn, "SELECT 1 FROM Mangas WHERE LOWER(RTRIM(LTRIM(Titulo))) = ?", [strtolower(trim($titulo))]);
    return sqlsrv_fetch_array($stmt) ? true : false;
}

function extraerTituloYPortada($script): array {
    $pattern = "/INSERT INTO Mangas\s*\((.*?)\)\s*VALUES\s*\((.*?)\)/is";
    if (preg_match($pattern, $script, $match)) {
        $columnas = array_map('trim', explode(',', $match[1]));
        $valores  = array_map('trim', explode(',', $match[2]));

        $titulo = '';
        $portada = '';

        foreach ($columnas as $i => $col) {
            $col = strtolower($col);
            $valor = trim($valores[$i], " N'");
            if ($col === 'titulo') $titulo = $valor;
            if ($col === 'urlportada') $portada = $valor;
        }
        return [$titulo, $portada];
    }
    return [null, null];
}

function descargarImagenDrive($urlDrive, $guardarComo) {
    if (!preg_match('/id=([a-zA-Z0-9_-]+)/', $urlDrive, $idMatch)) {
        throw new Exception("No se encontró ID válido en la URL de portada.");
    }

    $id = $idMatch[1];
    $downloadUrl = "https://drive.google.com/uc?export=download&id=$id";

    $ch = curl_init($downloadUrl);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_FOLLOWLOCATION, true);
    curl_setopt($ch, CURLOPT_USERAGENT, 'Mozilla/5.0');
    curl_setopt($ch, CURLOPT_COOKIEFILE, "");
    $imageData = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if (!$imageData || $httpCode !== 200 || strlen($imageData) < 1000) {
        throw new Exception("No se pudo descargar la portada desde Drive (HTTP $httpCode).");
    }

    if (!is_dir(dirname($guardarComo))) {
        mkdir(dirname($guardarComo), 0775, true);
    }

    file_put_contents($guardarComo, $imageData);
}

function ejecutarScriptPorBloques($script, $conn) {
    // ❌ ELIMINAR cualquier DECLARE/SET de @NewMangaID
    $script = preg_replace("/DECLARE\s+@NewMangaID\s+INT\s*;/i", "", $script);
    $script = preg_replace("/SET\s+@NewMangaID\s+=\s+SCOPE_IDENTITY\s*\(\s*\)\s*;/i", "", $script);
    $script = preg_replace("/@NewMangaID/i", "SCOPE_IDENTITY()", $script);
    $script = preg_replace("/IF EXISTS\s*\(.*?\)\s*BEGIN\s*.*?RETURN;.*?END\s*/is", "", $script);

    $bloques = array_filter(array_map('trim', explode(';', $script)));
    foreach ($bloques as $sql) {
        if ($sql !== '') {
            $stmt = sqlsrv_query($conn, $sql);
            if (!$stmt) throw new Exception(print_r(sqlsrv_errors(), true));
        }
    }
}

// ─── Exportar todos los mangas ───
if (isset($_POST['export_all'])) {
    $sql = "SELECT MangaID FROM Mangas";
    $stmt = sqlsrv_query($conn, $sql);
    $script = "/* === EXPORTACIÓN MASIVA === */\nBEGIN TRANSACTION;\n";

    while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
        $script .= generarScriptManga($row['MangaID'], $conn);
    }

    $script .= "COMMIT;\n";
    header("Content-Type: text/sql");
    header("Content-Disposition: attachment; filename=\"mangas_" . date("Ymd_His") . ".sql\"");
    echo $script;
    exit();
}

// ─── Exportar individual ───
if (isset($_POST['export_id'])) {
    $id = (int) $_POST['export_id'];
    $script = generarScriptManga($id, $conn);
    header("Content-Type: text/sql");
    header("Content-Disposition: attachment; filename=\"manga_$id.sql\"");
    echo $script;
    exit();
}
function esc($txt) {
    return str_replace("'", "''", $txt ?? '');
}

function generarScriptManga($id, $conn) {
    $sql = "SELECT Titulo, Autor, Descripcion, Estado,
                   CONVERT(date, FechaPublicacion) AS Fec,
                   URLMangaDrive, URLPortada, GeneroID
            FROM Mangas WHERE MangaID = ?";
    $stmt = sqlsrv_query($conn, $sql, [$id]);
    if (!$stmt || !($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC))) return '';

    extract($row);
    $titulo = esc($Titulo);
    $autor = esc($Autor);
    $descripcion = esc($Descripcion);
    $estado = esc($Estado);
    $fecha = $Fec->format('Y-m-d');
    $urlDrive = esc($URLMangaDrive);
    $urlPortada = esc($URLPortada);
    $generoID = (int)$GeneroID;

    $script = "\n/* === $titulo === */\n";
    $script .= "IF EXISTS (SELECT 1 FROM Mangas WHERE Titulo = N'$titulo')\nBEGIN\n";
    $script .= "    PRINT 'Manga \"$titulo\" ya existe — se omitió.';\n    RETURN;\nEND\n";
    $script .= "DECLARE @NewMangaID INT;\n";
    $script .= "INSERT INTO Mangas (Titulo, Autor, Descripcion, Estado, FechaPublicacion, URLMangaDrive, URLPortada, GeneroID)\n";
    $script .= "VALUES (N'$titulo', N'$autor', N'$descripcion', N'$estado', '$fecha', N'$urlDrive', N'$urlPortada', $generoID);\n";
    $script .= "SET @NewMangaID = SCOPE_IDENTITY();\n";

    $altTitles = sqlsrv_query($conn, "SELECT TituloAlternativo FROM TitulosAlternativos WHERE MangaID = ?", [$id]);
    while ($alt = sqlsrv_fetch_array($altTitles, SQLSRV_FETCH_ASSOC)) {
        $ta = esc($alt['TituloAlternativo']);
        $script .= "INSERT INTO TitulosAlternativos (MangaID, TituloAlternativo)\n";
        $script .= "VALUES (@NewMangaID, N'$ta');\n";
    }

    return $script;
}


/* ───── ACCIONES ───── */
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_POST['importar']) && isset($_FILES['sql_file'])) {
        $file = $_FILES['sql_file']['tmp_name'];
        $name = $_FILES['sql_file']['name'];
        $content = file_get_contents($file);
        $msg = "";

        [$titulo, $urlDrive] = extraerTituloYPortada($content);

        if (!$titulo || !$urlDrive) {
            $msg = "❌ Error al importar: Falta título o URL de portada.";
        } else if (mangaYaExiste($titulo, $conn)) {
            $msg = "⚠️ El manga '$titulo' ya existe. Se omitió.";
        } else {
            try {
                $slug = slug($titulo);
                $relativePath = "imgs/covers/$slug.jpg";
                $absolutePath = __DIR__ . "/$relativePath";

                descargarImagenDrive($urlDrive, $absolutePath); // ✅ Descargar imagen

                ejecutarScriptPorBloques($content, $conn); // ✅ Insertar

                $stmt = sqlsrv_query($conn, "SELECT MangaID FROM Mangas WHERE LOWER(RTRIM(LTRIM(Titulo))) = ?", [strtolower(trim($titulo))]);
                if ($manga = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
                    $id = $manga['MangaID'];
                    sqlsrv_query($conn,
                        "UPDATE Mangas SET URLPortadaWeb = ? WHERE MangaID = ?",
                        [$relativePath, $id]
                    );
                }

                $msg = "✅ Script '$name' importado correctamente.";
            } catch (Exception $e) {
                $msg = "❌ Error al importar: " . $e->getMessage();
            }
        }

        echo "<script>alert(" . json_encode($msg) . "); window.location.href=window.location.href;</script>";
        exit();
    }
}
?>



<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Exportar / Importar Mangas</title>
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

    .panel {
        background: #1c2541;
        padding: 2rem;
        border-radius: 12px;
        margin-bottom: 30px;
        max-width: 700px;
        margin-inline: auto;
        box-shadow: 0 0 12px #0006;
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
        color: #eee;
        background: #1e2a4a;
        border: 1px solid #2c3e50;
        padding: 8px;
        border-radius: 6px;
    }

    form {
        display: inline;
    }

    .manga-btn {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin: 6px 0;
        border-bottom: 1px solid #2e3d5c;
        padding-bottom: 6px;
        color: #eee;
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

<a href="admin_dashboard.php" class="volver">← Volver</a>

<h1 style="text-align:center;">📦 Gestión de Mangas (.SQL)</h1>

<div class="panel">
    <h2>Exportar todos los mangas</h2>
    <form method="post">
        <button name="export_all">📤 Exportar TODO</button>
    </form>
</div>

<div class="panel">
    <h2>Exportar manga individual</h2>
    <?php
    $stmt = sqlsrv_query($conn, "SELECT MangaID, Titulo FROM Mangas ORDER BY Titulo");
    while ($m = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
        $id = $m['MangaID'];
        $titulo = htmlspecialchars($m['Titulo']);
        echo "<div class='manga-btn'>
                <span>$titulo</span>
                <form method='post'>
                    <input type='hidden' name='export_id' value='$id'>
                    <button>Exportar</button>
                </form>
              </div>";
    }
    ?>
</div>

<div class="panel">
    <h2>Importar script SQL</h2>
    <form method="post" enctype="multipart/form-data">
        <input type="file" name="sql_file" required>
        <br><br>
        <button name="importar">📥 Importar</button>
    </form>
</div>

</body>
</html>
