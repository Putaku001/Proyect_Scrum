<?php
session_start();
include("../../Config/db.php");

if (!isset($_SESSION['usuario_id'])) {
    header("Location: ../../Public/login.html");
    exit();
}

$id = $_SESSION['usuario_id'];

// Marcar la suscripción activa (vigente y no cancelada) como cancelada
$sql = "UPDATE Suscripciones
        SET Cancelada = 1
        WHERE UsuarioID = ? AND Cancelada = 0
        AND FechaFin = (
            SELECT MAX(FechaFin) FROM Suscripciones WHERE UsuarioID = ? AND Cancelada = 0
        )";
$stmt = sqlsrv_query($conn, $sql, [$id, $id]);

if ($stmt === false) {
    die(print_r(sqlsrv_errors(), true));
}

header("Location: ../editar_perfil.php?mensaje=cancelada");
exit();
