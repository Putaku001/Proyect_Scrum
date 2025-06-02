<?php
session_start();
require_once 'db.php';

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
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
  <link rel="stylesheet" href="./css/style.css">
  <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap" rel="stylesheet">
  <style>
    * {
      margin: 0;
      padding: 0;
      box-sizing: border-box;
    }

    body {
      background: linear-gradient(135deg, var(--bg-primary), var(--bg-secondary));
      font-family: 'Inter', sans-serif;
      color: var(--text-primary);
    }

    header {
      background: var(--bg-header);
      padding: 20px 40px;
      display: flex;
      justify-content: space-between;
      align-items: center;
      box-shadow: 0 0 15px rgba(0, 0, 0, 0.6);
    }

    .logo {
      display: flex;
      align-items: center;
      gap: 12px;
    }

    .logo img {
      height: 40px;
    }

    .logo span {
      font-size: 1.6rem;
      color: var(--accent-color);
      font-weight: bold;
    }

    nav ul {
      list-style: none;
      display: flex;
      gap: 30px;
    }

    nav a {
      color: var(--text-secondary);
      font-weight: 600;
      text-decoration: none;
      transition: color 0.3s ease;
    }

    nav a:hover {
      color: var(--accent-color);
    }

    .profile-name {
      color: var(--accent-color);
      font-weight: 600;
    }

    .contenedor {
      max-width: 1400px;
      margin: 0 auto;
      padding: 100px 30px 60px;
      min-height: calc(100vh - 140px); /* evita que el footer se monte encima si hay poco contenido */
    }


    .contenedor h2 {
      font-size: 2.2rem;
      margin-bottom: 40px;
      text-align: center;
      color: var(--accent-color);
      text-shadow: 0 0 10px rgba(138, 43, 226, 0.5);
    }

    .galeria {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
      gap: 35px;
    }

    .manga {
      position: relative;
      background: var(--bg-card);
      border-radius: 15px;
      overflow: hidden;
      transition: transform 0.3s ease, box-shadow 0.3s ease;
      box-shadow: 0 8px 20px rgba(0,0,0,0.4);
    }

    .manga:hover {
      transform: translateY(-10px) scale(1.03);
      box-shadow: 0 12px 25px rgba(0,0,0,0.6);
    }

    .manga img {
      width: 100%;
      height: 300px;
      object-fit: cover;
      display: block;
    }

    .manga h4 {
      padding: 15px;
      text-align: center;
      font-size: 1rem;
      color: var(--text-secondary);
    }

    .quitar-form {
      position: absolute;
      top: 12px;
      right: 12px;
    }

    .quitar-btn {
      background: rgba(239, 68, 68, 0.95);
      border: none;
      border-radius: 50%;
      width: 32px;
      height: 32px;
      font-size: 18px;
      font-weight: bold;
      color: white;
      cursor: pointer;
      box-shadow: 0 0 8px rgba(255, 0, 0, 0.5);
      transition: background 0.3s ease;
    }

    .quitar-btn:hover {
      background: #dc2626;
    }

    .sin-favoritos {
      text-align: center;
      font-size: 1.2rem;
      color: #aaa;
      margin-top: 80px;
    }

    footer {
      background: var(--bg-tertiary);
      color: var(--text-secondary);
      text-align: center;
      padding: 30px;
      margin-top: 60px;
      font-size: 0.95rem;
      border-top: 1px solid var(--input-border);
    }
  </style>
</head>
<body>

<header>
  <div class="logo">
    <img src="./imgs/Logito.png" alt="Logo">
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

  <div class="profile-name">Hola, <?php echo $_SESSION['nombre']; ?></div>
</header>

<main class="contenedor">
  <h2>⭐ Tus Favoritos</h2>

  <?php if ($stmt && sqlsrv_has_rows($stmt)): ?>
    <div class="galeria">
      <?php while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)):
        $titulo = htmlspecialchars($row['Titulo']);
        $portada = $row['URLPortadaWeb'] ?: './imgs/no_portada.png';
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

    fetch('quitar_favorito_ajax.php', {
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


<script src="./js/theme-switcher.js"></script>
</body>
</html>
