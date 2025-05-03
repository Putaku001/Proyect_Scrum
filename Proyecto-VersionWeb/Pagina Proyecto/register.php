<?php
include("db.php"); // Asegúrate que la ruta es correcta

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $nombre = $_POST['nombre'];
    $email = $_POST['email'];
    $password = $_POST['password'];
    $rolId = 1;

    // Hashear en PHP usando SHA-256
    $passwordHash = hash('sha256', $password);

    $sql = "INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, RolID)
            VALUES (?, ?, ?, ?)";
    $params = [$nombre, $email, $passwordHash, $rolId];
    $stmt = sqlsrv_query($conn, $sql, $params);

    if ($stmt) {
        echo "<script>alert('Registro exitoso. Puedes iniciar sesión.'); window.location.href='login.html';</script>";
    } else {
        echo "<script>alert('Error al registrar. Verifica que el correo o nombre no estén en uso.'); window.location.href='register.html';</script>";
    }
}
?>