<?php
session_start();
include("../Config/db.php");

if (!isset($_SESSION['usuario_id'])) {
  header("Location: ../Public/login.html");
  exit();
}

$id = $_SESSION['usuario_id'];
$rol = $_SESSION['rol'] ?? 1; // 1 = usuario, 2 = admin
$backPage = ($rol == 2) ? './Admin/admin_dashboard.php' : './Client/dashboard.php';

$sql = "SELECT * FROM Usuarios WHERE UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, [$id]);
$user = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

function avatarToDataUri(?string $bin)
{
  return $bin ? ('data:image/png;base64,' . base64_encode($bin)) : '../assets/imgs/default.png';
}

// --- Cambia aquí: consulta SIEMPRE la última suscripción (aunque esté vencida) ---
$sqlSub = "SELECT TOP 1 * FROM Suscripciones WHERE UsuarioID = ? ORDER BY FechaFin DESC";
$stmtSub = sqlsrv_query($conn, $sqlSub, [$id]);
$rowSub = sqlsrv_fetch_array($stmtSub, SQLSRV_FETCH_ASSOC);

$fechaFinText = '';
$subsVencida = true;    // Por defecto: vencida
$subsCancelada = false;
$tieneSuscripcion = false;

if ($rowSub && isset($rowSub['FechaFin'])) {
  $tieneSuscripcion = true;
  $fechaFin = $rowSub['FechaFin'];
  $fechaFinText = ($fechaFin instanceof DateTime) ? $fechaFin->format('d/m/Y') : date('d/m/Y', strtotime($fechaFin));
  $hoy = new DateTime();
  $subsVencida = ($fechaFin instanceof DateTime ? $fechaFin : new DateTime($fechaFin)) < $hoy;
  $subsCancelada = isset($rowSub['Cancelada']) && $rowSub['Cancelada'];
}
?>

<!DOCTYPE html>
<html lang="es">

<head>
  <meta charset="UTF-8">
  <title>Editar Perfil</title>
  <link rel="stylesheet" href="../assets/css/style.css">
  <style>
    :root {
      --primary: #00d4ff;
      --primary-hover: #00aacc;
      --bg: #1e1e2f;
      --panel: #2c2f4a;
      --input-bg: #3b3e5c;
      --header-bg: #23233a;
      --text-main: #f0f0f0;
      --text-secondary: #a0a0b0;
      --accent: #8a2be2;
    }
    [data-theme="light"] {
      --primary: #00d4ff;
      --primary-hover: #06b6d4;
      --bg: #f3f5ff;
      --panel: #fff;
      --input-bg: #eaeafd;
      --header-bg: #f4f6fc;
      --text-main: #2a2a3a;
      --text-secondary: #555c7b;
      --accent: #8a2be2;
    }
    body {
      background: var(--bg);
      font-family: 'Inter', 'Segoe UI', Arial, sans-serif;
      color: var(--text-main);
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      align-items: center;
      margin: 0;
      padding: 0;
    }
    header {
      background: var(--header-bg);
      width: 100vw;
      min-height: 64px;
      box-shadow: 0 2px 12px rgba(30,30,50,0.10);
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 14px 6vw;
      position: sticky; top: 0; z-index: 22;
    }
    .logo {
      display: flex; align-items: center; gap: 12px;
    }
    .logo img { height: 38px; border-radius: 8px; }
    .logo span { font-size: 1.2rem; font-weight: bold; color: var(--accent);}
    nav ul {
      list-style: none;
      display: flex; gap: 24px;
      margin: 0; padding: 0;
    }
    nav a {
      color: var(--text-secondary);
      text-decoration: none;
      font-weight: 600;
      font-size: 1rem;
      padding: 6px 12px;
      border-radius: 7px;
      transition: background .18s, color .18s;
    }
    nav a:hover { background: rgba(138,43,226,0.09); color: var(--accent);}
    .theme-switcher button {
      background: none;
      border: none;
      font-size: 1.24rem;
      color: var(--text-main);
      cursor: pointer;
      margin-left: 15px;
    }
    .profile-name {
      color: var(--accent);
      font-weight: 700;
      font-size: 1.02rem;
      margin-left: 18px;
      max-width: 190px;
      white-space: normal;
      word-break: break-word;
      overflow-wrap: anywhere;
      text-align: right;
    }
    /* ---- FORM PERFIL ---- */
    .main-wrap {
      flex: 1 1 auto;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      width: 100vw;
      min-height: calc(100vh - 64px - 60px);
    }
    .profile-box {
      background: var(--panel);
      padding: 32px 28px 24px 28px;
      border-radius: 14px;
      width: 100%;
      max-width: 470px;
      box-shadow: 0 6px 24px rgba(0,0,0,.13);
      margin-top: 40px;
      margin-bottom: 30px;
      display: flex;
      flex-direction: column;
      align-items: center;
    }
    .profile-box h2 {
      text-align: center;
      color: var(--primary);
      margin-bottom: 16px;
      font-size: 1.5rem;
      font-weight: bold;
      letter-spacing: 1px;
    }
    .profile-box label {
      display: block;
      margin-top: 13px;
      font-weight: bold;
      color: var(--text-secondary);
      font-size: 1rem;
    }
    .profile-box input[type="text"], .profile-box input[type="email"], .profile-box input[type="password"] {
      width: 100%;
      padding: 12px;
      margin-top: 6px;
      border: none;
      border-radius: 8px;
      background: var(--input-bg);
      color: var(--text-main);
      font-size: 1em;
    }
    .profile-box input[type="text"]:focus, .profile-box input[type="email"]:focus, .profile-box input[type="password"]:focus {
      outline: 2px solid var(--primary);
      background: #23234622;
    }
    .guardar-btn {
      background: var(--primary);
      color: #222;
      font-weight: bold;
      border: none;
      border-radius: 8px;
      padding: 13px 0;
      cursor: pointer;
      font-size: 1em;
      width: 100%;
      margin-top: 22px;
      transition: background .2s, color .2s;
      box-shadow: 0 1px 6px rgba(0,212,255,0.12);
      letter-spacing: 1px;
    }
    .guardar-btn:hover { background: var(--primary-hover);}
    .salir-btn {
      background: var(--input-bg);
      color: var(--text-main);
      border: 1px solid #666;
      margin-top: 10px;
    }
    .salir-btn:hover { background: #444;}
    .avatar-preview {
      display: block;
      margin: 0 auto 16px auto;
      border-radius: 50%;
      width: 90px; height: 90px;
      object-fit: cover;
      cursor: pointer;
      border: 3px solid var(--primary);
      background: #18182a;
    }
    .badge-box {
      margin-top: 20px;
      text-align: center;
    }
    .badge {
      display: inline-block;
      padding: 7px 18px;
      border-radius: 22px;
      font-weight: bold;
      font-size: .98em;
      margin-bottom: 5px;
      background: #00d4ff;
      color: #000;
    }
    .badge.no {
      background: #ff7676;
      color: #fff;
    }
    /* MODAL AVATAR */
    #avatarModal {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, .79);
      display: none;
      justify-content: center;
      align-items: center;
      z-index: 1000;
    }
    #avatarModal.show { display: flex; }
    .modal-content {
      background: var(--panel);
      width: 90%;
      max-width: 600px;
      max-height: 92vh;
      border-radius: 10px;
      padding: 22px 22px 18px 22px;
      overflow-y: auto;
      animation: fadeIn .2s;
      color: var(--text-main);
    }
    @keyframes fadeIn {
      from { transform: scale(0.95); opacity: 0;}
      to { transform: scale(1); opacity: 1;}
    }
    .modal-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, 70px);
      gap: 14px;
      justify-content: center;
      margin: 18px 0;
    }
    .modal-grid img {
      width: 70px; height: 70px;
      border-radius: 50%;
      object-fit: cover;
      cursor: pointer;
      border: 2.5px solid transparent;
      transition: transform .18s, border .18s;
      background: #18182a;
    }
    .modal-grid img:hover, .modal-grid img.selected { border: 2.5px solid var(--primary);}
    .modal-actions {
      margin-top: 10px;
      text-align: center;
      display: flex;
      justify-content: center;
      gap: 15px;
    }
    .btn {
      padding: 10px 23px;
      border-radius: 8px;
      font-weight: bold;
      border: none;
      cursor: pointer;
      font-size: 1rem;
    }
    .btn.cancel {
      background: none;
      color: #f66;
      border: 1.3px solid #f66;
    }
    .btn.save {
      background: var(--primary);
      color: #000;
    }
    @media (max-width: 600px) {
      header { flex-direction: column; gap: 5px; padding: 10px 2vw;}
      .logo img { height: 25px;}
      .logo span { font-size: 0.92rem;}
      nav ul { gap: 10px;}
      .main-wrap { min-height: 0;}
      .profile-box { max-width: 98vw; margin-top: 10px; padding: 12px 6px 14px 6px;}
      .profile-box h2 { font-size: 1.09rem; }
      .badge { font-size: 0.88em; padding: 5px 12px;}
      .modal-content { max-width: 96vw; padding: 7px 2px 6px 2px;}
      .modal-grid { grid-template-columns: repeat(auto-fill, 55px); gap: 7px; }
      .modal-grid img { width: 55px; height: 55px;}
    }
  </style>
</head>

<body>

  <form class="profile-box" action="./actualizar_perfil.php" method="POST">
    <h2>Editar Perfil</h2>
    <?php if (isset($_GET['mensaje']) && $_GET['mensaje'] == 'cancelada'): ?>
      <p style="background: #ff5555; color: #fff; padding: 10px; border-radius: 8px; text-align:center;">
        Suscripción cancelada correctamente.
      </p>
    <?php endif; ?>


    <img src="<?= avatarToDataUri($user['Avatar']) ?>" id="avatarPreview" class="avatar-preview" title="Cambiar avatar">
    <input type="hidden" name="avatar_choice" id="avatarChoice">

    <label>Nombre de usuario</label>
    <input type="text" name="nombre" value="<?= htmlspecialchars($user['NombreUsuario']) ?>" required>

    <label>Correo electrónico</label>
    <input type="email" name="email" value="<?= htmlspecialchars($user['Email']) ?>" required>

    <label>Nueva contraseña <span style="font-size:.9em;color:#aaa">(opcional)</span></label>
    <input type="password" name="password" placeholder="Deja vacío si no deseas cambiarla">

    <?php if ($rol == 1): ?>
      <div class="badge-box">
        <?php if ($user['EsPremium'] && !$subsVencida): ?>
          <span class="badge yes">Premium ✅</span><br>
          <?php if ($fechaFinText): ?>
            <span style="font-size: .85em; color: #aaa;">
              Expira el: <?= $fechaFinText ?>
              <?php if ($subsCancelada): ?>
                <br><span style="color:#ff7676;">(Cancelada, mantienes premium hasta la fecha)</span>
              <?php endif; ?>
            </span><br>
          <?php endif; ?>

          <?php if (!$subsCancelada): ?>
            <!-- Solo permite cancelar si no está ya cancelada -->
            <button type="button" class="guardar-btn" onclick="cancelarSuscripcion()">
              Cancelar Suscripción
            </button>
          <?php endif; ?>

        <?php elseif ($tieneSuscripcion && $subsVencida): ?>
          <span class="badge no">Vencida</span><br>
          <span style="font-size: .85em; color: #aaa;">
            Tu suscripción venció el <?= $fechaFinText ?>
          </span><br>
          <button type="button" class="guardar-btn" onclick="window.location.href='./Client/suscripciones.php'">
            Renovar Suscripción
          </button>
        <?php else: ?>
          <span class="badge no">Gratis</span>
          <button type="button" class="guardar-btn" onclick="window.location.href='./Client/suscripciones.php'">
            Suscribirme
          </button>
        <?php endif; ?>
      </div>

    <?php endif; ?>

    <button type="submit" class="guardar-btn">Guardar Cambios</button>
    <button type="button" class="guardar-btn salir-btn" onclick="location.href='<?= $backPage ?>'">
      Cancelar / Salir
    </button>
  </form>


  <div id="avatarModal">
    <div class="modal-content">
      <h3>Selecciona tu avatar</h3>
      <div class="modal-grid">
        <?php
        $dir = __DIR__ . '/../assets/imgs/avatars';
        foreach (array_diff(scandir($dir), ['.', '..']) as $img) {
          echo '<img src="../assets/imgs/avatars/' . $img . '" data-file="' . $img . '">';
        }
        ?>
      </div>
      <div class="modal-actions">
        <button type="button" class="btn cancel" id="cancelBtn">Cancelar</button>
        <button type="button" class="btn save" id="saveBtn">Elegir</button>
      </div>
    </div>
  </div>

  <script>
    document.addEventListener('DOMContentLoaded', () => {
      const preview = document.getElementById('avatarPreview');
      const modal = document.getElementById('avatarModal');
      const choiceInp = document.getElementById('avatarChoice');
      const cancelBtn = document.getElementById('cancelBtn');
      const saveBtn = document.getElementById('saveBtn');
      const imgs = Array.from(modal.querySelectorAll('.modal-grid img'));
      let selectedImg = null;

      preview.addEventListener('click', () => modal.classList.add('show'));
      imgs.forEach(img => {
        img.addEventListener('click', () => {
          imgs.forEach(i => i.classList.remove('selected'));
          img.classList.add('selected');
          selectedImg = img;
        });
      });

      cancelBtn.onclick = () => modal.classList.remove('show');
      saveBtn.onclick = () => {
        if (!selectedImg) {
          alert('Selecciona un avatar');
          return;
        }
        preview.src = selectedImg.src;
        choiceInp.value = selectedImg.dataset.file;
        modal.classList.remove('show');
      };

      modal.addEventListener('click', e => {
        if (e.target === modal) modal.classList.remove('show');
      });
    });

    function cancelarSuscripcion() {
      if (confirm('¿Estás seguro de que deseas cancelar tu suscripción?')) {
        window.location.href = './Client/cancelar_suscripcion.php';
      }
    }
  </script>

  </script>
</body>

</html>
