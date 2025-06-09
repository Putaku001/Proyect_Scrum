<?php
session_start();
include("db.php");

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

$plan = $_GET['plan'] ?? '';
if (!in_array($plan, ['mensual', 'anual'])) {
    header("Location: suscripciones.php");
    exit();
}

$idUsuario = $_SESSION['usuario_id'];
$sql = "SELECT TOP 1 FechaFin FROM Suscripciones WHERE UsuarioID = ? ORDER BY FechaFin DESC";
$stmt = sqlsrv_query($conn, $sql, [$idUsuario]);
$ultima = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

$hoy = new DateTime();
$tipoPago = "Nueva suscripción";
if ($ultima && isset($ultima['FechaFin']) && $ultima['FechaFin'] instanceof DateTime && $ultima['FechaFin'] > $hoy) {
    $tipoPago = "Renovación de suscripción";
}

$precio = $plan === 'mensual' ? 5 : 50;
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title><?= $tipoPago ?> - <?= ucfirst($plan) ?></title>
    <link rel="stylesheet" href="./css/style.css">
    <style>
        body {
            background: #1e1e2f;
            font-family: 'Segoe UI', sans-serif;
            color: #f0f0f0;
            display: flex;
            justify-content: center;
            padding: 40px 20px;
        }
        .payment-box {
            background: #2c2f4a;
            padding: 30px;
            border-radius: 12px;
            max-width: 500px;
            width: 100%;
            box-shadow: 0 4px 12px rgba(0, 0, 0, .5);
        }
        .payment-box h2 {
            text-align: center;
            color: #00d4ff;
            margin-bottom: 20px;
        }
        .payment-box label {
            display: block;
            margin-top: 15px;
            font-weight: bold;
            color: #ccc;
        }
        .payment-box input {
            width: 100%;
            padding: 12px;
            margin-top: 8px;
            border: none;
            border-radius: 8px;
            background: #3b3e5c;
            color: #f0f0f0;
        }
        .payment-box button {
            width: 100%;
            margin-top: 25px;
            padding: 12px;
            background: #00d4ff;
            color: #000;
            font-weight: bold;
            border: none;
            border-radius: 8px;
            cursor: pointer;
        }
        .payment-box button:hover {
            background: #00aacc;
        }
    </style>
</head>
<body>
    <form class="payment-box" action="procesar_pago.php" method="POST" onsubmit="return validarFormulario();">
        <h2><?= $tipoPago ?> - <?= ucfirst($plan) ?> ($<?= $precio ?>)</h2>
        <input type="hidden" name="plan" value="<?= htmlspecialchars($plan) ?>">

        <label>Número de Tarjeta</label>
        <input type="text" name="card_number" id="card_number" required placeholder="1234 5678 9012 3456">

        <label>Nombre en la Tarjeta</label>
        <input type="text" name="card_name" required placeholder="Tu Nombre">

        <label>Fecha de Expiración (MM/AA)</label>
        <input type="text" name="expiry_date" id="expiry_date" required placeholder="MM/AA">

        <label>CVV</label>
        <input type="number" name="cvv" id="cvv" required placeholder="123" maxlength="3">

        <button type="submit">Pagar $<?= $precio ?></button>
    </form>

    <script>
        function validarFormulario() {
            const exp = document.getElementById('expiry_date').value.trim();
            const cvv = document.getElementById('cvv').value.trim();

            if (!/^\d{2}\/\d{2}$/.test(exp)) {
                alert('La fecha debe estar en formato MM/AA');
                return false;
            }

            const [mm, yy] = exp.split('/').map(Number);
            const fechaActual = new Date();
            const fechaIngresada = new Date(2000 + yy, mm - 1);

            if (fechaIngresada < fechaActual) {
                alert('La tarjeta está vencida.');
                return false;
            }

            if (!/^\d{3}$/.test(cvv)) {
                alert('El CVV debe tener exactamente 3 dígitos.');
                return false;
            }

            return true;
        }
    </script>
</body>
</html>
