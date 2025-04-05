<?php
session_start();
if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

// Ruta de la foto actual del usuario (temporalmente usaremos el default)
$fotoPerfil = './imgs/default.png';
if (isset($_SESSION['foto_perfil']) && file_exists($_SESSION['foto_perfil'])) {
    $fotoPerfil = $_SESSION['foto_perfil'];
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Dashboard - Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <style>
    .profile-dropdown {
      position: relative;
      display: inline-block;
      cursor: pointer;
    }
    .profile-dropdown img {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      object-fit: cover;
      margin-right: 8px;
    }
    .profile-name {
      color: #f0f0f0;
      font-weight: bold;
    }
    .dropdown-content {
      display: none;
      position: absolute;
      right: 0;
      background-color: #2a2a2a;
      min-width: 200px;
      box-shadow: 0 4px 8px rgba(0,0,0,0.3);
      border-radius: 10px;
      z-index: 999;
    }
    .dropdown-content a,
    .dropdown-content label {
      color: white;
      padding: 12px;
      display: block;
      text-decoration: none;
      cursor: pointer;
    }
    .dropdown-content a:hover,
    .dropdown-content label:hover {
      background-color: #444;
    }
    .profile-dropdown:hover .dropdown-content {
      display: block;
    }
    input[type="file"] {
      display: none;
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
        <li><a href="#">Inicio</a></li>
        <li><a href="#">Mi Lista</a></li>
        <li><a href="#">Catálogo</a></li>
      </ul>
    </nav>
    <div class="profile">
      <div class="profile-dropdown">
        <img src="<?php echo $fotoPerfil; ?>" alt="Perfil">
        <span class="profile-name">Hola, <?php echo $_SESSION['nombre']; ?></span>
        <div class="dropdown-content">
          <form action="subir_foto.php" method="post" enctype="multipart/form-data" id="formFoto">
            <label for="fotoInput">Cambiar foto</label>
            <input type="file" name="foto" id="fotoInput" onchange="document.getElementById('formFoto').submit();">
          </form>
          <a href="#" onclick="document.getElementById('logoutModal').style.display='block'">Cerrar Sesión</a>
        </div>
      </div>
    </div>
  </div>
</header>

<main>
  <section class="hero">
    <div class="hero-content">
      <h1>Bienvenido a tu perfil, <?php echo $_SESSION['nombre']; ?>!</h1>
      <p>Aquí podrás gestionar tu lista de mangas, favoritos y más.</p>
    </div>
  </section>
</main>

<!-- Modal de Cerrar Sesión -->
<div id="logoutModal" class="modal" style="display:none;">
  <div class="modal-content" style="background:#2c2f4a; padding:20px; border-radius:10px; text-align:center;">
    <h2>¿Estás seguro que deseas cerrar sesión?</h2>
    <button onclick="location.href='logout.php'" style="margin-top:15px; padding:10px 20px; background:red; color:white; border:none; border-radius:5px;">Cerrar sesión</button>
    <button onclick="document.getElementById('logoutModal').style.display='none'" style="margin-top:15px; padding:10px 20px; background:#444; color:white; border:none; border-radius:5px;">Cancelar</button>
  </div>
</div>

<footer>
  <p>&copy; 2025 Manga Verse. Todos los derechos reservados.</p>
</footer>
</body>
</html>
