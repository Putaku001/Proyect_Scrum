<?php
session_start();
if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: admin_dashboard.php");
    exit();
}

include("db.php");

$sql = "SELECT UsuarioID, NombreUsuario, Email, EsPremium, FechaRegistro, RolID FROM Usuarios";
$stmt = sqlsrv_query($conn, $sql);
$usuarios = [];
while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
    $usuarios[] = $row;
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Gestión de Usuarios - Manga Verse</title>
  <link rel="stylesheet" href="./css/style.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css"/>
  <style>
    body {
      background-color: #1e1e2f;
      color: #f0f0f0;
      font-family: 'Segoe UI', sans-serif;
      margin: 0;
      padding: 30px;
    }

    h1 {
      text-align: center;
      color: #00d4ff;
    }

    .top-links {
      text-align: center;
      margin-bottom: 20px;
    }

    .top-links a {
      background-color: #00d4ff;
      color: #000;
      padding: 10px 20px;
      border-radius: 8px;
      text-decoration: none;
      font-weight: bold;
    }

    .admin-options {
      display: flex;
      justify-content: center;
      gap: 40px;
      margin-top: 40px;
      flex-wrap: wrap;
    }

    .admin-card {
      background-color: #1e2238;
      padding: 25px;
      border-radius: 15px;
      width: 260px;
      text-align: center;
      box-shadow: 0 4px 12px rgba(0,0,0,0.4);
      transition: transform 0.3s ease;
      cursor: pointer;
    }

    .admin-card:hover {
      transform: translateY(-6px);
    }

    .admin-card i {
      font-size: 2.5rem;
      color: #00d4ff;
      margin-bottom: 10px;
    }

    .admin-card h3 {
      margin: 0;
      color: #f0f0f0;
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
      background-color: #2c2f4a;
      border-radius: 10px;
      overflow: hidden;
    }

    th, td {
      padding: 15px;
      text-align: center;
      border-bottom: 1px solid #444;
    }

    th {
      background-color: #3d3f5c;
    }

    tr:hover {
      background-color: #3b3e5c;
    }

    .actions a {
      margin: 0 5px;
      padding: 6px 12px;
      border-radius: 5px;
      text-decoration: none;
      font-weight: bold;
    }

    .actions a:first-child {
      background-color: #00d4ff;
      color: #000;
    }

    .actions a:last-child {
      background-color: #ff4c4c;
      color: #fff;
    }

    .form-container {
      max-width: 500px;
      margin: 0 auto;
      padding: 25px;
      background-color: #2c2f4a;
      border-radius: 10px;
      box-shadow: 0 4px 10px rgba(0,0,0,0.3);
    }

    .form-container input,
    .form-container select {
      width: 100%;
      padding: 12px;
      margin: 10px 0;
      border: none;
      border-radius: 6px;
      background-color: #3b3e5c;
      color: #f0f0f0;
    }

    .form-container button {
      width: 100%;
      padding: 12px;
      background-color: #00d4ff;
      color: #000;
      font-weight: bold;
      border: none;
      border-radius: 6px;
      margin-top: 10px;
      cursor: pointer;
    }
  </style>
</head>
<body>

<h1>Panel de Administración de Usuarios</h1>

<div class="top-links">
  <a href="admin_dashboard.php">← Volver al Dashboard</a>
</div>

<div class="admin-options">
  <div class="admin-card" onclick="mostrarSeccion('seccionTabla')">
    <i class="fas fa-user-edit"></i>
    <h3>Editar Usuarios</h3>
  </div>
  <div class="admin-card" onclick="mostrarSeccion('seccionFormulario')">
    <i class="fas fa-user-plus"></i>
    <h3>Crear Usuario</h3>
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
            <a href="editar_usuario.php?id=<?= $usuario['UsuarioID'] ?>">✏️ Editar</a>
            <a href="eliminar_usuario.php?id=<?= $usuario['UsuarioID'] ?>" onclick="return confirm('¿Estás seguro de eliminar este usuario?')">🗑️ Eliminar</a>
          </td>
        </tr>
      <?php endforeach; ?>
    </tbody>
  </table>
</div>

<div id="seccionFormulario" class="seccion">
  <div class="form-container">
    <h2>Crear Nuevo Usuario</h2>
    <form method="post" action="crear_usuario.php">
      <input type="text" name="nombre" placeholder="Nombre de usuario" required>
      <input type="email" name="email" placeholder="Correo electrónico" required>
      <input type="password" name="password" placeholder="Contraseña" required>
      <select name="rol" required>
        <option value="1">Usuario</option>
        <option value="2">Administrador</option>
      </select>
      <button type="submit">Crear Usuario</button>
    </form>
  </div>
</div>

<script>
function mostrarSeccion(id) {
  document.getElementById('seccionTabla').style.display = 'none';
  document.getElementById('seccionFormulario').style.display = 'none';
  document.getElementById(id).style.display = 'block';
}
</script>

</body>
</html>
