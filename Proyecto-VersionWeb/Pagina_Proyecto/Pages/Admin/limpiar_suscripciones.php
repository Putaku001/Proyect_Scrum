<?php
// limpiar_suscripciones.php
include_once("../../Config/db.php");

// Elimina TODAS las suscripciones vencidas
$sql = "DELETE FROM Suscripciones WHERE FechaFin < CONVERT(date, GETDATE())";
sqlsrv_query($conn, $sql);

// Desactiva premium a los que no tienen ninguna activa
$update = "UPDATE Usuarios SET EsPremium = 0 
           WHERE UsuarioID NOT IN (
                SELECT UsuarioID FROM Suscripciones WHERE FechaFin >= CONVERT(date, GETDATE())
           )";
sqlsrv_query($conn, $update);
?>
