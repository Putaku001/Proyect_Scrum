<?php
session_start();
if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: admin_dashboard.php");
    exit();
}

include("../../Config/db.php");

$sql = "SELECT UsuarioID, NombreUsuario, Email, EsPremium, FechaRegistro, RolID FROM Usuarios";
$stmt = sqlsrv_query($conn, $sql);
$usuarios = [];
while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
    $usuarios[] = $row;
}
?>
<!DOCTYPE html>
<html lang="es" data-theme="dark">
<head>
  <meta charset="UTF-8">
  <title>Gestión de Usuarios - Manga Verse</title>
  <link rel="stylesheet" href="../../assets/css/style.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css"/>
  <style>
    :root {
      --bg-primary: #1c1c28;
      --bg-secondary: #2c2f4a;
      --bg-card: #2a2a3a;
      --bg-tertiary: #151526;
      --accent-color: #8a2be2;
      --button-primary: linear-gradient(135deg, #8a2be2 40%, #4e9bff 100%);
      --input-border: #39396b;
      --text-primary: #f0f0f0;
      --text-secondary: #a0a0b0;
    }
    [data-theme="light"] {
      --bg-primary: #f8f9ff;
      --bg-secondary: #f0f0f0;
      --bg-card: #fff;
      --bg-tertiary: #d4e6f9;
      --accent-color: #7229e6;
      --button-primary: linear-gradient(135deg, #8a2be2 30%, #4e9bff 100%);
      --input-border: #b7b7de;
      --text-primary: #404040;
      --text-secondary: #7a7a7a;
    }

    body {
      min-height: 100vh;
      margin: 0;
      padding: 0;
      box-sizing: border-box;
      font-family: 'Roboto', Arial, sans-serif;
      background: var(--bg-primary);
      color: var(--text-primary);
    }

    /* === BOTÓN FIJO ESQUINA IZQUIERDA === */
    .volver-btn-fixed {
      position: fixed;
      top: 22px;
      left: 22px;
      z-index: 2000;
      background: linear-gradient(135deg, var(--accent-color, #8a2be2) 40%, #4e9bff 100%);
      color: #fff;
      padding: 13px 34px 13px 23px;
      border-radius: 40px;
      text-decoration: none;
      font-weight: bold;
      font-size: 1.09rem;
      letter-spacing: .2px;
      box-shadow: 0 4px 18px 0 rgba(87,40,230,0.07);
      display: inline-flex;
      align-items: center;
      gap: 10px;
      border: none;
      outline: none;
      cursor: pointer;
      transition: background .22s, box-shadow .22s, transform .16s;
      min-width: 120px;
      margin-bottom: 3px;
      user-select: none;
    }
    .volver-btn-fixed i {
      font-size: 1.15em;
      margin-right: 1px;
      margin-left: -4px;
      transition: transform 0.18s;
    }
    .volver-btn-fixed:hover, .volver-btn-fixed:focus {
      background: linear-gradient(135deg, #6f21b5 10%, #20bfff 110%);
      box-shadow: 0 6px 18px rgba(87,40,230,0.15);
      transform: translateY(-2px) scale(1.03);
    }
    .volver-btn-fixed:active {
      background: linear-gradient(135deg, #5f14a1 10%, #1094c5 110%);
      transform: scale(0.99);
    }
    @media (max-width: 820px) {
      .volver-btn-fixed {
        top: 10px;
        left: 10px;
        font-size: 1rem;
        padding: 11px 19px 11px 14px;
        min-width: 80px;
        border-radius: 28px;
        gap: 6px;
      }
      .volver-btn-fixed i {
        font-size: 1em;
        margin-left: -2px;
      }
    }
    @media (max-width: 450px) {
      .volver-btn-fixed {
        font-size: 0.93rem;
        padding: 9px 10px 9px 9px;
        min-width: 56px;
        border-radius: 20px;
        gap: 4px;
      }
      .volver-btn-fixed i {
        font-size: 0.96em;
        margin-left: -1px;
      }
      .volver-text { display: none; }
    }

    .admin-container {
      max-width: 1200px;
      margin: 90px auto 50px;
      padding: 0 20px;
    }

    h1 {
      text-align: center;
      color: var(--accent-color);
      margin-bottom: 30px;
      font-size: 2.2rem;
    }

    .admin-options {
      display: flex;
      justify-content: center;
      gap: 30px;
      margin: 40px 0;
      flex-wrap: wrap;
    }

    .admin-card {
      background: var(--bg-card);
      padding: 30px;
      border-radius: 15px;
      width: 280px;
      text-align: center;
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
      transition: all 0.3s ease;
      cursor: pointer;
      border: 1px solid var(--input-border);
    }

    .admin-card:hover {
      transform: translateY(-6px);
      box-shadow: 0 8px 16px rgba(0,0,0,0.2);
    }

    .admin-card i {
      font-size: 2.5rem;
      color: var(--accent-color);
      margin-bottom: 15px;
    }

    .admin-card h3 {
      margin: 0 0 10px 0;
      color: var(--text-primary);
      font-size: 1.3rem;
    }

    .admin-card p {
      color: var(--text-secondary);
      margin: 0;
      font-size: 0.9rem;
    }

    .seccion {
      display: none;
      animation: fadeIn 0.5s ease forwards;
      margin-top: 40px;
    }

    @keyframes fadeIn {
      from { opacity: 0; transform: translateY(10px); }
      to { opacity: 1; transform: translateY(0); }
    }

    table {
      width: 100%;
      border-collapse: collapse;
      background: var(--bg-card);
      border-radius: 12px;
      overflow: hidden;
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
      border: 1px solid var(--input-border);
    }

    th, td {
      padding: 15px;
      text-align: center;
      border-bottom: 1px solid var(--input-border);
    }

    th {
      background: var(--bg-secondary);
      color: var(--text-primary);
      font-weight: 600;
    }

    tr:hover {
      background: var(--bg-secondary);
    }

    .actions {
      display: flex;
      justify-content: center;
      gap: 10px;
    }

    .actions a {
      padding: 8px 16px;
      border-radius: 6px;
      text-decoration: none;
      font-weight: 500;
      transition: all 0.3s ease;
      display: inline-flex;
      align-items: center;
      gap: 6px;
      font-size: 0.9rem;
    }

    .edit-btn {
      background: var(--accent-color);
      color: white;
    }

    .delete-btn {
      background: #ff4c4c;
      color: white;
    }

    .actions a:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.2);
    }

    .form-container {
      max-width: 600px;
      margin: 0 auto;
      padding: 30px;
      background: var(--bg-card);
      border-radius: 15px;
      box-shadow: 0 8px 16px rgba(0,0,0,0.1);
      border: 1px solid var(--input-border);
    }

    .form-container h2 {
      text-align: center;
      color: var(--accent-color);
      margin-bottom: 20px;
    }

    .form-container input,
    .form-container select {
      width: 100%;
      padding: 12px 15px;
      margin: 10px 0;
      border: 1px solid var(--input-border);
      border-radius: 8px;
      background: var(--input-bg);
      color: var(--text-primary);
      font-size: 1rem;
      transition: border-color 0.3s ease;
    }

    .form-container input:focus,
    .form-container select:focus {
      outline: none;
      border-color: var(--accent-color);
      box-shadow: 0 0 0 2px rgba(138, 43, 226, 0.2);
    }

    .form-container button {
      width: 100%;
      padding: 12px;
      background: var(--button-primary);
      color: white;
      font-weight: bold;
      border: none;
      border-radius: 8px;
      margin-top: 15px;
      cursor: pointer;
      transition: all 0.3s ease;
      font-size: 1rem;
    }

    .form-container button:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.2);
    }

    .theme-switcher {
      position: fixed;
      top: 30px;
      right: 30px;
      z-index: 10;
    }

    @media (max-width: 1024px) {
      .admin-container {
        padding: 0 15px;
      }
      .admin-card {
        width: 220px;
        padding: 25px;
      }
      th, td {
        padding: 12px;
        font-size: 0.9rem;
      }
    }

    @media (max-width: 768px) {
      .admin-container {
        margin-top: 100px;
      }
      h1 {
        font-size: 1.8rem;
      }
      .admin-card {
        width: 100%;
        max-width: 300px;
        padding: 20px;
      }
      .admin-card i {
        font-size: 2rem;
      }
      .admin-card h3 {
        font-size: 1.1rem;
      }
      table {
        font-size: 0.85rem;
      }
      th, td {
        padding: 10px;
      }
      .actions {
        flex-direction: column;
        gap: 8px;
      }
      .actions a {
        width: 100%;
        justify-content: center;
        padding: 6px 12px;
      }
      .form-container {
        padding: 20px;
      }
    }

    @media (max-width: 576px) {
      .admin-container {
        margin: 80px auto 30px;
      }
      h1 {
        font-size: 1.6rem;
        margin-bottom: 20px;
      }
      .admin-options {
        gap: 15px;
      }
      .admin-card {
        padding: 15px;
      }
      .admin-card i {
        font-size: 1.8rem;
        margin-bottom: 10px;
      }
      table {
        display: block;
        overflow-x: auto;
      }
      .form-container input,
      .form-container select,
      .form-container button {
        padding: 10px 12px;
      }
    }

    @media (max-width: 480px) {
      .admin-container {
        margin-top: 70px;
      }
      h1 {
        font-size: 1.4rem;
      }
      .admin-card h3 {
        font-size: 1rem;
      }
      .admin-card p {
        font-size: 0.8rem;
      }
      .form-container {
        padding: 15px;
      }
    }
  </style>
</head>
<body>
<!-- BOTÓN FIJO VOLVER -->
<a href="admin_dashboard.php" class="volver-btn-fixed">
  <i class="fas fa-arrow-left"></i>
  <span class="volver-text">Volver al Dashboard</span>
</a>

<div class="admin-container">
  <div class="theme-switcher">
    <button id="theme-toggle" aria-label="Cambiar tema">
      <span class="dark-icon">🌙</span><span class="light-icon">☀️</span>
    </button>
  </div>

  <h1><i class="fas fa-users-cog"></i> Panel de Administración de Usuarios</h1>

  <div class="admin-options">
    <div class="admin-card" onclick="mostrarSeccion('seccionTabla')">
      <i class="fas fa-user-edit"></i>
      <h3>Gestión de Usuarios</h3>
      <p>Ver, editar o eliminar usuarios existentes</p>
    </div>
    <div class="admin-card" onclick="mostrarSeccion('seccionFormulario')">
      <i class="fas fa-user-plus"></i>
      <h3>Crear Usuario</h3>
      <p>Agregar nuevos usuarios al sistema</p>
    </div>
  </div>

  <div id="seccionTabla" class="seccion">
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Email</th>
          <th>Premium</th>
          <th>Fecha Registro</th>
          <th>Rol</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <?php foreach ($usuarios as $usuario): ?>
          <tr>
            <td><?= $usuario['UsuarioID'] ?></td>
            <td><?= htmlspecialchars($usuario['NombreUsuario']) ?></td>
            <td><?= htmlspecialchars($usuario['Email']) ?></td>
            <td><?= $usuario['EsPremium'] ? '✅' : '❌' ?></td>
            <td><?= $usuario['FechaRegistro']->format('Y-m-d') ?></td>
            <td><?= $usuario['RolID'] == 2 ? '🛡️ Admin' : '👤 Usuario' ?></td>
            <td class="actions">
              <a href="editar_usuario.php?id=<?= $usuario['UsuarioID'] ?>" class="edit-btn"><i class="fas fa-edit"></i> Editar</a>
              <a href="eliminar_usuario.php?id=<?= $usuario['UsuarioID'] ?>" class="delete-btn" onclick="return confirm('¿Estás seguro de eliminar este usuario?')"><i class="fas fa-trash-alt"></i> Eliminar</a>
            </td>
          </tr>
        <?php endforeach; ?>
      </tbody>
    </table>
  </div>

  <div id="seccionFormulario" class="seccion">
    <div class="form-container">
      <h2><i class="fas fa-user-plus"></i> Crear Nuevo Usuario</h2>
      <form method="post" action="crear_usuario.php">
        <input type="text" name="nombre" placeholder="Nombre de usuario" required>
        <input type="email" name="email" placeholder="Correo electrónico" required>
        <input type="password" name="password" placeholder="Contraseña" required>
        <select name="rol" required>
          <option value="">Seleccionar rol...</option>
          <option value="1">Usuario</option>
          <option value="2">Administrador</option>
        </select>
        <button type="submit"><i class="fas fa-user-plus"></i> Crear Usuario</button>
      </form>
    </div>
  </div>
</div>

<script>
function mostrarSeccion(id) {
  document.getElementById('seccionTabla').style.display = 'none';
  document.getElementById('seccionFormulario').style.display = 'none';
  document.getElementById(id).style.display = 'block';
}
</script>
<script src="../../assets/js/theme-switcher.js"></script>
</body>
</html>
