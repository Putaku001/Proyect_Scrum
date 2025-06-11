<?php
include("../../Config/db.php");

$id = $_GET['id'];
$sql = "DELETE FROM Usuarios WHERE UsuarioID = ?";
sqlsrv_query($conn, $sql, [$id]);

header("Location: admin_panel.php");
?>
