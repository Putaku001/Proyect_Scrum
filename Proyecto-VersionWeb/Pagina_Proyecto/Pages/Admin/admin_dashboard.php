<?php
session_start();
if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
  header("Location: login.html");
  exit();
}

require_once '../../Config/db.php';

$uid = $_SESSION['usuario_id'];
$stmt = sqlsrv_query($conn, "SELECT Avatar FROM Usuarios WHERE UsuarioID = ?", [$uid]);
$row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
$fotoPerfil = ($row && $row['Avatar'])
  ? 'data:image/png;base64,' . base64_encode($row['Avatar'])
  : '../../assets/imgs/default.png';
?>
<!DOCTYPE html>
<html lang="es" data-theme="dark">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Dashboard Admin - Manga Verse</title>
  <link rel="stylesheet" href="../../assets/css/style.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
  <style>
 /* === VARIABLES PARA TEMA === */
:root {
  --bg-primary: #1c1c28;
  --bg-secondary: #2c2f4a;
  --bg-tertiary: #151526;
  --bg-header: rgba(30, 30, 50, 0.95);
  --bg-card: #2a2a3a;
  --text-primary: #f0f0f0;
  --text-secondary: #a0a0b0;
  --accent-color: #8a2be2;
  --button-primary: linear-gradient(135deg, #8a2be2 40%, #4e9bff 100%);
  --input-border: #39396b;
  --danger-color: #ff5252;
  --premium-color: #00c6ff;
}

/* ===== TEMA CLARO ===== */
[data-theme="light"] {
  --bg-primary: #f8f9ff;
  --bg-secondary: #f0f0f0;
  --bg-tertiary: #d4e6f9;
  --bg-header: #fff;
  --bg-card: #fff;
  --text-primary: #404040;
  --text-secondary: #7a7a7a;
  --accent-color: #7229e6;
  --button-primary: linear-gradient(135deg, #8a2be2 30%, #4e9bff 100%);
  --input-border: #b7b7de;
}

html, body {
  height: 100%;
  margin: 0; padding: 0;
  box-sizing: border-box;
}
body {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background: var(--bg-primary);
  color: var(--text-primary);
  font-family: 'Roboto', Arial, sans-serif;
}
.wrapper-admin {
  flex: 1 0 auto;
  display: flex;
  flex-direction: column;
}
header {
  background: var(--bg-header);
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
  color: var(--accent-color);
}
.header-logo {
  width: 40px; height: 40px;
  border-radius: 10px;
  object-fit: cover;
  margin-right: 8px;
}
nav ul {
  list-style: none;
  display: flex;
  gap: 24px;
  padding: 0; margin: 0;
}
nav a {
  color: var(--text-primary);
  text-decoration: none;
  font-weight: 500;
  padding: 7px 15px;
  border-radius: 18px;
  transition: background 0.2s;
  font-size: 1rem;
}
nav a:hover {
  background: var(--accent-color);
  color: #fff;
}
/* Switch de tema */
.theme-switcher button, #theme-toggle {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.32rem;
  color: var(--text-primary);
  border-radius: 50%;
  padding: 0 2px;
  transition: transform 0.17s, background 0.2s;
}
.theme-switcher button:active, #theme-toggle:active {
  transform: scale(1.18);
}
.light-icon { display: none; }
.dark-icon { display: inline; }
[data-theme="light"] .dark-icon { display: none; }
[data-theme="light"] .light-icon { display: inline; }

/* Perfil y dropdown */
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
.profile-dropdown img, .profile-pic {
  width: 40px; height: 40px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid var(--accent-color);
  background: #18182a;
  flex-shrink: 0;
}
.profile-name {
  color: var(--text-primary);
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
  background: var(--bg-card);
  min-width: 175px;
  border-radius: 10px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.26);
  z-index: 999;
  border: 1px solid var(--input-border);
  animation: fadeIn .22s cubic-bezier(.3,1.2,.5,1.1);
  overflow: hidden;
}
.dropdown-content.show { display: block; }
.dropdown-content a {
  display: block;
  padding: 12px;
  color: var(--text-primary);
  text-decoration: none;
  transition: background 0.2s, color 0.2s;
  border-radius: 0;
  font-size: 1rem;
}
.dropdown-content a:hover {
  background: var(--bg-secondary);
  color: var(--accent-color);
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-10px);}
  to { opacity: 1; transform: translateY(0);}
}

/* PANEL ADMIN */
.admin-panel {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  padding: 90px 20px 30px 20px;
  min-height: 0;
}
.admin-panel h1 {
  color: var(--accent-color);
  margin-bottom: 18px;
  font-size: 2.3rem;
}
.admin-panel p {
  color: var(--text-secondary);
  margin-bottom: 36px;
  font-size: 1.07rem;
  max-width: 780px;
  margin-left: auto;
  margin-right: auto;
}
.admin-options {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 24px;
  max-width: 1200px;
  margin: 0 auto;
}
.admin-card {
  background: var(--bg-card);
  padding: 28px 22px;
  border-radius: 14px;
  width: 370px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.23);
  border: 1px solid var(--input-border);
  display: flex;
  flex-direction: column;
  align-items: center;
  transition: all 0.28s ease;
}
.admin-card:hover {
  transform: translateY(-7px) scale(1.016);
  box-shadow: 0 10px 26px rgba(0,0,0,0.30);
}
.admin-card h3 {
  font-size: 1.18rem;
  margin: 13px 0;
  color: var(--text-primary);
  text-align: center;
}
.admin-card a {
  display: inline-block;
  background: var(--button-primary);
  color: #fff;
  font-weight: bold;
  text-decoration: none;
  padding: 10px 20px;
  border-radius: 7px;
  transition: all 0.24s;
  margin-top: auto;
  width: fit-content;
}
.admin-card a:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0,0,0,0.18);
}
.admin-card i {
  font-size: 2.1rem;
  color: var(--accent-color);
  margin-bottom: 8px;
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

/* FOOTER SIEMPRE ABAJO */
footer {
  flex-shrink: 0;
  width: 100%;
  text-align: center;
  padding: 24px 8px 12px 8px;
  background: var(--bg-tertiary);
  color: var(--text-secondary);
  font-size: 0.99rem;
  box-shadow: 0 -2px 10px rgba(137,129,248,0.12);
  border-top: 1px solid var(--input-border);
  margin-top: auto;
}

/* ========== RESPONSIVE QUERIES ========== */
@media (min-width: 1440px) {
  .admin-options { gap: 44px; max-width: 1400px; }
  .admin-panel h1 { font-size: 2.8rem; }
  .admin-card { width: 460px; padding: 36px; }
}
@media (max-width: 1020px) {
  .admin-options { gap: 14px; }
  .admin-card { width: 99vw; max-width: 98vw; }
}
@media (max-width: 900px) {
  .header-container { padding: 13px 8px; gap: 10px; }
  .admin-panel { padding: 80px 3vw 20px 3vw; }
  .admin-card { min-width: 92vw; padding: 16px 3vw;}
  .admin-panel h1 { font-size: 2rem;}
  .profile-name { max-width: 110px; font-size: 0.95rem;}
}
@media (max-width: 700px) {
  .admin-panel h1 { font-size: 1.4rem;}
  .profile-name { max-width: 85px; font-size: .96rem;}
}
@media (max-width: 480px) {
  .header-logo { width: 27px; height: 27px; }
  .logo { font-size: 0.91rem;}
  .admin-panel h1 { font-size: 1.08rem;}
  .admin-card, .admin-panel p { font-size: 0.91rem; }
  .admin-card h3 { font-size: 0.89rem;}
  .profile-dropdown img { width: 27px; height: 27px;}
  .profile-name { max-width: 65vw; font-size: 0.90rem;}
  footer { padding: 13px 2px 9px 2px; font-size: 0.91rem;}
}
  </style>
</head>

<body>
  <header>
    <div class="header-container">
      <div class="logo">
        <img class="header-logo" src="../../assets/imgs/Logito.png" alt="Logo Manga Verse">
        <span>Manga Verse — Admin</span>
      </div>

      <div class="theme-switcher">
        <button id="theme-toggle" aria-label="Cambiar tema">
          <span class="dark-icon">🌙</span><span class="light-icon">☀️</span>
        </button>
      </div>

      <div class="profile">
        <div class="profile-dropdown" onclick="toggleDropdown()">
          <img class="profile-pic" src="<?= $fotoPerfil ?>" alt="Perfil">
          <span class="profile-name"><?= htmlspecialchars($_SESSION['nombre']) ?> ▼</span>
        </div>
        <div class="dropdown-content" id="dropdownMenu">
          <a href="../editar_perfil.php"><i class="fas fa-edit"></i> Editar perfil</a>
          <a href="../../Auth/logout.php"><i class="fas fa-sign-out-alt"></i> Cerrar sesión</a>
        </div>
      </div>
    </div>
  </header>

  <main class="admin-panel">
    <h1>Panel de Control</h1>
    <p>Bienvenido, administrador. Elige una sección para gestionar el sistema:</p>

    <div class="admin-options">
      <div class="admin-card">
        <i class="fas fa-users"></i>
        <h3>Gestión de Usuarios</h3>
        <a href="admin_panel.php">Administrar</a>
      </div>
      <div class="admin-card">
        <i class="fas fa-layer-group"></i>
        <h3>Ver Catálogo</h3>
        <a href="../catalogo_admin.php">Catálogo</a>
      </div>
      <div class="admin-card">
        <i class="fas fa-database"></i>
        <h3>Administración de Datos</h3>
        <a href="admin_datos.php">Gestionar</a>
      </div>
      <div class="admin-card">
        <i class="fas fa-file-pdf"></i>
        <h3>Reporte de Suscriptores</h3>
        <a href="reporte_suscriptores.php">Generar PDF</a>
      </div>
    </div>
  </main>

  <footer>
    <p>&copy; 2025 Manga Verse — Panel de administración</p>
  </footer>

  <script src="../../assets/js/theme-switcher.js"></script>
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