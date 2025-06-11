<?php
session_start();
require_once '../Config/db.php';

if (!isset($_SESSION['usuario_id'])) {
    header("Location: ../Public/login.html");
    exit();
}

$usuarioId = $_SESSION['usuario_id'];
$sql = "SELECT M.MangaID, M.Titulo, M.URLPortadaWeb
        FROM Favoritos F
        JOIN Mangas M ON F.MangaID = M.MangaID
        WHERE F.UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, [$usuarioId]);
?>
<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Mis Favoritos - Manga Verse</title>
  <link rel="stylesheet" href="../assets/css/style.css">
  <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap" rel="stylesheet">
  <style>
    :root {
      --bg-primary: #1c1c28;
      --bg-secondary: #2c2f4a;
      --bg-header: #23233a;
      --bg-tertiary: #151526;
      --bg-card: #232346;
      --input-border: #39396b;
      --accent-color: #8a2be2;
      --text-primary: #f0f0f0;
      --text-secondary: #a0a0b0;
    }
    [data-theme="light"] {
      --bg-primary: #eaeaf8;
      --bg-secondary: #f6f6fe;
      --bg-header: #f4f6fc;
      --bg-tertiary: #d4e6f9;
      --bg-card: #fff;
      --text-primary: #1d1d2c;
      --text-secondary: #5a5a78;
      --input-border: #bbc4e4;
      --accent-color: #8a2be2;
    }
    * {
      margin: 0; padding: 0; box-sizing: border-box;
    }
    html, body {
      min-height: 100%;
      height: 100%;
    }
    body {
      background: linear-gradient(135deg, var(--bg-primary), var(--bg-secondary));
      font-family: 'Inter', 'Roboto', sans-serif;
      color: var(--text-primary);
      min-height: 100vh;
      display: flex;
      flex-direction: column;
    }
    header {
      background: var(--bg-header);
      padding: 18px 5vw;
      display: flex;
      align-items: center;
      justify-content: space-between;
      box-shadow: 0 0 15px rgba(0, 0, 0, 0.35);
      flex-wrap: wrap;
      position: sticky; top: 0; width: 100%; z-index: 20;
    }
    .logo {
      display: flex;
      align-items: center;
      gap: 10px;
      min-width: 0;
    }
    .logo img {
      height: 38px; border-radius: 9px;
      background: #18182a;
    }
    .logo span {
      font-size: 1.25rem;
      color: var(--accent-color);
      font-weight: bold;
      letter-spacing: .5px;
      white-space: nowrap;
    }
    nav ul {
      list-style: none;
      display: flex;
      gap: 26px;
      padding: 0; margin: 0;
    }
    nav a {
      color: var(--text-secondary);
      font-weight: 600;
      text-decoration: none;
      font-size: 1rem;
      padding: 8px 16px;
      border-radius: 8px;
      transition: color 0.2s, background 0.2s;
    }
    nav a:hover {
      color: var(--accent-color);
      background: rgba(138,43,226,0.07);
    }
    .theme-switcher button {
      background: none;
      border: none;
      font-size: 1.2rem;
      color: var(--text-primary);
      cursor: pointer;
      margin-left: 12px;
    }
    /* Nombre full ancho, nunca recortado */
    .profile-name {
      color: var(--accent-color);
      font-weight: 700;
      font-size: 1.04rem;
      margin-left: 16px;
      max-width: 220px;
      min-width: 0;
      text-overflow: ellipsis;
      white-space: normal;
      overflow-wrap: anywhere;
      word-break: break-all;
      display: block;
    }
    @media (max-width: 1100px) {
      .profile-name { max-width: 120px; }
      header { flex-wrap: wrap; gap: 6px;}
    }
    @media (max-width: 700px) {
      header {
        flex-direction: column;
        align-items: stretch;
        padding: 13px 2vw 7px;
        gap: 8px;
      }
      .logo img { height: 30px; }
      .logo span { font-size: 1rem; }
      nav ul { gap: 14px;}
      .profile-name { font-size: 0.96rem; max-width: 98vw; margin: 7px 0 0 0;}
    }
    .contenedor {
      flex: 1 1 auto;
      max-width: 1400px;
      margin: 0 auto;
      padding: 65px 16px 28px;
      min-height: 0;
      width: 100%;
      display: flex;
      flex-direction: column;
    }
    .contenedor h2 {
      font-size: 2rem;
      margin-bottom: 26px;
      text-align: center;
      color: var(--accent-color);
      text-shadow: 0 0 10px rgba(138, 43, 226, 0.26);
      font-weight: 700;
    }
    .galeria {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(215px, 1fr));
      gap: 22px;
      width: 100%;
    }
    .manga {
      position: relative;
      background: var(--bg-card);
      border-radius: 15px;
      overflow: hidden;
      transition: transform 0.26s, box-shadow 0.26s;
      box-shadow: 0 6px 18px rgba(0,0,0,0.18);
      display: flex;
      flex-direction: column;
      height: 100%;
    }
    .manga:hover {
      transform: translateY(-6px) scale(1.04);
      box-shadow: 0 14px 26px rgba(138,43,226,0.10);
    }
    .manga img {
      width: 100%;
      height: 230px;
      object-fit: cover;
      display: block;
      background: #151526;
      border-bottom: 1px solid var(--input-border);
    }
    .manga h4 {
      padding: 15px 7px 15px 7px;
      text-align: center;
      font-size: 1.07rem;
      color: var(--text-secondary);
      font-weight: 600;
      word-break: break-word;
      flex: 1 1 auto;
    }
    .quitar-form {
      position: absolute;
      top: 12px;
      right: 12px;
      z-index: 3;
    }
    .quitar-btn {
      background: rgba(239, 68, 68, 0.97);
      border: none;
      border-radius: 50%;
      width: 32px;
      height: 32px;
      font-size: 1.2rem;
      font-weight: bold;
      color: white;
      cursor: pointer;
      box-shadow: 0 0 8px rgba(255, 0, 0, 0.32);
      transition: background 0.2s;
      line-height: 1;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .quitar-btn:hover {
      background: #dc2626;
    }
    .sin-favoritos {
      text-align: center;
      font-size: 1.13rem;
      color: #aaa;
      margin: 80px 0 0 0;
      width: 100%;
      font-weight: 600;
    }
    footer {
      background: var(--bg-tertiary, #151526);
      color: var(--text-secondary, #b7b7de);
      text-align: center;
      padding: 30px 10px 20px 10px;
      font-size: 0.98rem;
      border-top: 1px solid var(--input-border, #39396b);
      margin-top: auto;
      width: 100%;
      box-shadow: 0 -3px 10px rgba(138,43,226,0.05);
    }
    @media (max-width: 900px) {
      .contenedor { padding: 30px 6px 18px;}
      .galeria { gap: 10px; grid-template-columns: repeat(auto-fill, minmax(150px, 1fr)); }
      .manga img { height: 120px;}
      .contenedor h2 { font-size: 1.15rem;}
      .profile-name { max-width: 95vw;}
    }
    @media (max-width: 600px) {
      .galeria { grid-template-columns: 1fr; }
      .contenedor { padding: 24px 2px 10px; }
      .logo span { font-size: 0.98rem;}
      .manga img { height: 95px;}
    }
  </style>
</head>
<body>

<header>
  <div class="logo">
    <img src="../assets/imgs/Logito.png" alt="Logo">
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

  <div class="profile-name">Hola, <?php echo $_SESSION['nombre']; ?></div>
</header>

<main class="contenedor">
  <h2>⭐ Tus Favoritos</h2>

  <?php if ($stmt && sqlsrv_has_rows($stmt)): ?>
    <div class="galeria">
      <?php while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)):
        $titulo = htmlspecialchars($row['Titulo']);
        $portada = $row['URLPortadaWeb'] ?: '../assets/imgs/no_portada.png';
        $id = $row['MangaID'];
      ?>
     <div class="manga" data-id="<?= $id ?>">
  <form class="quitar-form" onsubmit="return false;">
    <input type="hidden" name="manga_id" value="<?= $id ?>">
    <button class="quitar-btn" data-manga-id="<?= $id ?>" title="Quitar de favoritos">&times;</button>
  </form>
  <a href="detalle_manga.php?id=<?= $id ?>" class="manga-link">
    <img src="<?= $portada ?>" alt="<?= $titulo ?>">
    <h4><?= $titulo ?></h4>
  </a>
</div>

      <?php endwhile; ?>
    </div>
  <?php else: ?>
    <p class="sin-favoritos">😢 No tienes mangas en tu lista de favoritos todavía.</p>
  <?php endif; ?>
</main>

<footer>
  <p>&copy; 2025 Manga Verse. Todos los derechos reservados.</p>
</footer>

<script>
document.querySelectorAll('.quitar-btn').forEach(btn => {
  btn.addEventListener('click', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const mangaId = this.dataset.mangaId;
    const card = this.closest('.manga');

    fetch('./Client/quitar_favorito_ajax.php', {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body: 'manga_id=' + mangaId
    })
    .then(res => res.text())
    .then(data => {
      if (data.trim() === 'ok') {
        card.style.transition = "opacity 0.3s ease";
        card.style.opacity = 0;
        setTimeout(() => card.remove(), 300);
      } else {
        alert("Error al quitar el manga de favoritos.");
      }
    });
  });
});
</script>


<script src="../assets/js/theme-switcher.js"></script>
</body>
</html>
