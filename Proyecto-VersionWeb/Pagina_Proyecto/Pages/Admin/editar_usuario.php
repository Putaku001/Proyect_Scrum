<?php
include("../../Config/db.php");

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
      font-family: 'Roboto', sans-serif;
      background: linear-gradient(135deg, var(--bg-primary), var(--bg-secondary));
      color: var(--text-primary);
      padding: 0;
      margin: 0;
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
    }

    .admin-container {
      width: 100%;
      max-width: 600px;
      margin: 30px auto;
      padding: 20px;
    }

    .form-container {
      background: var(--bg-card);
      padding: 30px;
      border-radius: 15px;
      box-shadow: 0 8px 32px rgba(137, 129, 248, 0.3);
      border: 1px solid var(--input-border);
    }

    .form-container h2 {
      text-align: center;
      color: var(--accent-color);
      margin-bottom: 30px;
      font-size: 1.8rem;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 10px;
    }

    label {
      display: block;
      margin-bottom: 8px;
      margin-top: 20px;
      font-weight: 500;
      color: var(--text-primary);
    }

    input[type="text"],
    input[type="email"],
    select {
      width: 100%;
      padding: 14px;
      border-radius: 8px;
      border: 1px solid var(--input-border);
      background: var(--input-bg);
      color: var(--text-primary);
      margin-bottom: 5px;
      font-size: 1rem;
      transition: border-color 0.3s ease, box-shadow 0.3s ease;
    }

    input[type="text"]:focus,
    input[type="email"]:focus,
    select:focus {
      outline: none;
      border-color: var(--accent-color);
      box-shadow: 0 0 0 3px rgba(138, 43, 226, 0.2);
    }

    input[type="checkbox"] {
      width: 18px;
      height: 18px;
      margin-right: 10px;
      accent-color: var(--accent-color);
    }

    .checkbox-container {
      margin: 25px 0;
      display: flex;
      align-items: center;
    }

    .checkbox-container label {
      margin: 0;
      display: flex;
      align-items: center;
      cursor: pointer;
    }

    button[type="submit"] {
      width: 100%;
      padding: 14px;
      border: none;
      border-radius: 8px;
      background: var(--button-primary);
      color: white;
      font-weight: bold;
      font-size: 1rem;
      cursor: pointer;
      transition: all 0.3s ease;
      margin-top: 10px;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
    }

    button[type="submit"]:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 12px rgba(0,0,0,0.2);
    }

    .back-link {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      margin-top: 25px;
      color: var(--accent-color);
      text-decoration: none;
      font-weight: 500;
      transition: all 0.3s ease;
    }

    .back-link:hover {
      text-decoration: underline;
      transform: translateX(-3px);
    }

    .theme-switcher {
      position: fixed;
      top: 30px;
      right: 30px;
      z-index: 10;
    }

    #theme-toggle {
      width: 50px;
      height: 50px;
      border-radius: 50%;
      background: var(--bg-card);
      border: 2px solid var(--accent-color);
      color: var(--accent-color);
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.5rem;
      transition: all 0.3s ease;
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    }

    #theme-toggle:hover {
      background: var(--accent-color);
      color: var(--button-text-color);
      transform: scale(1.1);
      box-shadow: 0 6px 16px rgba(138, 43, 226, 0.3);
    }

    @media (max-width: 768px) {
      .admin-container {
        padding: 15px;
        margin-top: 70px;
      }
      
      .form-container {
        padding: 25px;
      }
      
      .theme-switcher {
        top: 20px;
        right: 20px;
      }
      
      #theme-toggle {
        width: 44px;
        height: 44px;
        font-size: 1.3rem;
      }
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
