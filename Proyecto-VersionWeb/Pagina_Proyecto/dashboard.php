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
            $badgePremium = "<span class='premium-badge'>🎖️ Usuario Premium</span>";
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
  :root{--bg-card:#2c2f4a;--bg-secondary:#3b3e5c;--text-primary:#f0f0f0;}
  .profile-dropdown{position:relative;display:flex;align-items:center;gap:8px;cursor:pointer}
  .profile-dropdown img{width:40px;height:40px;border-radius:50%;object-fit:cover}
  .profile-name{color:#f0f0f0;font-weight:bold;white-space:nowrap}
  .dropdown-content{display:none;position:absolute;right:0;top:100%;background:var(--bg-card);min-width:180px;border-radius:10px;box-shadow:0 4px 8px rgba(0,0,0,.3);z-index:999}
  .dropdown-content a{display:block;padding:12px;color:var(--text-primary);text-decoration:none}
  .dropdown-content a:hover{background:var(--bg-secondary)}
  .profile-dropdown:hover .dropdown-content{display:block}
  .aviso-premium {
    background: #ff7676;
    color: #000;
    padding: 12px 20px;
    border-radius: 10px;
    margin: 20px auto;
    text-align: center;
    font-weight: bold;
    max-width: 500px;
  }
  .premium-badge {
    display: inline-block;
    background: #00d4ff;
    color: #000;
    padding: 8px 16px;
    border-radius: 30px;
    font-weight: bold;
    font-size: 0.9em;
    margin-top: 15px;
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
        <li><a href="#">Mi Lista</a></li>
        <li><a href="./catalogo.php">Catálogo</a></li>
      </ul>
    </nav>

    <div class="theme-switcher">
      <button id="theme-toggle" aria-label="Cambiar tema">
        <span class="dark-icon">🌙</span><span class="light-icon">☀️</span>
      </button>
    </div>

    <!-- Perfil -->
    <div class="profile">
      <div class="profile-dropdown">
        <img src="<?= $fotoPerfil ?>" alt="Perfil">
        <span class="profile-name">Hola, <?= htmlspecialchars($_SESSION['nombre']) ?>!</span>
        <div class="dropdown-content">
          <a href="editar_perfil.php">Editar perfil</a>
          <a href="#" onclick="document.getElementById('logoutModal').style.display='block'">Cerrar sesión</a>
        </div>
      </div>
    </div>
  </div>
</header>

<main>
  <section class="hero">
    <div class="hero-content">
      <h1>Bienvenido a tu perfil, <?= htmlspecialchars($_SESSION['nombre']) ?>!</h1>
      <p>Aquí podrás gestionar tu lista de mangas, favoritos y más.</p>

      <?= $badgePremium ?>

      <?php if ($mostrarAviso): ?>
        <div class="aviso-premium">
          Tu suscripción premium ha vencido. <a href="suscripciones.php" style="text-decoration:underline;">Renovar ahora</a>.
        </div>
      <?php endif; ?>
    </div>
  </section>
</main>

<!-- modal cerrar sesión -->
<div id="logoutModal" class="modal" style="display:none;">
  <div class="modal-content" style="background:#2c2f4a;padding:20px;border-radius:10px;text-align:center">
    <h2>¿Estás seguro que deseas cerrar sesión?</h2>
    <button onclick="location.href='logout.php'" style="margin-top:15px;padding:10px 20px;background:red;color:white;border:none;border-radius:5px">Cerrar sesión</button>
    <button onclick="document.getElementById('logoutModal').style.display='none'" style="margin-top:15px;padding:10px 20px;background:#444;color:white;border:none;border-radius:5px">Cancelar</button>
  </div>
</div>

<footer><p>&copy; 2025 Manga Verse. Todos los derechos reservados.</p></footer>

<script src="./js/theme-switcher.js"></script>
</body>
</html>
