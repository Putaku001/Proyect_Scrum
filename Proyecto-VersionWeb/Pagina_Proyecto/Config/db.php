<?php
$serverName = "localhost";
$connectionOptions = [
    "Database" => "proyectoDBS2",
    "Uid" => "usuarioSQL",    // ← reemplaza con tu usuario
    "PWD" => "Sijilo75", // ← reemplaza con tu contraseña
    "CharacterSet" => "UTF-8"
];

$conn = sqlsrv_connect($serverName, $connectionOptions);

if (!$conn) {
    die(print_r(sqlsrv_errors(), true));
}
