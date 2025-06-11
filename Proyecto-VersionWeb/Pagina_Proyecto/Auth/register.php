<?php
include("../Config/db.php");

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $nombre   = trim($_POST['nombre']);
    $email    = trim($_POST['email']);
    $password = $_POST['password'];
    $rolId    = 1; // Rol por defecto: Usuario

    /* ── Hash ── */
    $passwordHash = hash('sha256', $password);

    /* ── ¿Usuario o correo repetidos? ─────────────────── */
    $checkSql    = "SELECT COUNT(*) AS total FROM Usuarios WHERE NombreUsuario = ? OR Email = ?";
    $checkParams = [$nombre, $email];
    $checkStmt   = sqlsrv_query($conn, $checkSql, $checkParams);
    $exists      = sqlsrv_fetch_array($checkStmt, SQLSRV_FETCH_ASSOC);

    if ($exists['total'] > 0) {
        echo "<script>alert('El nombre de usuario o el correo ya están registrados.'); window.location.href='../Public/register.html';</script>";
        exit();
    }

    /* ── Insertar nuevo usuario (Avatar queda NULL) ───── */
    $sql    = "INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, RolID) VALUES (?, ?, ?, ?)";
    $params = [$nombre, $email, $passwordHash, $rolId];
    $stmt   = sqlsrv_query($conn, $sql, $params);

    if ($stmt) {
        echo "<script>alert('Registro exitoso. Puedes iniciar sesión.'); window.location.href='../Public/login.html';</script>";
    } else {
        $errors = print_r(sqlsrv_errors(), true);
        echo "<script>console.error(" . json_encode($errors) . "); alert('Error al registrar.'); window.location.href='../Public/register.html';</script>";
    }
}
?>
