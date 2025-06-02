<?php
/*────────────────────────────────────────────────────────────
 * drive_auth.php
 *  – Gestiona OAuth 2.0 con Google Drive.
 *  – Persiste refresh-token en disco y renueva automáticamente
 *    el access-token sin volver a pedir consentimiento.
 *  – Devuelve $access_token listo para usar.
 *───────────────────────────────────────────────────────────*/

if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

/* ─── CONFIG ───────────────────────────────────────────────*/
const GD_CLIENT_ID     = '719046572033-27o8382k35lnbvkeo2fn4j0hu7bfvev9.apps.googleusercontent.com';
const GD_CLIENT_SECRET = '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';
const GD_REDIRECT_URI  = 'http://localhost/Pagina_Proyecto/drive_auth.php';
const GD_SCOPE         = 'https://www.googleapis.com/auth/drive.readonly';
const GD_TOKEN_FILE    = __DIR__ . '/token_guardado.json';
/*───────────────────────────────────────────────────────────*/

/* 1 ─ Cargar refresh-token persistente */
if (empty($_SESSION['refresh_token']) && is_file(GD_TOKEN_FILE)) {
    $stored = json_decode(file_get_contents(GD_TOKEN_FILE), true) ?? [];
    $_SESSION['refresh_token'] = $stored['refresh_token'] ?? null;
}

/* 2 ─ Renovar access-token cuando sea necesario */
$need_new = empty($_SESSION['access_token'])
         || time() >= ($_SESSION['token_expires'] ?? 0);

if ($need_new && !empty($_SESSION['refresh_token'])) {
    $ch = curl_init('https://oauth2.googleapis.com/token');
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_POSTFIELDS     => http_build_query([
            'client_id'     => GD_CLIENT_ID,
            'client_secret' => GD_CLIENT_SECRET,
            'refresh_token' => $_SESSION['refresh_token'],
            'grant_type'    => 'refresh_token'
        ])
    ]);
    $tok = json_decode(curl_exec($ch), true);
    curl_close($ch);

    if (!empty($tok['access_token'])) {
        $_SESSION['access_token']  = $tok['access_token'];
        $_SESSION['token_expires'] = time() + $tok['expires_in'];
    } else {                              // revocado / expirado
        unset($_SESSION['refresh_token']);
        @unlink(GD_TOKEN_FILE);
    }
}

/* 3 ─ Autorizar en Google solo si no hay refresh-token */
if (empty($_SESSION['access_token']) && empty($_GET['code'])) {
    $state = json_encode(['back' => $_SERVER['REQUEST_URI']]);
    $auth  = 'https://accounts.google.com/o/oauth2/auth?' . http_build_query([
        'response_type' => 'code',
        'client_id'     => GD_CLIENT_ID,
        'redirect_uri'  => GD_REDIRECT_URI,
        'scope'         => GD_SCOPE,
        'access_type'   => 'offline',
        'state'         => $state
    ]);
    header('Location: ' . $auth);
    exit;
}

/* 4 ─ Canjear “code” por tokens (solo la primera vez) */
if (isset($_GET['code'])) {
    $ch = curl_init('https://oauth2.googleapis.com/token');
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_POSTFIELDS     => http_build_query([
            'code'          => $_GET['code'],
            'client_id'     => GD_CLIENT_ID,
            'client_secret' => GD_CLIENT_SECRET,
            'redirect_uri'  => GD_REDIRECT_URI,
            'grant_type'    => 'authorization_code'
        ])
    ]);
    $tok = json_decode(curl_exec($ch), true);
    curl_close($ch);

    if (!empty($tok['access_token'])) {
        $_SESSION['access_token']  = $tok['access_token'];
        $_SESSION['token_expires'] = time() + $tok['expires_in'];

        if (!empty($tok['refresh_token'])) {
            $_SESSION['refresh_token'] = $tok['refresh_token'];
            file_put_contents(GD_TOKEN_FILE, json_encode(
                ['refresh_token' => $tok['refresh_token']],
                JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES
            ));
        }

        /* Vuelve a la URL original */
        $back = '/';
        if (!empty($_GET['state'])) {
            $s = json_decode($_GET['state'], true);
            if (!empty($s['back'])) $back = $s['back'];
        }
        header('Location: ' . $back);
        exit;
    }

    die('Error OAuth: ' . print_r($tok, true));
}

/* 5 ─ Access-token listo para usar */
$access_token = $_SESSION['access_token'];
