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

        <label>Nombre en la Tarjeta</label>
        <input type="text" name="card_name" required placeholder="Tu Nombre" autocomplete="cc-name">

        <label>Número de Tarjeta</label>
        <input type="text" name="card_number" id="card_number" required 
               placeholder="1234 5678 9012 3456" maxlength="19" inputmode="numeric" autocomplete="cc-number">

        <label>Fecha de Expiración (MM/AA)</label>
        <input type="text" name="expiry_date" id="expiry_date" required 
               placeholder="MM/AA" maxlength="5" inputmode="numeric" autocomplete="cc-exp">

        <label>CVV</label>
        <input type="number" name="cvv" id="cvv" required placeholder="123" maxlength="3" autocomplete="cc-csc" oninput="this.value=this.value.replace(/[^0-9]/g,'').slice(0,3);">

        <button type="submit">Pagar $<?= $precio ?></button>
    </form>

    <script>
        // NÚMERO DE TARJETA: solo dígitos, espacio cada 4 dígitos, 16 dígitos exactos (19 con espacios)
        const cardNumberInput = document.getElementById('card_number');
        cardNumberInput.addEventListener('input', function(e) {
            let value = this.value.replace(/\D/g, '');  // Solo números
            if (value.length > 16) value = value.slice(0,16);

            // Insertar espacios cada 4 dígitos
            let formatted = '';
            for (let i = 0; i < value.length; i += 4) {
                if (i > 0) formatted += ' ';
                formatted += value.substr(i, 4);
            }
            this.value = formatted;
        });

        // FECHA DE EXPIRACIÓN: Formato MM/AA y añade la pleca automáticamente
        const expiryInput = document.getElementById('expiry_date');
        expiryInput.addEventListener('input', function(e) {
            let value = this.value.replace(/\D/g, ''); // Solo números
            if (value.length > 4) value = value.slice(0,4);

            if (value.length > 2) {
                value = value.slice(0,2) + '/' + value.slice(2);
            }
            this.value = value;
        });

        // VALIDACIÓN AL ENVIAR
        function validarFormulario() {
            const card = document.getElementById('card_number').value.replace(/\s/g, '');
            const exp = document.getElementById('expiry_date').value.trim();
            const cvv = document.getElementById('cvv').value.trim();

            if (!/^\d{16}$/.test(card)) {
                alert('El número de tarjeta debe contener exactamente 16 dígitos numéricos.');
                return false;
            }

            if (!/^\d{2}\/\d{2}$/.test(exp)) {
                alert('La fecha debe estar en formato MM/AA');
                return false;
            }

            // Validar mes y que no esté vencida
            const [mm, yy] = exp.split('/').map(Number);
            if (mm < 1 || mm > 12) {
                alert('El mes debe estar entre 01 y 12.');
                return false;
            }
            const fechaActual = new Date();
            const fechaIngresada = new Date(2000 + yy, mm - 1, 1);
            if (
                fechaIngresada.getFullYear() < fechaActual.getFullYear() ||
                (fechaIngresada.getFullYear() === fechaActual.getFullYear() && fechaIngresada.getMonth() < fechaActual.getMonth())
            ) {
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
