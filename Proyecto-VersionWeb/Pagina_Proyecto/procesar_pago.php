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
    $dias = ($plan === 'mensual') ? 30 : 365;

    // Obtener la última fecha de suscripción
    $sql = "SELECT TOP 1 FechaFin FROM Suscripciones WHERE UsuarioID = ? ORDER BY FechaFin DESC";
    $stmt = sqlsrv_query($conn, $sql, [$idUsuario]);
    $ultima = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

    $hoy = new DateTime();
    if ($ultima && isset($ultima['FechaFin']) && $ultima['FechaFin'] instanceof DateTime && $ultima['FechaFin'] > $hoy) {
        // Suscripción activa → extender desde el final actual
        $fechaBase = $ultima['FechaFin'];
    } else {
        // Sin suscripción o ya vencida → iniciar desde hoy
        $fechaBase = $hoy;
    }

    $nuevaFechaFin = clone $fechaBase;
    $nuevaFechaFin->add(new DateInterval("P{$dias}D"));

    $insert = "INSERT INTO Suscripciones (UsuarioID, TipoSuscripcion, FechaFin) VALUES (?, ?, ?)";
    $params = [$idUsuario, ucfirst($plan), $nuevaFechaFin->format('Y-m-d')];
    $stmt = sqlsrv_query($conn, $insert, $params);

    if ($stmt) {
        sqlsrv_query($conn, "UPDATE Usuarios SET EsPremium = 1 WHERE UsuarioID = ?", [$idUsuario]);
        echo "<script>alert('¡Suscripción procesada correctamente!'); window.location.href='editar_perfil.php';</script>";
    } else {
        $err = print_r(sqlsrv_errors(), true);
        echo "<script>console.error(" . json_encode($err) . "); alert('Error al guardar la suscripción.'); window.location.href='suscripciones.php';</script>";
    }
}
