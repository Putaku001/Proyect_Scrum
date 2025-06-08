<?php
include("db.php");

$id = $_GET['id'];
$sql = "SELECT * FROM Usuarios WHERE UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, [$id]);
$user = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
?>

<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <title>Editar Usuario</title>
  <link rel="stylesheet" href="./css/style.css">
  <style>
    body {
      font-family: 'Segoe UI', sans-serif;
      background-color: #1e1e2f;
      color: #f0f0f0;
      padding: 40px;
      margin: 0;
    }

    .form-container {
      max-width: 500px;
      margin: auto;
      background-color: #2c2f4a;
      padding: 30px;
      border-radius: 12px;
      box-shadow: 0 6px 20px rgba(0, 0, 0, 0.4);
    }

    .form-container h2 {
      text-align: center;
      color: #00d4ff;
      margin-bottom: 25px;
    }

    label {
      display: block;
      margin-bottom: 6px;
      margin-top: 15px;
      font-weight: bold;
      color: #ccc;
    }

    input[type="text"],
    input[type="email"],
    select {
      width: 100%;
      padding: 12px;
      border-radius: 8px;
      border: none;
      background-color: #3b3e5c;
      color: #f0f0f0;
      margin-bottom: 10px;
    }

    input[type="checkbox"] {
      transform: scale(1.2);
      margin-right: 10px;
      vertical-align: middle;
    }

    .checkbox-container {
      margin-top: 10px;
      margin-bottom: 20px;
    }

    button[type="submit"] {
      width: 100%;
      padding: 12px;
      border: none;
      border-radius: 8px;
      background: linear-gradient(135deg, #00d4ff, #1e90ff);
      color: #000;
      font-weight: bold;
      cursor: pointer;
      transition: background 0.3s;
    }

    button[type="submit"]:hover {
      background: linear-gradient(135deg, #00aacc, #0077cc);
    }

    .back-link {
      display: block;
      text-align: center;
      margin-top: 25px;
      color: #00d4ff;
      text-decoration: none;
    }

    .back-link:hover {
      text-decoration: underline;
    }
  </style>
</head>
<body>

  <div class="form-container">
    <h2>Editar Usuario</h2>
    <form action="actualizar_usuario.php" method="post">
      <input type="hidden" name="id" value="<?= $user['UsuarioID'] ?>">

      <label for="nombre">Nombre de Usuario</label>
      <input type="text" name="nombre" id="nombre" value="<?= htmlspecialchars($user['NombreUsuario']) ?>" required>

      <label for="email">Correo Electrónico</label>
      <input type="email" name="email" id="email" value="<?= htmlspecialchars($user['Email']) ?>" required>

      <label for="rol">Rol</label>
      <select name="rol" id="rol">
        <option value="1" <?= $user['RolID'] == 1 ? 'selected' : '' ?>>Usuario</option>
        <option value="2" <?= $user['RolID'] == 2 ? 'selected' : '' ?>>Administrador</option>
      </select>

      <div class="checkbox-container">
        <label><input type="checkbox" name="premium" value="1" <?= $user['EsPremium'] ? 'checked' : '' ?>> Usuario Premium</label>
      </div>

      <button type="submit">Actualizar</button>
    </form>
    <a href="admin_panel.php" class="back-link">← Volver al panel</a>
  </div>

</body>
</html>
