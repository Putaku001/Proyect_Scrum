<?php
session_start();
require_once "../../Config/db.php";

if (!isset($_SESSION['usuario_id'])) {
    header("Location: ../../Public/login.html");
    exit();
}

$usuarioId = $_SESSION['usuario_id'];
$tipo = $_POST['tipo'];
$fechaInicio = date('Y-m-d');
$fechaFin = $tipo === 'Anual'
    ? date('Y-m-d', strtotime('+1 year'))
    : date('Y-m-d', strtotime('+1 month'));

// Registrar nueva suscripción
$sql = "INSERT INTO Suscripciones (UsuarioID, TipoSuscripcion, FechaInicio, FechaFin)
        VALUES (?, ?, ?, ?)";
$params = [$usuarioId, $tipo, $fechaInicio, $fechaFin];
sqlsrv_query($conn, $sql, $params);

// Activar premium otra vez
$sql2 = "UPDATE Usuarios SET EsPremium = 1 WHERE UsuarioID = ?";
sqlsrv_query($conn, $sql2, [$usuarioId]);

header("Location: ../editar_perfil.php");
exit();
?>
