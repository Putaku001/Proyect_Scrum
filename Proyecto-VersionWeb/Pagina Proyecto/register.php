<?php
include("db.php");

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $nombre = $_POST['nombre'];
    $email = $_POST['email'];
    $password = $_POST['password'];
    $rolId = 2; // Usuario normal

    $sql = "
        INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, RolID)
        VALUES (?, ?, HASHBYTES('SHA2_256', CONVERT(VARCHAR, ?)), ?)
    ";

    $params = [$nombre, $email, $password, $rolId];
    $stmt = sqlsrv_query($conn, $sql, $params);

    if ($stmt) {
        echo "<script>alert('Registro exitoso. Puedes iniciar sesión.'); window.location.href='login.html';</script>";
    } else {
        echo "<script>alert('Error al registrar. Verifica que el correo o nombre no estén en uso.'); window.location.href='register.html';</script>";
    }
}
?>
