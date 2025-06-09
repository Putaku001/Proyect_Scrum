<?php
session_start();
include("db.php");

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

$id = $_SESSION['usuario_id'];

// 1. Marcar al usuario como no premium
$sql = "UPDATE Usuarios SET EsPremium = 0 WHERE UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, [$id]);

// Validación simple
if ($stmt === false) {
    die(print_r(sqlsrv_errors(), true));
}

// 2. Redirigir de vuelta con mensaje
header("Location: editar_perfil.php?mensaje=cancelada");
exit();
