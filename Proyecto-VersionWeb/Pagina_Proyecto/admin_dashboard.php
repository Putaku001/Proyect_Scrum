<?php
session_start();
if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
  header("Location: login.html");
  exit();
}

require_once 'db.php';

// Consulta directa para cargar avatar actualizado
$uid = $_SESSION['usuario_id'];
$stmt = sqlsrv_query($conn, "SELECT Avatar FROM Usuarios WHERE UsuarioID = ?", [$uid]);
$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
$fotoPerfil = ($row && $row['Avatar'])
  ? 'data:image/png;base64,' . base64_encode($row['Avatar'])
  : './imgs/default.png';
?>
<!DOCTYPE html>
<html lang="es">

<head>
  <meta charset="UTF-8">
  <title>Dashboard Admin - Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
  <style>
    body {
      margin: 0;
      background: #121221;
      font-family: 'Segoe UI', sans-serif;
      color: #f0f0f0
    }

    header {
      background: #1e1e2f;
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 15px 30px;
      box-shadow: 0 2px 8px rgba(0, 0, 0, .5)
    }

    .logo {
      display: flex;
      align-items: center;
      gap: 10px
    }

    .logo img {
      height: 40px
    }

    .profile {
      position: relative
    }

    .profile-dropdown {
      display: flex;
      align-items: center;
      gap: 10px;
      cursor: pointer;
      user-select: none
    }

    .profile-dropdown img {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      object-fit: cover
    }

    .profile-name {
      font-weight: bold;
      color: #00d4ff
    }

    .dropdown-content {
      display: none;
      position: absolute;
      right: 0;
      top: 55px;
      background: #2c2f4a;
      border-radius: 10px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, .4);
      min-width: 180px;
      z-index: 999
    }

    .dropdown-content a {
      display: block;
      padding: 12px 16px;
      text-decoration: none;
      color: #f0f0f0;
      border-bottom: 1px solid #444
    }

    .dropdown-content a:hover {
      background: #3b3e5c
    }

    .admin-panel {
      text-align: center;
      padding: 50px 20px
    }

    .admin-panel h1 {
      color: #00d4ff;
      margin-bottom: 10px
    }

    .admin-panel p {
      color: #aaa;
      margin-bottom: 40px
    }

    .admin-options {
      display: flex;
      justify-content: center;
      flex-wrap: wrap;
      gap: 30px
    }

    .admin-card {
      background: #1e2238;
      padding: 25px;
      border-radius: 15px;
      width: 260px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, .4);
      transition: transform .3s
    }

    .admin-card:hover {
      transform: translateY(-8px)
    }

    .admin-card h3 {
      font-size: 1.2rem;
      margin-bottom: 15px;
      color: #f0f0f0
    }

    .admin-card a {
      display: inline-block;
      background: linear-gradient(135deg, #00d4ff, #1e90ff);
      color: #000;
      font-weight: bold;
      text-decoration: none;
      padding: 10px 20px;
      border-radius: 8px;
      transition: background .3s
    }

    .admin-card a:hover {
      background: linear-gradient(135deg, #00c0ff, #0077cc)
    }

    .admin-card i {
      font-size: 2.2rem;
      color: #00d4ff;
      margin-bottom: 10px
    }

    footer {
      margin-top: 80px;
      text-align: center;
      color: #777;
      font-size: .9rem;
      padding: 20px;
      background: #1e1e2f
    }
  </style>
</head>

<body>

  <header>
    <div class="logo">
      <img src="./imgs/Logito.png" alt="Logo Manga Verse">
      <span>Manga Verse — Admin</span>
    </div>

    <div class="profile">
      <div class="profile-dropdown" onclick="toggleDropdown()">
        <img src="<?= $fotoPerfil ?>" alt="Perfil">
        <span class="profile-name"><?= htmlspecialchars($_SESSION['nombre']) ?> ▼</span>
      </div>
      <div class="dropdown-content" id="dropdownMenu">
        <a href="editar_perfil.php">✏️ Editar perfil</a>
        <a href="logout.php">🚪 Cerrar sesión</a>
      </div>
    </div>
  </header>

  <main class="admin-panel">
    <h1>Panel de Control</h1>
    <p>Bienvenido, administrador. Elige una sección para gestionar el sistema:</p>

    <div class="admin-options">
      <div class="admin-card">
        <i class="fas fa-users"></i>
        <h3>Gestión de Usuarios</h3><a href="admin_panel.php">Administrar</a>
      </div>
      <div class="admin-card">
        <i class="fas fa-layer-group"></i>
        <h3>Ver Catálogo</h3><a href="catalogo_admin.php">Catálogo</a>
      </div>
      <div class="admin-card">
        <i class="fas fa-database"></i>
        <h3>Administración de Datos</h3><a href="admin_datos.php">Gestionar</a>
      </div>
      <div class="admin-card">
        <i class="fas fa-file-pdf"></i>
        <h3>Reporte de Suscriptores</h3><a href="reporte_suscriptores.php">Generar PDF</a>
      </div>
    </div>
  </main>

  <footer>
    <p>&copy; 2025 Manga Verse — Panel de administración</p>
  </footer>

  <script>
    function toggleDropdown() {
      const m = document.getElementById('dropdownMenu');
      m.style.display = (m.style.display === 'block') ? 'none' : 'block';
    }
    document.addEventListener('click', e => {
      const d = document.getElementById('dropdownMenu');
      const t = document.querySelector('.profile-dropdown');
      if (!t.contains(e.target)) d.style.display = 'none';
    });
  </script>
</body>

</html>