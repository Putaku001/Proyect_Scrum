<?php
/************************************************************************
 * google_drive.php
 *  - Lista archivos PDF de una carpeta en Drive.
 *──────────────────────────────────────────────────────────────────────*/
session_start();

/* ─── CONFIG ──────────────────────────────────────────────*/
const CLIENT_ID     = '719046572033-27o8382k35lnbvkeo2fn4j0hu7bfvev9.apps.googleusercontent.com';
const CLIENT_SECRET = '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';
const REDIRECT_URI  = 'http://localhost/Pagina_Proyecto/google_drive.php';
const SCOPE         = 'https://www.googleapis.com/auth/drive.readonly';
const TOKEN_FILE    = __DIR__ . '/token_guardado.json';

const FOLDER_ID     = '1yg7WSwfztQeGgGWybI1ngNSn2nIs494O';   // ejemplo
/*──────────────────────────────────────────────────────────*/

/* 1 ─ Cargar refresh-token si existe */
if (empty($_SESSION['refresh_token']) && is_file(TOKEN_FILE)) {
    $_SESSION['refresh_token'] =
        json_decode(file_get_contents(TOKEN_FILE), true)['refresh_token'] ?? null;
}

/* 2 ─ Renovar access-token con refresh-token */
if (!empty($_SESSION['refresh_token'])) {
    $ch = curl_init('https://oauth2.googleapis.com/token');
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_POSTFIELDS     => http_build_query([
            'client_id'     => CLIENT_ID,
            'client_secret' => CLIENT_SECRET,
            'refresh_token' => $_SESSION['refresh_token'],
            'grant_type'    => 'refresh_token'
        ])
    ]);
    $tok = json_decode(curl_exec($ch), true);
    curl_close($ch);

    if (!empty($tok['access_token'])) {
        $_SESSION['access_token']  = $tok['access_token'];
        $_SESSION['token_expires'] = time() + $tok['expires_in'];
    } else {
        unset($_SESSION['refresh_token']);
        @unlink(TOKEN_FILE);
    }
}

/* 3 ─ Pedir autorización solo si falta el access-token */
if (empty($_SESSION['access_token']) && empty($_GET['code'])) {

    $need_prompt = !is_file(TOKEN_FILE);

    $state = json_encode(['page' => 'detalle_manga', 'id' => ($_GET['id'] ?? null)]);
    $auth_url = 'https://accounts.google.com/o/oauth2/auth?' . http_build_query([
        'response_type' => 'code',
        'client_id'     => CLIENT_ID,
        'redirect_uri'  => REDIRECT_URI,
        'scope'         => SCOPE,
        'access_type'   => 'offline',
        'prompt'        => $need_prompt ? 'consent' : 'none',
        'state'         => $state
    ]);
    header('Location: ' . $auth_url);
    exit;
}

/* 4 ─ Canjear code por tokens y guardar (igual que arriba) */
if (isset($_GET['code'])) {
    $ch = curl_init('https://oauth2.googleapis.com/token');
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_POSTFIELDS     => http_build_query([
            'code'          => $_GET['code'],
            'client_id'     => CLIENT_ID,
            'client_secret' => CLIENT_SECRET,
            'redirect_uri'  => REDIRECT_URI,
            'grant_type'    => 'authorization_code'
        ])
    ]);
    $tok = json_decode(curl_exec($ch), true);
    curl_close($ch);

    error_log('🔑 TOKENS (drive): ' . print_r($tok, true));

    if (!empty($tok['access_token'])) {
        $_SESSION['access_token']  = $tok['access_token'];
        $_SESSION['token_expires'] = time() + $tok['expires_in'];

        if (!empty($tok['refresh_token'])) {
            $_SESSION['refresh_token'] = $tok['refresh_token'];
            $bytes = file_put_contents(
                TOKEN_FILE,
                json_encode(['refresh_token' => $tok['refresh_token']],
                            JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES)
            );
            error_log("💾 SAVE TOKEN (drive): wrote $bytes bytes");
        }

        /* Redirección al punto de partida */
        if (!empty($_GET['state'])) {
            $s = json_decode($_GET['state'], true);
            if ($s && $s['page'] === 'detalle_manga' && !empty($s['id'])) {
                header('Location: detalle_manga.php?id=' . urlencode($s['id']));
                exit;
            }
        }

        header('Location: ' . REDIRECT_URI);
        exit;
    }

    die('Error OAuth (drive): ' . print_r($tok, true));
}

/* 5 ─ Access-token listo → llamar Drive API */
$access_token = $_SESSION['access_token'];

/* ----------- TU LÓGICA PARA LISTAR ARCHIVOS ---------------- */
$ch = curl_init(
    'https://www.googleapis.com/drive/v3/files?' . http_build_query([
        'q'        => sprintf("'%s' in parents and mimeType='application/pdf' and trashed=false", FOLDER_ID),
        'fields'   => 'files(id,name,webViewLink)',
        'pageSize' => 100
    ])
);
curl_setopt_array($ch, [
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_HTTPHEADER     => ["Authorization: Bearer $access_token"]
]);
$data     = json_decode(curl_exec($ch), true) ?: [];
$archivos = $data['files'] ?? [];
curl_close($ch);
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Volúmenes disponibles</title>
    <style>body{font-family:Arial;background:#111;color:#eee;padding:2rem}</style>
</head>
<body>
    <h1>Volúmenes disponibles</h1>
    <?php if ($archivos): ?>
        <ul>
            <?php foreach ($archivos as $f): ?>
                <li><a href="<?= htmlspecialchars($f['webViewLink']) ?>" target="_blank">
                    <?= htmlspecialchars($f['name']) ?></a></li>
            <?php endforeach; ?>
        </ul>
    <?php else: ?><p>No se encontraron PDFs.</p><?php endif; ?>
</body>
</html>
