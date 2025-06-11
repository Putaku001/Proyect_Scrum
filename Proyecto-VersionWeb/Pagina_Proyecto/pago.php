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
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap" rel="stylesheet">
    <style>
        :root {
            --accent: #00d4ff;
            --accent2: #8a2be2;
            --bg-main: #1e1e2f;
            --bg-card: #2c2f4a;
            --bg-input: #3b3e5c;
            --text-main: #f0f0f0;
            --text-label: #b9b9e0;
            --danger: #ef4444;
        }
        [data-theme="light"] {
            --bg-main: #f4f6fc;
            --bg-card: #fff;
            --bg-input: #f0f0ff;
            --text-main: #23233a;
            --text-label: #555c7b;
        }
        html, body {
            min-height: 100vh;
            margin: 0;
            padding: 0;
        }
        body {
            background: linear-gradient(120deg, var(--bg-main) 60%, var(--accent2) 300%);
            font-family: 'Inter', 'Segoe UI', Arial, sans-serif;
            color: var(--text-main);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }
        .payment-box {
            background: var(--bg-card);
            padding: 32px 34px 26px 34px;
            border-radius: 20px;
            box-shadow: 0 8px 36px rgba(30,30,50,0.16), 0 2px 8px rgba(0,0,0,0.07);
            max-width: 410px;
            width: 100%;
            margin: 28px 0;
            display: flex;
            flex-direction: column;
            gap: 12px;
            animation: fadeInUp .5s;
        }
        @keyframes fadeInUp {
            from { transform: translateY(40px); opacity: 0;}
            to { transform: none; opacity: 1;}
        }
        .payment-box h2 {
            text-align: center;
            color: var(--accent);
            margin-bottom: 22px;
            font-size: 1.4rem;
            letter-spacing: 0.5px;
            font-weight: bold;
        }
        .payment-box label {
            margin-top: 10px;
            margin-bottom: 3px;
            font-size: 1em;
            font-weight: 600;
            color: var(--text-label);
            letter-spacing: 0.1px;
        }
        .payment-box input {
            width: 100%;
            padding: 12px 11px;
            margin-bottom: 2px;
            border: none;
            border-radius: 10px;
            background: var(--bg-input);
            color: var(--text-main);
            font-size: 1em;
            font-family: inherit;
            box-shadow: 0 1px 4px rgba(138,43,226,0.04);
            transition: box-shadow 0.18s, outline 0.18s;
        }
        .payment-box input:focus {
            outline: 2px solid var(--accent);
            box-shadow: 0 2px 10px rgba(0,212,255,0.09);
            background: #1e90ff11;
        }
        .payment-box button {
            margin-top: 22px;
            width: 100%;
            padding: 13px;
            background: linear-gradient(92deg, var(--accent) 55%, var(--accent2) 100%);
            color: #fff;
            font-weight: bold;
            border: none;
            border-radius: 10px;
            font-size: 1.12em;
            letter-spacing: 1px;
            cursor: pointer;
            box-shadow: 0 2px 8px rgba(0,212,255,0.08);
            transition: background 0.2s, filter 0.18s;
        }
        .payment-box button:hover {
            filter: brightness(0.94) saturate(1.4);
            background: linear-gradient(92deg, #00aacc 45%, #8a2be2 100%);
        }

        /* Mensaje de error personalizado (por si quieres mostrar en futuro) */
        .error-msg {
            background: var(--danger);
            color: #fff;
            padding: 10px 14px;
            border-radius: 7px;
            margin-bottom: 16px;
            font-weight: 600;
            text-align: center;
        }

        @media (max-width: 540px) {
            .payment-box {
                padding: 16px 4vw 18px 4vw;
                max-width: 99vw;
                border-radius: 10px;
            }
            .payment-box h2 {
                font-size: 1.09rem;
                margin-bottom: 13px;
            }
            .payment-box button { font-size: 0.98em;}
        }
        @media (max-width: 380px) {
            .payment-box { padding: 8px 2vw 10px 2vw;}
        }
    </style>
</head>
<body>
    <form class="payment-box" action="procesar_pago.php" method="POST" onsubmit="return validarFormulario();">
        <h2><?= $tipoPago ?> - <?= ucfirst($plan) ?> ($<?= $precio ?>)</h2>
        <input type="hidden" name="plan" value="<?= htmlspecialchars($plan) ?>">

        <label for="card_name">Nombre en la Tarjeta</label>
        <input type="text" name="card_name" id="card_name" required placeholder="Tu Nombre" autocomplete="cc-name">

        <label for="card_number">Número de Tarjeta</label>
        <input type="text" name="card_number" id="card_number" required 
               placeholder="1234 5678 9012 3456" maxlength="19" inputmode="numeric" autocomplete="cc-number">

        <label for="expiry_date">Fecha de Expiración (MM/AA)</label>
        <input type="text" name="expiry_date" id="expiry_date" required 
               placeholder="MM/AA" maxlength="5" inputmode="numeric" autocomplete="cc-exp">

        <label for="cvv">CVV</label>
        <input type="number" name="cvv" id="cvv" required placeholder="123" maxlength="3" autocomplete="cc-csc"
            oninput="this.value=this.value.replace(/[^0-9]/g,'').slice(0,3);">

        <button type="submit">Pagar $<?= $precio ?></button>
    </form>

    <script>
        // NÚMERO DE TARJETA: solo dígitos, espacio cada 4 dígitos, 16 dígitos exactos (19 con espacios)
        const cardNumberInput = document.getElementById('card_number');
        cardNumberInput.addEventListener('input', function(e) {
            let value = this.value.replace(/\D/g, '');
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
            let value = this.value.replace(/\D/g, '');
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
