<?php
session_start();
require '../Config/db.php';

if (!isset($_SESSION['usuario_id'])) {
    header("Location: ../Public/login.html");
    exit();
}

$uid = (int) $_SESSION['usuario_id'];
$rol = (int) ($_SESSION['rol'] ?? 1); // 1 = usuario, 2 = admin

$nombre = trim($_POST['nombre'] ?? '');
$email  = trim($_POST['email'] ?? '');
$pwd    = trim($_POST['password'] ?? '');
$avatar = trim($_POST['avatar_choice'] ?? '');

$set = [];
$val = [];

$set[] = 'NombreUsuario=?';  $val[] = $nombre;
$set[] = 'Email=?';          $val[] = $email;

if ($pwd !== '') {
    $set[] = 'ContrasenaHash=?';
    $val[] = hash('sha256', $pwd);
}

if ($avatar !== '') {
    $path = __DIR__ . '/../assets/imgs/avatars/' . basename($avatar);
    if (!file_exists($path)) die("Avatar no encontrado.");
    $bin = file_get_contents($path);
    $stream = fopen('php://memory', 'r+');
    fwrite($stream, $bin);
    rewind($stream);
    $set[] = 'Avatar=?';
    $val[] = [$stream, SQLSRV_PARAM_IN, SQLSRV_PHPTYPE_STREAM(SQLSRV_ENC_BINARY), SQLSRV_SQLTYPE_VARBINARY('MAX')];
}

$val[] = $uid;
$sql = "UPDATE Usuarios SET " . implode(', ', $set) . " WHERE UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, $val);

if (!$stmt) {
    echo "<pre>"; print_r(sqlsrv_errors()); echo "</pre>";
    exit();
}

// ✅ Actualizar nombre en la sesión
$_SESSION['nombre'] = $nombre;

// ✅ Redirección correcta según el rol
$destino = ($rol === 2) ? './Admin/admin_dashboard.php' : './Client/dashboard.php';
header("Location: $destino");
exit();
