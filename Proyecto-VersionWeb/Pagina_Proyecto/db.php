<?php
$serverName = "localhost\\SQLEXPRESS";
$connectionOptions = [
    "Database" => "proyectoDBS3",
    "Uid" => "ale",    // ← reemplaza con tu usuario
    "PWD" => "sonic", // ← reemplaza con tu contraseña
    "CharacterSet" => "UTF-8"
];

$conn = sqlsrv_connect($serverName, $connectionOptions);

if (!$conn) {
    die(print_r(sqlsrv_errors(), true));
}
