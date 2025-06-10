<?php
$serverName = "localhost\\SQLEXPRESS";
$connectionOptions = [
    "Database" => "proyectoDBS3",
    "Uid" => "kenn",    // ← reemplaza con tu usuario
    "PWD" => "123", // ← reemplaza con tu contraseña
    "CharacterSet" => "UTF-8"
];

$conn = sqlsrv_connect($serverName, $connectionOptions);

if (!$conn) {
    die(print_r(sqlsrv_errors(), true));
}
