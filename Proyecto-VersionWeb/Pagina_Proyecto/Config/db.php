<?php
$serverName = "localhost";
$connectionOptions = [
    "Database" => "proyectoDBS2",
    "Uid" => "Shadow01xd",    // ← reemplaza con tu usuario
    "PWD" => "PraiseTheFool", // ← reemplaza con tu contraseña
    "CharacterSet" => "UTF-8"
];

$conn = sqlsrv_connect($serverName, $connectionOptions);

if (!$conn) {
    die(print_r(sqlsrv_errors(), true));
}
