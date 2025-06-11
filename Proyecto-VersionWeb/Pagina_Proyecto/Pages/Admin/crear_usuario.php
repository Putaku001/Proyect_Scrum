<?php
include("../../Config/db.php");

$nombre = $_POST['nombre'];
$email = $_POST['email'];
$pass = $_POST['password'];
$rol = $_POST['rol'];

$hash = hash('sha256', $pass);

$sql = "INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, RolID) VALUES (?, ?, ?, ?)";
$params = [$nombre, $email, $hash, $rol];

$stmt = sqlsrv_query($conn, $sql, $params);
header("Location: admin_panel.php");
?>
