<?php
session_start();
include("db.php");

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $plan = $_POST['plan'] ?? '';
    if (!in_array($plan, ['mensual', 'anual'])) {
        header("Location: suscripciones.php");
        exit();
    }

    $idUsuario = $_SESSION['usuario_id'];
    $dias = ($plan == 'mensual') ? 30 : 365;

    $fechaFin = date('Y-m-d', strtotime("+$dias days"));

    $sql = "INSERT INTO Suscripciones (UsuarioID, TipoSuscripcion, FechaFin) VALUES (?, ?, ?)";
    $params = [$idUsuario, ucfirst($plan), $fechaFin];
    $stmt = sqlsrv_query($conn, $sql, $params);

    if ($stmt) {
        $update = "UPDATE Usuarios SET EsPremium = 1 WHERE UsuarioID = ?";
        sqlsrv_query($conn, $update, [$idUsuario]);

        echo "<script>alert('¡Pago exitoso! Ahora eres usuario Premium 🎉'); window.location.href='editar_perfil.php';</script>";
    } else {
        $errors = print_r(sqlsrv_errors(), true);
        echo "<script>console.error(" . json_encode($errors) . "); alert('Error al procesar el pago.'); window.location.href='suscripciones.php';</script>";
    }
}
