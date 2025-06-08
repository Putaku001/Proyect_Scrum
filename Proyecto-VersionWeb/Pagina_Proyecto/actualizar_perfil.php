<?php
session_start();
include("db.php");

/* ── Solo usuarios autenticados ── */
if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

/* ───────────────────────────────────────────────────────────
   1. Datos del formulario (premium ya NO viene en el POST)
   ─────────────────────────────────────────────────────────── */
$id       = (int) $_POST['id'];                         // hidden input
$nombre   = trim($_POST['nombre']);
$email    = trim($_POST['email']);
$pwd      = trim($_POST['password']);
$avatarFn = trim($_POST['avatar_choice']);              // nombre de archivo o ''

/* ───────────────────────────────────────────────────────────
   2. Construir lista de campos a actualizar
   ─────────────────────────────────────────────────────────── */
$fields = "NombreUsuario=?, Email=?";
$params = [$nombre, $email];

/* (a) Contraseña (opcional) */
if ($pwd !== '') {
    $hash     = hash('sha256', $pwd);
    $fields  .= ", ContrasenaHash=?";
    $params[] = $hash;
}

/* (b) Avatar (opcional) */
if ($avatarFn !== '') {
    $path = __DIR__ . "/imgs/avatars/" . basename($avatarFn);
    if (!file_exists($path)) { die("Avatar no encontrado."); }

    $bin   = file_get_contents($path);
    $stream = fopen('php://memory', 'r+');
    fwrite($stream, $bin);
    rewind($stream);

    $fields  .= ", Avatar=?";
    $params[] = [
        &$stream,
        SQLSRV_PARAM_IN,
        SQLSRV_PHPTYPE_STREAM(SQLSRV_ENC_BINARY)
    ];

    /*  Actualizar la sesión para reflejar el cambio en el dashboard  */
    $_SESSION['avatar_bin'] = $bin;
}

/*  id para el WHERE  */
$params[] = $id;

/* ───────────────────────────────────────────────────────────
   3. Ejecutar UPDATE
   ─────────────────────────────────────────────────────────── */
$sql  = "UPDATE Usuarios SET $fields WHERE UsuarioID=?";
$stmt = sqlsrv_prepare($conn, $sql, $params);

if (!$stmt) {
    die("Error prepare: " . print_r(sqlsrv_errors(), true));
}

if (!sqlsrv_execute($stmt)) {
    die("Error al actualizar perfil: " . print_r(sqlsrv_errors(), true));
}

/* ───────────────────────────────────────────────────────────
   4. Redirigir al dashboard correcto
   ─────────────────────────────────────────────────────────── */
$rolId = $_SESSION['rol'] ?? 1;                         // 1=Usuario, 2=Admin
$dest  = ($rolId == 2) ? 'admin_dashboard.php' : 'dashboard.php';

header("Location: {$dest}?msg=Perfil+actualizado");
exit();
?>
