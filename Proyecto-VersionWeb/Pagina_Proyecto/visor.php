<?php
require_once 'db.php';
session_start();

if (!isset($_GET['manga_id']) || !isset($_GET['index'])) {
    die("Datos incompletos para mostrar el visor.");
}

$mangaId = (int) $_GET['manga_id'];
$index = (int) $_GET['index'];
$mode = $_GET['mode'] ?? 'cascade';

// Obtener datos del manga
$sql = "SELECT Titulo, URLMangaDrive FROM Mangas WHERE MangaID = ?";
$stmt = sqlsrv_query($conn, $sql, [$mangaId]);
if (!$stmt || !sqlsrv_fetch($stmt)) die("Manga no encontrado.");

$titulo = sqlsrv_get_field($stmt, 0);
$urlDrive = sqlsrv_get_field($stmt, 1);

// Función para extraer FolderId
function extraerFolderId($url) {
    if (preg_match('/\/folders\/([a-zA-Z0-9_-]+)/', $url, $matches)) return $matches[1];
    if (preg_match('/[?&]id=([a-zA-Z0-9_-]+)/', $url, $matches)) return $matches[1];
    return null;
}
$folderId = extraerFolderId($urlDrive);
if (!$folderId) die("Carpeta inválida.");

// OAuth
if (!isset($_SESSION['access_token'])) {
    $client_id = 'TU_CLIENT_ID.apps.googleusercontent.com';
    $redirect_uri = 'http://localhost/tu_ruta/google_drive.php';
    $state = json_encode(['page' => 'visor', 'manga_id' => $mangaId, 'index' => $index, 'mode' => $mode]);
    $auth_url = "https://accounts.google.com/o/oauth2/auth?" . http_build_query([
        'response_type' => 'code',
        'client_id' => $client_id,
        'redirect_uri' => $redirect_uri,
        'scope' => 'https://www.googleapis.com/auth/drive.readonly',
        'access_type' => 'offline',
        'prompt' => 'consent',
        'state' => $state
    ]);
    header("Location: $auth_url");
    exit();
}

$access_token = $_SESSION['access_token'];

// Obtener lista de PDFs
$ch = curl_init("https://www.googleapis.com/drive/v3/files?q=" . urlencode("'$folderId' in parents and mimeType='application/pdf'") . "&fields=files(id,name)&orderBy=name&pageSize=100");
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, ["Authorization: Bearer $access_token"]);
$response = curl_exec($ch);
$data = json_decode($response, true);
curl_close($ch);

$archivos = $data['files'] ?? [];

usort($archivos, function ($a, $b) {
    preg_match('/(\d+)/', $a['name'], $numA);
    preg_match('/(\d+)/', $b['name'], $numB);
    return intval($numA[1] ?? 0) - intval($numB[1] ?? 0);
});

if (!isset($archivos[$index])) die("Capítulo no encontrado.");

$capitulo = $archivos[$index];
$archivoId = $capitulo['id'];
$capituloNombre = $capitulo['name'];
$pdfUrl = "https://www.googleapis.com/drive/v3/files/$archivoId?alt=media";
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title><?= htmlspecialchars($titulo . ' - ' . $capituloNombre); ?></title>
    <script src="./js/build/pdf.js"></script>
    <style>
        body {
            background: #1c1c1c;
            color: #fff;
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
            padding: 20px;
        }
        h1 { text-align: center; margin-top: 10px; }

        .controls-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 10px;
        }

        .mode-switch, .navegacion {
            display: flex;
            gap: 12px;
            flex-wrap: wrap;
            justify-content: center;
        }

        .btn-purple {
            background: #8a2be2;
            padding: 10px 20px;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
        }

        .btn-purple:hover {
            background: #a050f0;
        }

        .volver-btn {
            position: absolute;
            top: 20px;
            left: 20px;
            background: #444;
            padding: 10px 15px;
            color: #fff;
            border-radius: 5px;
            font-weight: bold;
        }

        #pdf-canvas {
            display: block;
            margin: 20px auto;
            border: 2px solid #8a2be2;
            border-radius: 8px;
        }

        #pdf-container canvas {
            display: block;
            margin: 20px auto;
            border: 2px solid #8a2be2;
            border-radius: 8px;
        }

        .page-nav {
            text-align: center;
            margin: 20px 0;
        }

        .page-nav button {
            margin: 0 10px;
            padding: 10px 20px;
            background: #8a2be2;
            color: white;
            border: none;
            border-radius: 5px;
            font-weight: bold;
            cursor: pointer;
        }

        /* Spinner bloqueante */
        .spinner-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100vw;
            height: 100vh;
            background-color: rgba(28, 28, 28, 0.9);
            display: flex;
            justify-content: center;
            align-items: center;
            z-index: 9999;
        }

        .spinner {
            width: 40px;
            height: 40px;
            position: relative;
        }

        .double-bounce1, .double-bounce2 {
            width: 100%;
            height: 100%;
            border-radius: 50%;
            background-color: #8a2be2;
            opacity: 0.6;
            position: absolute;
            animation: bounce 2s infinite ease-in-out;
        }

        .double-bounce2 {
            animation-delay: -1s;
        }

        @keyframes bounce {
            0%, 100% { transform: scale(0); }
            50% { transform: scale(1); }
        }

        .hide { display: none !important; }
    </style>
</head>
<body>

<!-- Botón para volver a detalle -->
<a class="volver-btn" href="detalle_manga.php?id=<?= $mangaId ?>">← Volver</a>

<!-- Spinner bloqueante -->
<div id="spinner" class="spinner-overlay">
    <div class="spinner">
        <div class="double-bounce1"></div>
        <div class="double-bounce2"></div>
    </div>
</div>

<h1><?= htmlspecialchars($titulo) ?> - <?= htmlspecialchars($capituloNombre) ?></h1>

<div class="controls-container">
    <div class="mode-switch">
        <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index ?>&mode=book">Modo Libro</a>
        <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index ?>&mode=cascade">Modo Cascada</a>
    </div>
    <div class="navegacion">
        <?php if ($index > 0): ?>
            <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index - 1 ?>&mode=<?= $mode ?>">⬅ Capítulo anterior</a>
        <?php endif; ?>
        <?php if ($index < count($archivos) - 1): ?>
            <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index + 1 ?>&mode=<?= $mode ?>">Capítulo siguiente ➡</a>
        <?php endif; ?>
    </div>
</div>

<?php if ($mode === 'book'): ?>
    <canvas id="pdf-canvas"></canvas>
    <div class="page-nav">
        <button onclick="anteriorPagina()">⬅ Página anterior</button>
        <button onclick="siguientePagina()">➡ Página siguiente</button>
    </div>
    <script>
        pdfjsLib.GlobalWorkerOptions.workerSrc = './js/build/pdf.worker.js';
        const token = "<?= $access_token ?>";
        const pdfUrl = "<?= $pdfUrl ?>";
        let pdfDoc = null;
        let pageNum = 1;
        const spinner = document.getElementById("spinner");

        pdfjsLib.getDocument({ url: pdfUrl, httpHeaders: { Authorization: `Bearer ${token}` } }).promise.then(function(pdf) {
            pdfDoc = pdf;
            spinner.classList.add("hide");
            renderPage(pageNum);
        }).catch(err => {
            spinner.classList.add("hide");
            alert("No se pudo cargar el capítulo.");
            console.error(err);
        });

        function renderPage(num) {
            pdfDoc.getPage(num).then(function(page) {
                const scale = 1.5;
                const viewport = page.getViewport({ scale });
                const canvas = document.getElementById("pdf-canvas");
                const context = canvas.getContext("2d");
                canvas.width = viewport.width;
                canvas.height = viewport.height;
                page.render({ canvasContext: context, viewport });
            });
        }

        function siguientePagina() {
            if (pageNum < pdfDoc.numPages) {
                pageNum++;
                renderPage(pageNum);
            }
        }

        function anteriorPagina() {
            if (pageNum > 1) {
                pageNum--;
                renderPage(pageNum);
            }
        }
    </script>
<?php else: ?>
    <div id="pdf-container"></div>
    <script>
        pdfjsLib.GlobalWorkerOptions.workerSrc = './js/build/pdf.worker.js';
        const token = "<?= $access_token ?>";
        const pdfUrl = "<?= $pdfUrl ?>";
        const spinner = document.getElementById("spinner");

        pdfjsLib.getDocument({ url: pdfUrl, httpHeaders: { Authorization: `Bearer ${token}` } }).promise.then(function(pdf) {
            const container = document.getElementById("pdf-container");
            spinner.classList.add("hide");
            for (let num = 1; num <= pdf.numPages; num++) {
                pdf.getPage(num).then(function(page) {
                    const scale = 1.5;
                    const viewport = page.getViewport({ scale });
                    const canvas = document.createElement("canvas");
                    canvas.width = viewport.width;
                    canvas.height = viewport.height;
                    canvas.style.margin = "20px auto";
                    canvas.style.border = "2px solid #8a2be2";
                    canvas.style.borderRadius = "8px";
                    container.appendChild(canvas);
                    const context = canvas.getContext("2d");
                    page.render({ canvasContext: context, viewport });
                });
            }
        }).catch(err => {
            spinner.classList.add("hide");
            alert("No se pudo cargar el capítulo.");
            console.error(err);
        });
    </script>

<?php endif; ?>
<!-- Controles inferiores (modo, navegación) -->
<div class="controls-container" style="margin-top: 40px;">
    <div class="mode-switch">
        <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index ?>&mode=book">Modo Libro</a>
        <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index ?>&mode=cascade">Modo Cascada</a>
    </div>
    <div class="navegacion">
        <?php if ($index > 0): ?>
            <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index - 1 ?>&mode=<?= $mode ?>">⬅ Capítulo anterior</a>
        <?php endif; ?>
        <?php if ($index < count($archivos) - 1): ?>
            <a class="btn-purple" href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index + 1 ?>&mode=<?= $mode ?>">Capítulo siguiente ➡</a>
        <?php endif; ?>
    </div>
</div>
</body>
</html>
