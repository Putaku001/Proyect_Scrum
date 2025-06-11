<?php
session_start();
require 'db.php';

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

$uid = $_SESSION['usuario_id'];

// Marcar como cancelada la última suscripción activa no cancelada ni vencida
$sql = "UPDATE Suscripciones 
        SET Cancelada = 1 
        WHERE SuscripcionID = (
            SELECT TOP 1 SuscripcionID FROM Suscripciones 
            WHERE UsuarioID = ? AND (Cancelada IS NULL OR Cancelada = 0) AND FechaFin >= GETDATE()
            ORDER BY FechaFin DESC
        )";
sqlsrv_query($conn, $sql, [$uid]);

header("Location: editar_perfil.php?mensaje=cancelada");
exit();
?>
