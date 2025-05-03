<?php
session_start();
include("db.php"); // Asegúrate que la ruta es correcta

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $email = $_POST['email'];
    $password = $_POST['password'];

    // Aplicar el mismo hash que en el registro
    $passwordHash = hash('sha256', $password);

    $sql = "
        SELECT UsuarioID, NombreUsuario, RolID 
        FROM Usuarios 
        WHERE Email = ? 
        AND ContrasenaHash = ?
    ";
    $params = array($email, $passwordHash);
    $stmt = sqlsrv_query($conn, $sql, $params);

    if ($stmt && sqlsrv_has_rows($stmt)) {
        $usuario = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
        $_SESSION['usuario_id'] = $usuario['UsuarioID'];
        $_SESSION['nombre'] = $usuario['NombreUsuario'];
        $_SESSION['rol'] = $usuario['RolID'];

        header("Location: dashboard.php");
        exit();
    } else {
        echo "<script>alert('Correo o contraseña incorrectos.'); window.location.href='login.html';</script>";
    }
}
?>