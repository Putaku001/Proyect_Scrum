<?php
session_start();

require __DIR__ . '/../PHPMailer/src/Exception.php';
require __DIR__ . '/../PHPMailer/src/PHPMailer.php';
require __DIR__ . '/../PHPMailer/src/SMTP.php';

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;

include("../Config/db.php");

$error = '';
$success = '';

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $email = $_POST['email'];
    
    $sql = "SELECT Email, NombreUsuario FROM Usuarios WHERE Email = ?";
    $params = array($email);
    $stmt = sqlsrv_query($conn, $sql, $params);
    
    if ($stmt && sqlsrv_has_rows($stmt)) {
        $usuario = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
        $codigo = rand(100000, 999999);
        $_SESSION['reset_code'] = $codigo;
        $_SESSION['reset_email'] = $email;
        
        $mail = new PHPMailer(true);
        
        try {
            $mail->isSMTP();
            $mail->Host = 'smtp.gmail.com';
            $mail->SMTPAuth = true;
            $mail->Username = 'soportes.proyectostl@gmail.com'; 
            $mail->Password = 'dztopjihnjholgcv'; 
            $mail->SMTPSecure = PHPMailer::ENCRYPTION_STARTTLS;
            $mail->Port = 587;
            
            $mail->setFrom('no-reply@mangaverse.com', 'Manga Verse');
            $mail->addAddress($usuario['Email'], $usuario['NombreUsuario']);
            
            $mail->isHTML(true);
            $mail->Subject = 'Recuperacion';
            $mail->Body = "
                <h2>Hola {$usuario['NombreUsuario']},</h2>
                <p>Tu codigo de verificacion es: <strong>{$codigo}</strong></p>
                <p>Este codigo expirara en 15 minutos.</p>
            ";
            
            $mail->send();
            header("Location: reset_password.php");
            exit();
        } catch (Exception $e) {
            $error = "Error al enviar el correo: " . $mail->ErrorInfo;
            error_log("PHPMailer Error: " . $e->getMessage());
        }
    } else {
        $error = "No se encontro una cuenta con ese correo electronico.";
    }
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <title>Recuperar Contraseña - Manga Verse</title>
  <link rel="stylesheet" href="../assets/css/style.css" />
  <style>
    .alert {
      padding: 15px;
      margin-bottom: 20px;
      border: 1px solid transparent;
      border-radius: 4px;
    }
    .alert-error {
      color: #a94442;
      background-color: #f2dede;
      border-color: #ebccd1;
    }
    .alert-success {
      color: #3c763d;
      background-color: #dff0d8;
      border-color: #d6e9c6;
    }
  </style>
</head>
<body>
  <header>
    <div class="header-container">
      <div class="logo">
        <img src="../assets/imgs/Logito.png" alt="Logo Manga Verse" class="header-logo" />
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
    <div class="login-box">
      <h2>Recuperar Contraseña</h2>
      <?php if ($error): ?>
        <div class="alert alert-error"><?php echo htmlspecialchars($error); ?></div>
      <?php endif; ?>
      <?php if ($success): ?>
        <div class="alert alert-success"><?php echo htmlspecialchars($success); ?></div>
      <?php endif; ?>
      <form action="forgot_password.php" method="POST">
        <label for="email">Correo electrónico</label>
        <input type="email" name="email" id="email" required placeholder="Ingresa tu correo electrónico" value="<?php echo isset($_POST['email']) ? htmlspecialchars($_POST['email']) : ''; ?>">
        
        <button type="submit">Enviar Código</button>
      </form>
      <p>¿Recordaste tu contraseña? <a href="../Auth/login.php">Iniciar Sesión</a></p>
    </div>
  </main>

  <footer>
    <p>&copy; 2025 Manga Verse. Todos los derechos reservados.</p>
  </footer>
</body>
</html>