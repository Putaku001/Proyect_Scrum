<?php 
session_start();

if (!isset($_SESSION['usuario_id'])) {
  header("Location: login.html");
  exit();
}

require_once 'db.php';

$uid = $_SESSION['usuario_id'];
$stmt = sqlsrv_query($conn, "SELECT Avatar FROM Usuarios WHERE UsuarioID = ?", [$uid]);
$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

$fotoPerfil = ($row && $row['Avatar'])
  ? 'data:image/png;base64,' . base64_encode($row['Avatar'])
  : './imgs/default.png';

// Verificar si la suscripción ha vencido o sigue activa
$sqlPremium = "
    SELECT EsPremium,
           (SELECT TOP 1 FechaFin FROM Suscripciones WHERE UsuarioID = ? ORDER BY FechaFin DESC) AS FechaFin
    FROM Usuarios WHERE UsuarioID = ?
";
$stmt2 = sqlsrv_query($conn, $sqlPremium, [$uid, $uid]);
$userData = sqlsrv_fetch_array($stmt2, SQLSRV_FETCH_ASSOC);

$mostrarAviso = false;
$badgePremium = '';
if ($userData && $userData['EsPremium']) {
  $fechaFinObj = $userData['FechaFin'];
  if ($fechaFinObj instanceof DateTime) {
    $vence = strtotime($fechaFinObj->format('Y-m-d'));
    $hoy = strtotime(date('Y-m-d'));
    if ($vence < $hoy) {
      $mostrarAviso = true;
    } else {
      $badgePremium = "<span class='premium-badge'>🏆 Usuario Premium</span>";
    }
  }
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Dashboard - Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <style>
    html, body {
      height: 100%;
      margin: 0;
      padding: 0;
      box-sizing: border-box;
    }
    body {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      background: var(--bg-primary, #1c1c28);
      color: var(--text-primary, #f0f0f0);
      font-family: 'Roboto', Arial, sans-serif;
    }
    header {
      background: var(--bg-header, #23233a);
      position: sticky;
      top: 0;
      z-index: 100;
      width: 100%;
      box-shadow: 0 2px 6px rgba(30,30,50,0.16);
    }
    .header-container {
      max-width: 1200px;
      margin: 0 auto;
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 18px;
      padding: 16px 22px;
      flex-wrap: wrap;
    }
    .logo {
      display: flex;
      align-items: center;
      gap: 10px;
      font-size: 1.2rem;
      font-weight: 700;
      color: var(--accent-color, #8a2be2);
    }
    .header-logo {
      width: 40px; height: 40px;
      border-radius: 10px;
      object-fit: cover;
    }
    nav ul {
      list-style: none;
      display: flex;
      gap: 24px;
      padding: 0;
      margin: 0;
    }
    nav a {
      color: var(--text-primary, #fff);
      text-decoration: none;
      font-weight: 500;
      padding: 7px 15px;
      border-radius: 18px;
      transition: background 0.2s;
      font-size: 1rem;
    }
    nav a:hover {
      background: var(--accent-color, #8a2be2);
      color: #fff;
    }
    .theme-switcher button {
      background: none;
      border: none;
      cursor: pointer;
      font-size: 1.32rem;
      color: var(--text-primary, #fff);
      transition: transform 0.17s;
    }
    .theme-switcher button:active {
      transform: scale(1.18);
    }
    .profile {
      display: flex;
      align-items: center;
      min-width: 0;
    }
    .profile-dropdown {
      position: relative;
      display: flex;
      align-items: center;
      gap: 8px;
      min-width: 0;
      max-width: 300px;
      cursor: pointer;
      user-select: none;
    }
    .profile-dropdown img {
      width: 40px; height: 40px;
      border-radius: 50%;
      object-fit: cover;
      border: 2px solid var(--accent-color, #8a2be2);
      background: #18182a;
      flex-shrink: 0;
    }
    .profile-name {
      color: var(--text-primary, #fff);
      font-weight: bold;
      font-size: 1rem;
      word-break: break-word;
      white-space: normal;
      max-width: 180px;
      line-height: 1.2;
      display: inline-block;
    }
    .dropdown-content {
      display: none;
      position: absolute;
      right: 0;
      top: 110%;
      background: var(--bg-card, #2c2f4a);
      min-width: 175px;
      border-radius: 10px;
      box-shadow: 0 4px 12px rgba(0,0,0,0.26);
      z-index: 999;
      border: 1px solid var(--input-border, #39396b);
      animation: fadeIn .22s cubic-bezier(.3,1.2,.5,1.1);
      overflow: hidden;
    }
    .dropdown-content.show {
      display: block;
    }
    .dropdown-content a {
      display: block;
      padding: 12px;
      color: var(--text-primary, #fff);
      text-decoration: none;
      transition: background 0.2s, color 0.2s;
      border-radius: 0;
      font-size: 1rem;
    }
    .dropdown-content a:hover {
      background: var(--bg-secondary, #3b3e5c);
      color: var(--accent-color, #8a2be2);
    }
    @keyframes fadeIn {
      from { opacity: 0; transform: translateY(-10px);}
      to { opacity: 1; transform: translateY(0);}
    }

    /* HERO BANNER 100vh CENTRADO */
    .hero-profile-bg {
      min-height: calc(100vh - 72px); /* Ajusta 72px según altura de tu header */
      width: 100vw;
      margin-left: 50%;
      transform: translateX(-50%);
      background: linear-gradient(100deg, #18182a 0%, #8a2be2 120%);
      display: flex;
      align-items: center;
      justify-content: center;
      flex-direction: column;
      text-align: center;
      padding: 0;
      box-shadow: 0 8px 32px rgba(30,20,80,0.17);
      position: relative;
      z-index: 1;
    }
    .hero-profile-content {
      width: 100%;
      max-width: 800px;
      margin: 0 auto;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
    }
    .hero-profile-content h1 {
      font-size: 2.8rem;
      color: #fff;
      margin-bottom: 18px;
      font-weight: 800;
      line-height: 1.12;
      word-break: break-word;
      text-shadow: 0 2px 12px rgba(15,15,40,0.25);
    }
    .hero-profile-content p {
      font-size: 1.3rem;
      color: #f1f1fa;
      margin-bottom: 18px;
      line-height: 1.5;
    }
    .premium-badge {
      display: inline-block;
      background: #00d4ff;
      color: #222;
      padding: 14px 32px;
      border-radius: 32px;
      font-weight: bold;
      font-size: 1.20em;
      margin: 22px auto 0 auto;
      box-shadow: 0 2px 7px rgba(0,0,0,0.19);
      letter-spacing: .5px;
      vertical-align: middle;
      text-align: center;
    }
    .aviso-premium {
      background: #ff7676;
      color: #fff;
      padding: 14px 22px;
      border-radius: 10px;
      margin: 22px auto 0 auto;
      text-align: center;
      font-weight: bold;
      max-width: 440px;
      box-shadow: 0 4px 10px rgba(0,0,0,0.18);
      font-size: 1.1rem;
    }

    /* MODAL */
    .modal {
      position: fixed;
      left: 0; top: 0;
      width: 100vw; height: 100vh;
      background: rgba(34, 34, 53, 0.77);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 9999;
      transition: background 0.2s;
    }
    .modal-content {
      background: #2c2f4a;
      padding: 24px 28px;
      border-radius: 12px;
      text-align: center;
      color: #fff;
      min-width: 260px;
      box-shadow: 0 4px 14px rgba(0,0,0,0.25);
      animation: fadeIn .2s;
    }
    .modal-content button {
      margin: 13px 7px 0 7px;
      padding: 10px 23px;
      border: none;
      border-radius: 7px;
      font-size: 1rem;
      font-weight: bold;
      cursor: pointer;
      transition: background .17s, color .17s;
    }
    .modal-content button:first-of-type {
      background: red;
      color: #fff;
    }
    .modal-content button:last-of-type {
      background: #444;
      color: #fff;
    }
    .modal-content button:hover {
      filter: brightness(1.12);
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
    /* RESPONSIVE */
    @media (max-width: 900px) {
      .header-container { padding: 13px 8px; gap: 10px; }
      .hero-profile-bg { padding: 0;}
      .hero-profile-content h1 { font-size: 2rem;}
      .hero-profile-content p { font-size: 1.05rem;}
      .profile-name { max-width: 110px; font-size: 0.95rem;}
    }
    @media (max-width: 480px) {
      .header-logo { width: 27px; height: 27px; }
      .logo { font-size: 0.91rem;}
      .hero-profile-bg { padding: 0;}
      .hero-profile-content h1 { font-size: 1.08rem;}
      .premium-badge, .aviso-premium { font-size: 0.89rem; padding: 7px 10px;}
      .profile-dropdown img { width: 27px; height: 27px;}
      .profile-name { max-width: 65vw; font-size: 0.90rem;}
    }
    [data-theme="light"] {
      --bg-card: #fff;
      --bg-secondary: #f0f0f0;
      --bg-tertiary: #d4e6f9;
      --text-primary: #404040;
      --danger-color: #ff5252;
      --premium-color: #00c6ff;
    }
    [data-theme="light"] .premium-badge { color: #000; }
    [data-theme="light"] .dropdown-content {
      box-shadow: 0 4px 8px rgba(0,0,0,0.08);
    }
  </style>
</head>
<body>
  <header>
    <div class="header-container">
      <div class="logo">
        <img src="./imgs/Logito.png" alt="Logo" class="header-logo">
        <span>Manga Verse</span>
      </div>
      <nav>
        <ul>
          <li><a href="#">Inicio</a></li>
          <li><a href="./favoritos.php">Mi Lista</a></li>
          <li><a href="./catalogo.php">Catálogo</a></li>
        </ul>
      </nav>
      <div class="theme-switcher">
        <button id="theme-toggle" aria-label="Cambiar tema">
          <span class="dark-icon">🌙</span><span class="light-icon">☀️</span>
        </button>
      </div>
      <div class="profile">
        <div class="profile-dropdown" id="profileDropdown">
          <img src="<?= $fotoPerfil ?>" alt="Perfil">
          <span class="profile-name">Hola, <?= htmlspecialchars($_SESSION['nombre']) ?>!</span>
          <div class="dropdown-content" id="dropdownContent">
            <a href="editar_perfil.php">Editar perfil</a>
            <a href="#" onclick="document.getElementById('logoutModal').style.display='flex'">Cerrar sesión</a>
          </div>
        </div>
      </div>
    </div>
  </header>
  <!-- Hero Banner perfil ocupa toda la pantalla visible -->
  <div class="hero-profile-bg">
    <div class="hero-profile-content">
      <h1>Bienvenido a tu perfil, <?= htmlspecialchars($_SESSION['nombre']) ?>!</h1>
      <p>Aquí podrás gestionar tu lista de mangas, favoritos y más.</p>
      <?= $badgePremium ?>
      <?php if ($mostrarAviso): ?>
        <div class="aviso-premium">
          Tu suscripción premium ha vencido. <a href="suscripciones.php" style="text-decoration:underline;color:#fff">Renovar ahora</a>.
        </div>
      <?php endif; ?>
    </div>
  </div>
  <!-- modal cerrar sesión -->
  <div id="logoutModal" class="modal" style="display:none;">
    <div class="modal-content">
      <h2>¿Estás seguro que deseas cerrar sesión?</h2>
      <button onclick="location.href='logout.php'">Cerrar sesión</button>
      <button onclick="document.getElementById('logoutModal').style.display='none'">Cancelar</button>
    </div>
  </div>
  <footer>
    <p>&copy; 2025 Manga Verse. Todos los derechos reservados.</p>
  </footer>
  <script src="./js/theme-switcher.js"></script>
  <script>
    // --- Dropdown por CLICK ---
    document.addEventListener('DOMContentLoaded', function () {
      var profileDropdown = document.getElementById('profileDropdown');
      var dropdownContent = document.getElementById('dropdownContent');
      let isOpen = false;
      profileDropdown.addEventListener('click', function (e) {
        e.stopPropagation();
        isOpen = !isOpen;
        dropdownContent.classList.toggle('show', isOpen);
      });
      document.addEventListener('click', function (e) {
        if (isOpen && !profileDropdown.contains(e.target)) {
          dropdownContent.classList.remove('show');
          isOpen = false;
        }
      });
    });
  </script>
</body>
</html>