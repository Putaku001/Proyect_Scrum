<?php
session_start();
include("../Config/db.php");


if ($_SERVER["REQUEST_METHOD"] === "POST") {
    $email    = $_POST['email'];
    $password = $_POST['password'];
    $hash     = hash('sha256', $password);

    $sql    = "SELECT UsuarioID, NombreUsuario, RolID, Avatar
               FROM Usuarios
               WHERE Email = ? AND ContrasenaHash = ?";
    $params = [$email, $hash];
    $stmt   = sqlsrv_query($conn, $sql, $params);

    if ($stmt === false) {
        die(print_r(sqlsrv_errors(), true));
    }

    if (sqlsrv_has_rows($stmt)) {
        $u = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

        /* ── Sesión ─────────────────────────────────────── */
        $_SESSION['usuario_id'] = $u['UsuarioID'];
        $_SESSION['nombre']     = $u['NombreUsuario'];
        $_SESSION['rol']        = $u['RolID'];

        if (!empty($u['Avatar'])) {
            $_SESSION['avatar_bin'] = $u['Avatar'];     // BLOB → sesión
        } else {
            unset($_SESSION['avatar_bin']);             // se usará default.png
        }

        /* ── Redirección según rol ────────────────────── */
        header("Location: " . ($u['RolID'] == 2 ? '../Pages/Admin/admin_dashboard.php'
                                                : '../Pages/Client/dashboard.php'));
        exit();
    }

    echo "<script>alert('Correo o contraseña incorrectos.'); 
          window.location.href='../Public/login.html';</script>";
}

?>
