<?php
session_start();
include("db.php");

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

$id = $_SESSION['usuario_id'];
// Consultamos si tiene o tuvo alguna suscripción
$sql = "SELECT EsPremium, 
        (SELECT TOP 1 FechaFin FROM Suscripciones WHERE UsuarioID = ? ORDER BY FechaFin DESC) AS FechaFin,
        (SELECT COUNT(*) FROM Suscripciones WHERE UsuarioID = ?) AS TotalSuscripciones
        FROM Usuarios WHERE UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, [$id, $id, $id]);
$user = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

$fechaFin = $user['FechaFin'] ?? null;
$esPremium = $user['EsPremium'] ?? 0;
$totalSuscripciones = $user['TotalSuscripciones'] ?? 0;
$hoy = new DateTime();
$estado = "Sin suscripción activa";
$vencida = true;

// ¿Alguna vez ha tenido suscripción?
$esNuevo = ($totalSuscripciones == 0);

if ($esPremium == 1) {
    if ($fechaFin instanceof DateTime) {
        if ($fechaFin > $hoy) {
            $estado = "Suscripción activa hasta el " . $fechaFin->format('d/m/Y');
            $vencida = false;
        } else {
            $estado = "Tu suscripción venció el " . $fechaFin->format('d/m/Y');
        }
    } else {
        $estado = "Suscripción activa";
        $vencida = false;
    }
} else {
    $estado = "Sin suscripción activa";
    $vencida = true;
}

?>
<!DOCTYPE html>
<html lang="es">

<head>
    <meta charset="UTF-8">
    <title>Planes de Suscripción</title>
    <link rel="stylesheet" href="./css/style.css">
    <style>
        body {
            background: #1e1e2f;
            font-family: 'Segoe UI', sans-serif;
            color: #f0f0f0;
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 40px 20px;
        }
        h1 {
            color: #00d4ff;
            margin-bottom: 10px;
        }
        .estado-sub {
            font-size: 1em;
            margin-bottom: 30px;
            padding: 12px 20px;
            border-radius: 8px;
            background: #3b3e5c;
            color: #f0f0f0;
            text-align: center;
        }
        .estado-sub.vencida {
            background: #933;
        }
        .plans {
            display: flex;
            gap: 20px;
            flex-wrap: wrap;
            justify-content: center;
        }
        .plan-card {
            background: #2c2f4a;
            border-radius: 12px;
            padding: 25px;
            width: 280px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, .5);
            text-align: center;
            border: 2px solid transparent;
            transition: transform 0.3s, border 0.3s;
        }
        .plan-card:hover {
            transform: scale(1.05);
            border-color: #00d4ff;
        }
        .plan-title {
            font-size: 1.4em;
            margin-bottom: 10px;
            color: #00d4ff;
        }
        .plan-price {
            font-size: 2em;
            margin-bottom: 15px;
            font-weight: bold;
        }
        .plan-details {
            font-size: .9em;
            color: #ccc;
            margin-bottom: 20px;
        }
        .plan-card button {
            background: #00d4ff;
            color: #000;
            font-weight: bold;
            border: none;
            border-radius: 8px;
            padding: 12px 20px;
            cursor: pointer;
            font-size: 1em;
        }
        .plan-card button:hover {
            background: #00aacc;
        }
    </style>
</head>

<body>
    <h1>
        <?php
            if ($esNuevo) {
                echo 'Suscribirse';
            } else if ($esPremium && !$vencida) {
                echo 'Cambiar o ampliar tu suscripción';
            } else {
                echo 'Renovar Suscripción';
            }
        ?>
    </h1>

    <div class="estado-sub <?= $vencida ? 'vencida' : '' ?>">
        <?= $estado ?>
    </div>

    <div class="plans">
        <div class="plan-card">
            <div class="plan-title">Plan Mensual</div>
            <div class="plan-price">$5 / mes</div>
            <div class="plan-details">
                Accede al catálogo premium durante 30 días.
            </div>
            <form action="pago.php" method="GET">
                <input type="hidden" name="plan" value="mensual">
                <button type="submit">
                    <?= $esNuevo ? 'Suscribirse' : ($vencida ? 'Renovar' : 'Elegir') ?> Mensual
                </button>
            </form>
        </div>

        <div class="plan-card">
            <div class="plan-title">Plan Anual</div>
            <div class="plan-price">$50 / año</div>
            <div class="plan-details">
                Accede al catálogo premium durante 365 días.
            </div>
            <form action="pago.php" method="GET">
                <input type="hidden" name="plan" value="anual">
                <button type="submit">
                    <?= $esNuevo ? 'Suscribirse' : ($vencida ? 'Renovar' : 'Elegir') ?> Anual
                </button>
            </form>
        </div>
    </div>
</body>

</html>
