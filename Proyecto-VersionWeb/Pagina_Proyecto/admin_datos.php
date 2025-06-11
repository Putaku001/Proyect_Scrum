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
<html lang="es" data-theme="dark">
<head>
    <meta charset="UTF-8">
    <title>Exportar / Importar Mangas - Manga Verse</title>
    <link rel="stylesheet" href="./css/style.css">
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

<script src="./js/theme-switcher.js"></script>
</body>
</html>
