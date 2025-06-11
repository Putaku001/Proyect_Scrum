<?php
session_start();
include("../Config/db.php");

$error = '';
$success = '';
$show_password_form = false;

if (!isset($_SESSION['reset_email']) || !isset($_SESSION['reset_code'])) {
    header("Location: forgot_password.php");
    exit();
}

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    if (isset($_POST['verify_code'])) {
        $user_code = $_POST['code'];
        
        if ($user_code == $_SESSION['reset_code']) {
            $success = "Código verificado correctamente. Por favor, ingresa tu nueva contraseña.";
            $show_password_form = true;
        } else {
            $error = "Código incorrecto. Por favor, intenta nuevamente.";
        }
    } elseif (isset($_POST['reset_password'])) {
        $new_password = $_POST['new_password'];
        $confirm_password = $_POST['confirm_password'];
        
        if ($new_password !== $confirm_password) {
            $error = "Las contraseñas no coinciden.";
            $show_password_form = true;
        } else {
            $new_password_hash = hash('sha256', $new_password);
            
            $sql = "UPDATE Usuarios SET ContrasenaHash = ? WHERE Email = ?";
            $params = array($new_password_hash, $_SESSION['reset_email']);
            $stmt = sqlsrv_query($conn, $sql, $params);
            
            if ($stmt) {
                $success = "¡Contraseña actualizada con éxito! Serás redirigido al login en 3 segundos.";
                
                unset($_SESSION['reset_code']);
                unset($_SESSION['reset_email']);
                
                header("Refresh: 3; url=../Public/login.html");
                
                echo '<!DOCTYPE html>
                <html lang="es">
                <head>
                  <meta charset="UTF-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1.0">
                  <title>Contraseña Actualizada - Manga Verse</title>
                  <link rel="stylesheet" href="../assets/css/style.css">
                  <style>
                    .success-message {
                      background-color: #4CAF50;
                      color: white;
                      padding: 20px;
                      border-radius: 5px;
                      text-align: center;
                      max-width: 500px;
                      margin: 50px auto;
                      box-shadow: 0 4px 8px rgba(0,0,0,0.1);
                    }
                    .success-icon {
                      font-size: 50px;
                      margin-bottom: 20px;
                      color: #fff;
                    }
                  </style>
                </head>
                <body>
                  <div class="success-message">
                    <div class="success-icon">✓</div>
                    <h2>¡Contraseña Actualizada!</h2>
                    <p>Tu contraseña ha sido cambiada exitosamente.</p>
                    <p>Serás redirigido automáticamente a la página de inicio de sesión.</p>
                  </div>
                </body>
                </html>';
                exit();
            } else {
                $error = "Error al actualizar la contraseña. Por favor, intenta nuevamente.";
                $show_password_form = true;
            }
        }
    }
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Restablecer Contraseña - Manga Verse</title>
  <link rel="stylesheet" href="../assets/css/style.css">
  <style>
    .alert {
      padding: 15px;
      margin: 15px 0;
      border-radius: 4px;
      font-size: 16px;
    }
    .error {
      background-color: #FFEBEE;
      color: #C62828;
      border-left: 4px solid #EF5350;
    }
    .success {
      background-color: #E8F5E9;
      color: #2E7D32;
      border-left: 4px solid #4CAF50;
    }
    .form-container {
      max-width: 500px;
      margin: 0 auto;
      padding: 20px;
      background: linear-gradient(135deg, #1c1c28, #2c2f4a);
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    }
    .form-group {
      margin-bottom: 20px;
    }
    .form-group label {
      display: block;
      margin-bottom: 8px;
      font-weight: 600;
    }
    .form-control {
      width: 100%;
      padding: 10px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 16px;
    }
    .btn {
      background-color: #4CAF50;
      color: white;
      padding: 12px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 16px;
      width: 100%;
    }
    .btn:hover {
      background-color: #45a049;
    }
  </style>
</head>
<body>
  <header>
    <div class="header-container">
      <div class="logo">
        <img src="../assets/imgs/Logito.png" alt="Logo Manga Verse" class="header-logo">
        <span>Manga Verse</span>
      </div>
      <nav>
        <ul>
          <li><a href="../Public/index.html">Inicio</a></li>
          <li><a href="#">Mi Lista</a></li>
          <li><a href="#">Catálogo</a></li>
        </ul>
      </nav>
            <!-- Selector de tema oscuro/claro -->
            <div class="theme-switcher">
        <button id="theme-toggle" aria-label="Cambiar tema">
          <span class="dark-icon">🌙</span> <!-- Icono para tema oscuro -->
          <span class="light-icon">☀️</span> <!-- Icono para tema claro -->
        </button>
      </div>
      <div class="profile">
        <a href="../Auth/login.php" id="login-link">Iniciar Sesión</a>
      </div>
    </div>
  </header>
  <main class="login-container">
    <div class="form-container">
      <h2>Restablecer Contraseña</h2>
      
      <?php if (!empty($error)): ?>
        <div class="alert error"><?php echo htmlspecialchars($error); ?></div>
      <?php endif; ?>
      
      <?php if (!empty($success) && !$show_password_form): ?>
        <div class="alert success"><?php echo htmlspecialchars($success); ?></div>
      <?php endif; ?>

      <?php if (!$show_password_form): ?>
        <form action="reset_password.php" method="POST">
          <div class="form-group">
            <label for="code">Código de verificación</label>
            <input type="text" name="code" id="code" class="form-control" required placeholder="Ingresa el código recibido por email">
          </div>
          <button type="submit" name="verify_code" class="btn">Verificar Código</button>
        </form>
      <?php else: ?>
        <form action="reset_password.php" method="POST">
          <div class="form-group">
            <label for="new_password">Nueva Contraseña</label>
            <input type="password" name="new_password" id="new_password" class="form-control" required placeholder="Ingresa tu nueva contraseña">
          </div>
          <div class="form-group">
            <label for="confirm_password">Confirmar Contraseña</label>
            <input type="password" name="confirm_password" id="confirm_password" class="form-control" required placeholder="Confirma tu nueva contraseña">
          </div>
          <button type="submit" name="reset_password" class="btn">Cambiar Contraseña</button>
        </form>
      <?php endif; ?>
      
      <p style="text-align: center; margin-top: 20px;">
        <a href="../Public/login.html">Volver a Iniciar Sesión</a>
      </p>
    </div>
  </main>

  <footer>
    <p>&copy; 2025 Manga Verse. Todos los derechos reservados.</p>
  </footer>
</body>
</html>