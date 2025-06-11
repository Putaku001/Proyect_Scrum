<?php
/*─────────────────────────────────────────────────────────────*
 * detalle_manga.php – Ficha de un manga + lista de tomos PDF *
 * Bloquea todo acceso a tomos para usuarios NO Premium       *
 *─────────────────────────────────────────────────────────────*/

if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

require_once '../Config/db.php';
require_once '../drive_auth.php';   // ← $access_token listo

/*─────  Manga solicitado  ────────────────────────────────────*/
if (!isset($_GET['id'])) die('Manga no especificado.');
$mangaId = (int)$_GET['id'];

/* Info del manga */
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

/*─────  Portada  ─────────────────────────────────────────────*/
/*─────  Portada  ────────────────────────────────────────────*/
$defaultCover = '../assets/imgs/no_portada.png';
$urlPortada   = $defaultCover;

/* 1) Intenta primero la copia local -------------------------*/
if (!empty($urlPortadaWeb)) {
    //   Si viene absoluta (/Pagina_Proyecto/…), usa DOCUMENT_ROOT.
    //   Si viene relativa (assets/imgs/…), complétala con __DIR__.
    $absPath = str_starts_with($urlPortadaWeb, '/')
        ? $_SERVER['DOCUMENT_ROOT'] . $urlPortadaWeb
        : __DIR__ . '/../' . ltrim($urlPortadaWeb, './');

    if (is_file($absPath)) {
        $urlPortada = $urlPortadaWeb;                       // ¡encontrada!
    }
}

/* 2) Si no existe localmente, usa la de Google Drive --------*/
if ($urlPortada === $defaultCover && !empty($urlPortadaDrive)) {
    $urlPortada = driveThumbnail($urlPortadaDrive);         // enlace directo
}


/*─────  ¿El visitante es Premium?  ──────────────────────────*/
$usuarioPremium = false;
if (isset($_SESSION['usuario_id'])) {
    $stmtP = sqlsrv_query(
        $conn,
        "SELECT EsPremium FROM Usuarios WHERE UsuarioID = ?",
        [$_SESSION['usuario_id']]
    );
    if ($stmtP && sqlsrv_fetch($stmtP)) {
        $usuarioPremium = (bool) sqlsrv_get_field($stmtP, 0);
    }
}

/*─────  ID de la carpeta Drive  ─────────────────────────────*/
function folderId($url)
{
    if (preg_match('/\/folders\/([a-zA-Z0-9_-]+)/', $url, $m)) return $m[1];
    if (preg_match('/[?&]id=([a-zA-Z0-9_-]+)/', $url, $m))    return $m[1];
    return null;
}
$folder_id = folderId($urlDrive) ?: die('Carpeta Drive inválida.');

/*─────  Listar PDFs  ─────────────────────────────────────────*/
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

/* Orden numérico */
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
    <title><?php echo htmlspecialchars($titulo); ?> - Manga Verse</title>
    <link rel="stylesheet" href="../assets/css/style.css">
    <style>
        /* ——— Estilos actualizados para detalle_manga.php ——— */
        body {
            background: var(--bg-primary);
            color: var(--text-primary);
            font-family: 'Roboto', sans-serif;
            margin: 0;
        }
        .detalle-container {
            max-width: 1000px;
            margin: 80px auto 40px;
            background: var(--bg-card);
            border-radius: 15px;
            padding: 30px;
            box-shadow: 0 8px 32px rgba(137, 129, 248, 0.3);
            border: 1px solid var(--input-border);
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
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.4);
        }
        .detalle-info {
            flex: 1;
            min-width: 300px;
        }
        .detalle-info h1 {
            font-size: 2.2rem;
            margin-bottom: 10px;
            color: var(--accent-color);
        }
        .detalle-info p {
            font-size: 1rem;
            line-height: 1.6;
            color: var(--text-primary);
        }
        .detalle-info h3 {
            font-size: 1.2rem;
            color: var(--accent-color);
            margin-top: 20px;
        }
        .detalle-info h4 {
            color: var(--text-primary); /* Usará blanco en modo oscuro */
            font-size: 1.1rem;
            margin: 15px 0 10px;
        }
        .badge-genero {
            background: var(--button-primary);
            color: white;
            display: inline-block;
            padding: 6px 14px;
            border-radius: 20px;
            font-size: 0.85rem;
            margin: 10px 0 20px;
            font-weight: bold;
        }
        .tomos-section {
            margin-top: 40px;
        }
        .tomos-section h2 {
            font-size: 1.8rem;
            margin-bottom: 20px;
            color: var(--accent-color);
        }
        .tomo-card {
            background: var(--bg-secondary);
            padding: 15px 20px;
            margin-bottom: 15px;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
            border: 1px solid var(--input-border);
        }
        .tomo-card strong {
            color: var(--text-primary);
            display: block;
            margin-bottom: 8px;
        }
        .tomo-card a {
            color: var(--accent-color);
            text-decoration: none;
            margin-right: 15px;
            transition: all 0.3s ease;
            padding: 5px 10px;
            border-radius: 5px;
        }
        .tomo-card a:hover {
            text-decoration: none;
            background: rgba(138, 43, 226, 0.1);
        }
        
        /* ——— Estilos mejorados para el botón de favoritos ——— */
        .favoritos-form {
            margin-top: 25px;
        }
        .favoritos-btn {
            padding: 12px 24px;
            background: var(--button-primary);
            color: var(--button-text-color);
            font-weight: bold;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
            display: inline-flex;
            align-items: center;
            gap: 8px;
            font-size: 1rem;
        }
        .favoritos-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 16px rgba(0, 0, 0, 0.3);
        }
        .favoritos-btn.quitar {
            background: linear-gradient(135deg, #ff4757, #dc3545);
        }
        
        /* ——— Estilo para el enlace de login ——— */
        .login-link {
            color: var(--accent-color);
            text-decoration: none;
            transition: all 0.3s ease;
        }
        .login-link:hover {
            text-decoration: underline;
        }
        footer {
      flex-shrink: 0;
      text-align: center;
      padding: 32px 10px 22px 10px;
      background: var(--bg-tertiary, #151526);
      color: var(--text-secondary, #b7b7de);
      font-size: 0.96rem;
      box-shadow: 0 -4px 10px rgba(137, 129, 248, 0.08);
      border-top: 1px solid var(--input-border, #39396b);
      width: 100%;
      margin-top: auto;
    }
    </style>
</head>
<body>
<header>
    <div class="header-container">
        <div class="logo">
            <img src="../assets/imgs/Logito.png" alt="Logo Manga Verse" class="header-logo">
            <span>Manga Verse</span>
        </div>
        
        <nav>
            <ul>
                <li><a href="./Client/dashboard.php">Inicio</a></li>
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
                <a href="../Public/login.html">Iniciar Sesión</a>
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
/*── Favoritos ───────────────────────────────────────────────*/
$esFavorito = false;
if (isset($_SESSION['usuario_id'])) {
    $usuarioId = $_SESSION['usuario_id'];
    $sqlFav = "SELECT 1 FROM Favoritos WHERE UsuarioID = ? AND MangaID = ?";
    $stmtFav = sqlsrv_query($conn, $sqlFav, [$usuarioId, $mangaId]);
    if ($stmtFav && sqlsrv_fetch($stmtFav)) $esFavorito = true;
}
?>
            <?php if (isset($_SESSION['usuario_id'])): ?>
                <form action="<?= $esFavorito ? './Client/quitar_favorito.php' : './Client/agregar_favorito.php' ?>" method="POST" style="margin-top: 15px;">
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
                <p><a href="../Public/login.html" style="color: #1e90ff;">Inicia sesión para agregar a favoritos</a></p>
            <?php endif; ?>

        </div><!-- /.detalle-info -->
    </div><!-- /.detalle-top -->

  <div class="tomos-section">
    <h2>📚 Tomos disponibles:</h2>

<?php if (!empty($archivos)): ?>
<?php foreach ($archivos as $idx => $file): 
      $nombre = htmlspecialchars($file['name']);
      $idPDF  = $file['id'];
      $linkGV = htmlspecialchars($file['webViewLink']);
      $esPremiumTomo = str_starts_with($file['name'], '[P]');
?>
    <div class="tomo-card">
        <strong><?= $nombre ?></strong><br>

<?php if ($esPremiumTomo && !$usuarioPremium): ?>
        <!-- Solo para Premium si empieza con [P] -->
        <a href="#" onclick="alert('Este tomo es exclusivo para usuarios Premium.'); return false;">
            🚫 Tomo Premium
        </a>
<?php else: ?>
        <!-- Tomo libre o usuario Premium -->
        <a href="<?= $linkGV ?>" target="_blank">🌐 Ver en Google Drive</a> |
        <a href="visor.php?manga_id=<?= $mangaId ?>&index=<?= $idx ?>&id=<?= $idPDF ?>">📖 Leer aquí</a>
<?php endif; ?>
    </div>
<?php endforeach; ?>
<?php else: ?>
    <p style="color:#ccc;">No hay tomos disponibles en esta carpeta.</p>
<?php endif; ?>

</div><!-- /.tomos-section -->

</div><!-- /.detalle-container -->

<footer>
  <p>&copy; 2025 Manga Verse</p>
</footer>

<script src="../assets/js/theme-switcher.js"></script>
</body>
</html>
