<?php
/*────────────────────────────────────────────────────────────
 * drive_auth.php
 *  – Gestiona OAuth 2.0 con Google Drive.
 *  – Guarda refresh-token en disco y lo reutiliza para todos
 *    los usuarios de la aplicación.
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
const GD_TOKEN_FILE    = __DIR__ . '/token_guardado.json';   // misma carpeta
/*───────────────────────────────────────────────────────────*/

/* 1 ─ Cargar refresh-token persistente (si existe en disco) */
if (empty($_SESSION['refresh_token']) && is_file(GD_TOKEN_FILE)) {
    $stored = json_decode(file_get_contents(GD_TOKEN_FILE), true) ?? [];
    $_SESSION['refresh_token'] = $stored['refresh_token'] ?? null;
}

/* 2 ─ Renovar access-token si no existe o caducó */
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
    } else {                    // refresh-token revocado o caducado
        error_log('⚠️  REFRESH TOKEN inválido, pidiendo consentimiento de nuevo');
        unset($_SESSION['refresh_token']);
        @unlink(GD_TOKEN_FILE);
    }
}

/* 3 ─ Pedir autorización SOLO si aún no hay access-token */
if (empty($_SESSION['access_token']) && empty($_GET['code'])) {

    /* Fuerza “consent” solamente cuando NO existe token en disco */
    $need_prompt = !is_file(GD_TOKEN_FILE);

    $state = json_encode(['back' => $_SERVER['REQUEST_URI']]);
    $auth  = 'https://accounts.google.com/o/oauth2/auth?' . http_build_query([
        'response_type' => 'code',
        'client_id'     => GD_CLIENT_ID,
        'redirect_uri'  => GD_REDIRECT_URI,
        'scope'         => GD_SCOPE,
        'access_type'   => 'offline',
        'prompt'        => $need_prompt ? 'consent' : 'none',
        'state'         => $state
    ]);

    header('Location: ' . $auth);
    exit;
}

/* 4 ─ Canjear “code” por tokens (solo primera vez) */
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

    error_log('🔑 TOKENS RECIBIDOS: ' . print_r($tok, true)); // DEBUG

    if (!empty($tok['access_token'])) {

        $_SESSION['access_token']  = $tok['access_token'];
        $_SESSION['token_expires'] = time() + $tok['expires_in'];

        /* Guardar refresh-token en disco (solo la primera vez) */
        if (!empty($tok['refresh_token'])) {
            $_SESSION['refresh_token'] = $tok['refresh_token'];
            $bytes = file_put_contents(
                GD_TOKEN_FILE,
                json_encode(['refresh_token' => $tok['refresh_token']],
                            JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES)
            );
            error_log("💾 SAVE TOKEN: wrote $bytes bytes to " . GD_TOKEN_FILE);
        }

        /* Volver a la URL original */
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

/* 5 ─ Access-token listo para usar por el resto del script */
$access_token = $_SESSION['access_token'];
