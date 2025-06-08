<?php
include("db.php");

$id = $_POST['id'];
$nombre = $_POST['nombre'];
$email = $_POST['email'];
$rol = $_POST['rol'];
$premium = isset($_POST['premium']) ? 1 : 0;

$sql = "UPDATE Usuarios SET NombreUsuario=?, Email=?, RolID=?, EsPremium=? WHERE UsuarioID=?";
$params = [$nombre, $email, $rol, $premium, $id];
sqlsrv_query($conn, $sql, $params);

header("Location: admin_panel.php");
?>
