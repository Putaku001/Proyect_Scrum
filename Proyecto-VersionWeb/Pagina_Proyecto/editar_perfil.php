<?php
session_start();
include("db.php");

if (!isset($_SESSION['usuario_id'])) {
  header("Location: login.html");
  exit();
}

$id  = $_SESSION['usuario_id'];
$rol = $_SESSION['rol'] ?? 1; // 1 = usuario, 2 = admin
$backPage = ($rol == 2) ? 'admin_dashboard.php' : 'dashboard.php';

$sql  = "SELECT * FROM Usuarios WHERE UsuarioID = ?";
$stmt = sqlsrv_query($conn, $sql, [$id]);
$user = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);

function avatarToDataUri(?string $bin)
{
  return $bin ? ('data:image/png;base64,' . base64_encode($bin)) : './imgs/default.png';
}

$fechaFinText = '';
$subsVencida = false;
if ($user['EsPremium']) {
  $sqlFechaFin = "SELECT TOP 1 FechaFin FROM Suscripciones WHERE UsuarioID = ? ORDER BY FechaFin DESC";
  $stmtFechaFin = sqlsrv_query($conn, $sqlFechaFin, [$id]);
  $rowFechaFin  = sqlsrv_fetch_array($stmtFechaFin, SQLSRV_FETCH_ASSOC);
  if ($rowFechaFin && $rowFechaFin['FechaFin'] instanceof DateTime) {
    $fechaFin = $rowFechaFin['FechaFin'];
    $hoy = new DateTime();
    $fechaFinText = $fechaFin->format('d/m/Y');
    $subsVencida = $fechaFin < $hoy;
  }
}
?>
<!DOCTYPE html>
<html lang="es">

<head>
  <meta charset="UTF-8">
  <title>Editar Perfil</title>
  <link rel="stylesheet" href="./css/style.css">
  <style>
    :root {
      --primary: #00d4ff;
      --bg: #1e1e2f;
      --panel: #2c2f4a;
    }

    body {
      background: var(--bg);
      font-family: 'Segoe UI', sans-serif;
      color: #f0f0f0;
      display: flex;
      justify-content: center;
      padding: 40px 20px;
    }

    .profile-box {
      background: var(--panel);
      padding: 30px;
      border-radius: 12px;
      max-width: 500px;
      width: 100%;
      box-shadow: 0 4px 12px rgba(0, 0, 0, .5);
    }

    .profile-box h2 {
      text-align: center;
      color: var(--primary);
      margin-bottom: 20px;
    }

    .profile-box label {
      display: block;
      margin-top: 15px;
      font-weight: bold;
      color: #ccc;
    }

    .profile-box input {
      width: 100%;
      padding: 12px;
      margin-top: 8px;
      border: none;
      border-radius: 8px;
      background: #3b3e5c;
      color: #f0f0f0;
    }

    .guardar-btn {
      background-color: var(--primary);
      color: #000;
      font-weight: bold;
      border: none;
      border-radius: 8px;
      padding: 12px;
      cursor: pointer;
      font-size: 1em;
      width: 100%;
      margin-top: 20px;
      transition: background .3s;
    }

    .guardar-btn:hover {
      background-color: #00aacc;
    }

    .salir-btn {
      background: #3b3e5c;
      color: #fff;
      border: 1px solid #666;
    }

    .salir-btn:hover {
      background: #555;
    }

    .avatar-preview {
      display: block;
      margin: 0 auto 20px;
      border-radius: 50%;
      width: 100px;
      height: 100px;
      object-fit: cover;
      cursor: pointer;
    }

    .badge-box {
      margin-top: 20px;
      text-align: center;
    }

    .badge {
      display: inline-block;
      padding: 6px 16px;
      border-radius: 20px;
      font-weight: bold;
      font-size: .9em;
      margin-bottom: 5px;
    }

    .badge.yes {
      background: #00d4ff;
      color: #000;
    }

    .badge.no {
      background: #ff7676;
      color: #000;
    }

    #avatarModal {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, .8);
      display: none;
      justify-content: center;
      align-items: center;
      z-index: 1000;
    }

    #avatarModal.show {
      display: flex;
    }

    .modal-content {
      background: #2b2e45;
      width: 90%;
      max-width: 680px;
      max-height: 90vh;
      border-radius: 10px;
      padding: 30px;
      overflow-y: auto;
      animation: fadeIn .3s ease-out;
    }

    @keyframes fadeIn {
      from {
        transform: scale(0.95);
        opacity: 0;
      }

      to {
        transform: scale(1);
        opacity: 1;
      }
    }

    .modal-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, 80px);
      gap: 16px;
    }

    .modal-grid img {
      width: 80px;
      height: 80px;
      border-radius: 50%;
      object-fit: cover;
      cursor: pointer;
      transition: transform .2s, border .2s;
    }

    .modal-grid img:hover {
      transform: scale(1.08);
    }

    .modal-grid img.selected {
      border: 3px solid var(--primary);
    }

    .modal-actions {
      margin-top: 20px;
      text-align: center;
      display: flex;
      justify-content: center;
      gap: 15px;
    }

    .btn {
      padding: 10px 20px;
      border-radius: 8px;
      font-weight: bold;
      border: none;
      cursor: pointer;
    }

    .btn.cancel {
      background: none;
      color: #f66;
      border: 1px solid #f66;
    }

    .btn.save {
      background: var(--primary);
      color: #000;
    }
  </style>
</head>

<body>

  <form class="profile-box" action="actualizar_perfil.php" method="POST">
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
        <?php if ($user['EsPremium']): ?>
          <span class="badge yes">Premium ✅</span><br>
          <?php if ($fechaFinText): ?>
            <span style="font-size: .85em; color: #aaa;">Expira el: <?= $fechaFinText ?></span><br>
          <?php endif; ?>

          <!-- Botón para cancelar suscripción -->
          <button type="button" class="guardar-btn" onclick="cancelarSuscripcion()">
            Cancelar Suscripción
          </button>

        <?php else: ?>
          <span class="badge no">Gratis</span>
          <button type="button" class="guardar-btn" onclick="window.location.href='suscripciones.php'">
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
        $dir = __DIR__ . '/imgs/avatars';
        foreach (array_diff(scandir($dir), ['.', '..']) as $img) {
          echo '<img src="imgs/avatars/' . $img . '" data-file="' . $img . '">';
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
        window.location.href = 'cancelar_suscripcion.php';
      }
    }
  </script>

  </script>
</body>

</html>