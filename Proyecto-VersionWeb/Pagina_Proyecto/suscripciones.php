<?php
session_start();
include("db.php");

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

$id = $_SESSION['usuario_id'];
$sql = "SELECT EsPremium, (SELECT TOP 1 FechaFin FROM Suscripciones WHERE UsuarioID = ? ORDER BY FechaFin DESC) AS FechaFin FROM Usuarios u LEFT JOIN Suscripciones s ON u.UsuarioID = s.UsuarioID WHERE u.UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, [$id, $id]);
$user = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

if ($user['EsPremium']) {
    header("Location: editar_perfil.php");
    exit();
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
            margin-bottom: 20px;
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

    <h1>Elige tu plan de suscripción</h1>

    <div class="plans">

        <div class="plan-card">
            <div class="plan-title">Plan Mensual</div>
            <div class="plan-price">$5 / mes</div>
            <div class="plan-details">
                Disfruta del catálogo completo de Mangaverse y vive
                increíbles aventuras en el mundo del manga durante un mes.<br>
            </div>
            <form action="pago.php" method="GET">
                <input type="hidden" name="plan" value="mensual">
                <button type="submit">Elegir Plan Mensual</button>
            </form>
        </div>

        <div class="plan-card">
            <div class="plan-title">Plan Anual</div>
            <div class="plan-price">$50 / año</div>
            <div class="plan-details">
                Disfruta del catálogo completo de Mangaverse y vive
                increíbles aventuras en el mundo del manga durante un año.<br>
            </div>
            <form action="pago.php" method="GET">
                <input type="hidden" name="plan" value="anual">
                <button type="submit">Elegir Plan Anual</button>
            </form>
        </div>

    </div>

</body>

</html>