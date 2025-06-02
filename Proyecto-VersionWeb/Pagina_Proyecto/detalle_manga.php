<?php
/* 1 ─ Iniciar sesión SOLAMENTE si aún no existe */
if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

require_once 'db.php';
require_once 'drive_auth.php';   // ← ya devuelve $access_token

/*────  Datos del manga  ───────────────────────────────────────────*/
if (!isset($_GET['id'])) die('Manga no especificado.');
$mangaId = (int)$_GET['id'];

$sql = "SELECT M.Titulo, M.Autor, M.Descripcion, M.FechaPublicacion,
               M.URLMangaDrive, M.URLPortada, M.URLPortadaWeb,
               G.Nombre AS Genero
        FROM   Mangas M
        LEFT JOIN Generos G ON M.GeneroID = G.GeneroID
        WHERE  MangaID = ?";
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

/*────  Portada  ──────────────────────────────────────────────────*/
$urlPortada = './imgs/no_portada.png';
if (!empty($urlPortadaWeb) && file_exists($urlPortadaWeb)) {
    $urlPortada = $urlPortadaWeb;
} elseif (!empty($urlPortadaDrive)) {
    $urlPortada = $urlPortadaDrive;
}

/*────  Extraer ID de carpeta de Drive  ───────────────────────────*/
function folderId($url) {
    if (preg_match('/\/folders\/([a-zA-Z0-9_-]+)/', $url, $m)) return $m[1];
    if (preg_match('/[?&]id=([a-zA-Z0-9_-]+)/', $url, $m))    return $m[1];
    return null;
}
$folder_id = folderId($urlDrive) ?: die('Carpeta Drive inválida.');

/*────  Listar los PDFs  ──────────────────────────────────────────*/
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

usort($archivos, function($a,$b){
    preg_match('/(\d+)/',$a['name'],$na);
    preg_match('/(\d+)/',$b['name'],$nb);
    return ($na[1]??0) - ($nb[1]??0);
});

/*────  HTML (idéntico a tu versión original) ─────────────────────*/
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title><?php echo htmlspecialchars($titulo); ?> - Manga Verse</title>
    <link rel="stylesheet" href="./css/style.css">
    <style>
        body {
            background: var(--bg-primary);
            color: var(--text-primary);
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
        }
        .detalle-container {
            max-width: 1000px;
            margin: 80px auto 40px;
            background: var(--bg-card);
            border-radius: 15px;
            padding: 30px;
            box-shadow: 0 0 25px rgba(0,0,0,0.3);
        }
        .detalle-top {
            display: flex;
            flex-wrap: wrap;
            gap: 25px;
        }
        .detalle-portada {
            width: 280px;
            height: 420px;
            object-fit: cover;
            border-radius: 10px;
        }
        .detalle-info {
            flex: 1;
        }
        .detalle-info h1 {
            font-size: 2.2rem;
            margin-bottom: 10px;
        }
        .detalle-info p {
            font-size: 1rem;
            line-height: 1.5;
        }
        .detalle-info h3 {
            font-size: 1.2rem;
            color: #1e90ff;
            margin-top: 20px;
        }
        .badge-genero {
            background: linear-gradient(135deg, #1e90ff, #8a2be2);
            display: inline-block;
            padding: 6px 14px;
            border-radius: 20px;
            font-size: 0.85rem;
            margin: 10px 0 20px;
        }
        .tomos-section {
            margin-top: 30px;
        }
        .tomos-section h2 {
            font-size: 1.5rem;
            margin-bottom: 20px;
        }
        .tomo-card {
            background: var(--bg-secondary);
            color: var(--accent-color);
            padding: 12px 18px;
            margin-bottom: 10px;
            border-radius: 8px;
        }
        .tomo-card a {
            color: var(--accent-color);
            text-decoration: none;
            margin-right: 10px;
        }
        .tomo-card a:hover {
            text-decoration: underline;
        }
        footer p {
        color: var(--text-secondary);
        }
    </style>
</head>
<body>
<header>
    <div class="header-container">
        <div class="logo">
            <img src="./imgs/Logito.png" alt="Logo Manga Verse" class="header-logo">
            <span>Manga Verse</span>
        </div>
        
        <nav>
            <ul>
                <li><a href="dashboard.php">Inicio</a></li>
                <li><a href="favoritos.php">Mi Lista</a></li>
                <li><a href="catalogo.php">Catálogo</a></li>
            </ul>
        </nav>

        <div class="theme-switcher">
            <button id="theme-toggle" aria-label="Cambiar tema">
                <span class="dark-icon">🌙</span>
                <span class="light-icon">☀️</span>
            </button>
        </div>
        <div class="profile">
            <?php if (isset($_SESSION['nombre'])): ?>
                <span>Hola, <?php echo $_SESSION['nombre']; ?></span>
            <?php else: ?>
                <a href="login.html">Iniciar Sesión</a>
            <?php endif; ?>
        </div>
    </div>
</header>

<div class="detalle-container">
    <div class="detalle-top">
        <img src="<?php echo htmlspecialchars($urlPortada); ?>" alt="Portada de <?php echo htmlspecialchars($titulo); ?>" class="detalle-portada">
        <div class="detalle-info">
            <h1><?php echo htmlspecialchars($titulo); ?></h1>
            <span class="badge-genero"><?php echo htmlspecialchars($genero); ?></span>
            <p><strong>Autor:</strong> <?php echo htmlspecialchars($autor); ?></p>
            <p><strong>Fecha de publicación:</strong> <?php echo htmlspecialchars($fechaPublicacion->format('d/m/Y')); ?></p>
            <h3>📖 Sinopsis</h3>
            <p><?php echo nl2br(htmlspecialchars($descripcion)); ?></p>

            <?php
            $esFavorito = false;
            if (isset($_SESSION['usuario_id'])) {
                $usuarioId = $_SESSION['usuario_id'];
                $sqlFav = "SELECT 1 FROM Favoritos WHERE UsuarioID = ? AND MangaID = ?";
                $stmtFav = sqlsrv_query($conn, $sqlFav, [$usuarioId, $mangaId]);
                if ($stmtFav && sqlsrv_fetch($stmtFav)) {
                    $esFavorito = true;
                }
            }
            ?>

            <?php if (isset($_SESSION['usuario_id'])): ?>
                <form action="<?= $esFavorito ? 'quitar_favorito.php' : 'agregar_favorito.php' ?>" method="POST" style="margin-top: 15px;">
                    <input type="hidden" name="manga_id" value="<?= $mangaId ?>">
                    <button type="submit" style="
                        padding: 10px 20px;
                        background: <?= $esFavorito ? '#dc3545' : 'linear-gradient(135deg, #ff8c00, #ff2e63)' ?>;
                        color: white;
                        font-weight: bold;
                        border: none;
                        border-radius: 8px;
                        cursor: pointer;
                        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
                    ">
                        <?= $esFavorito ? '❌ Quitar de Favoritos' : '⭐ Agregar a Favoritos' ?>
                    </button>
                </form>
            <?php else: ?>
                <p><a href="login.html" style="color: #1e90ff;">Inicia sesión para agregar a favoritos</a></p>
            <?php endif; ?>

        </div>
    </div>


    <div class="tomos-section">
        <h2>📚 Tomos disponibles:</h2>
        <?php if (!empty($archivos)): ?>
            <?php foreach ($archivos as $index => $archivo): ?>
                <?php
                    $archivoId = $archivo['id'];
                    $archivoNombre = htmlspecialchars($archivo['name']);
                ?>
                <div class="tomo-card">
                    <strong><?= $archivoNombre ?></strong><br>
                    <a href="<?= htmlspecialchars($archivo['webViewLink']) ?>" target="_blank">🌐 Ver en Google Drive</a> |
                    <a href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $index ?>&mode=cascade">📖 Leer aquí</a>
                </div>
            <?php endforeach; ?>
        <?php else: ?>
            <p style="color: #ccc;">No hay tomos disponibles en esta carpeta.</p>
        <?php endif; ?>
    </div>
</div>

<footer>
    <p style="text-align:center; color:#888; padding:40px 0;">&copy; 2025 Manga Verse. Todos los derechos reservados.</p>
</footer>

<script src="./js/theme-switcher.js"></script>
</body>
</html>
