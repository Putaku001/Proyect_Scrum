<?php
/************************************************************************
 * google_drive.php
 *  - Usa OAuth 2.0 “installed app” (flujo en navegador)
 *  - Guarda refresh-token en disco para que la sesión de Drive dure meses
 *  - Renueva access-token en cada carga SIN pedir de nuevo consentimiento
 *  - Solo redirige a Google si el refresh-token no existe o está revocado
 ***********************************************************************/

session_start();

/*───────── CONFIGURACIÓN ─────────────────────────────────────────────*/
const CLIENT_ID     = '719046572033-27o8382k35lnbvkeo2fn4j0hu7bfvev9.apps.googleusercontent.com';
const CLIENT_SECRET = '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';
const REDIRECT_URI  = 'http://localhost/Pagina_Proyecto/google_drive.php';
const SCOPE         = 'https://www.googleapis.com/auth/drive.readonly';
const TOKEN_FILE    = __DIR__ . '/token_guardado.json';   // ⚠️ Fuera del doc-root si es posible

/* ID de la carpeta Drive que contiene los PDFs (puedes traerla de BD) */
const FOLDER_ID     = '1yg7WSwfztQeGgGWybI1ngNSn2nIs494O';
/*─────────────────────────────────────────────────────────────────────*/

$manga_id = $_GET['id'] ?? null;

/*───────── 1) Cargar refresh-token persistente ───────────────────────*/
if (empty($_SESSION['refresh_token']) && is_file(TOKEN_FILE)) {
    $json = json_decode(file_get_contents(TOKEN_FILE), true) ?? [];
    $_SESSION['refresh_token'] = $json['refresh_token'] ?? null;
}

/*───────── 2) Renovar el access-token siempre que haya refresh ───────*/
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
        // Falló el refresh (revocado / caducado)
        unset($_SESSION['refresh_token']);
        @unlink(TOKEN_FILE);
    }
}

/*───────── 3) Si aún falta token ⇒ pedir autorización a Google ──────*/
if (empty($_SESSION['access_token']) && empty($_GET['code'])) {
    $state = json_encode(['page' => 'detalle_manga', 'id' => $manga_id]);
    $auth_url = 'https://accounts.google.com/o/oauth2/auth?' . http_build_query([
        'response_type' => 'code',
        'client_id'     => CLIENT_ID,
        'redirect_uri'  => REDIRECT_URI,
        'scope'         => SCOPE,
        'access_type'   => 'offline'       // necesario para recibir refresh-token
        // sin 'prompt' → Google decide si muestra consentimiento
        ,'state'        => $state
    ]);
    header('Location: ' . $auth_url);
    exit;
}

/*───────── 4) Canjear “code” por tokens (solo primera vez) ───────────*/
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

    if (!empty($tok['access_token'])) {
        $_SESSION['access_token']  = $tok['access_token'];
        $_SESSION['token_expires'] = time() + $tok['expires_in'];

        if (!empty($tok['refresh_token'])) {
            $_SESSION['refresh_token'] = $tok['refresh_token'];
            file_put_contents(TOKEN_FILE, json_encode(
                ['refresh_token' => $tok['refresh_token']],
                JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES
            ));
        }

        /* Redirigir a detalle_manga si veníamos de allí */
        if (!empty($_GET['state'])) {
            $state = json_decode($_GET['state'], true);
            if ($state && $state['page'] === 'detalle_manga' && !empty($state['id'])) {
                header('Location: detalle_manga.php?id=' . urlencode($state['id']));
                exit;
            }
        }

        header('Location: ' . REDIRECT_URI);  // recargar limpio
        exit;
    }

    die('<h2>Error al canjear el code:</h2><pre>' . print_r($tok, true) . '</pre>');
}

/*───────── 5) Ya tenemos access-token válido → usar Drive API ────────*/
$access_token = $_SESSION['access_token'];

$ch = curl_init(
    'https://www.googleapis.com/drive/v3/files?' .
    http_build_query([
        'q'      => sprintf("'%s' in parents and mimeType='application/pdf' and trashed=false", FOLDER_ID),
        'fields' => 'files(id,name,webViewLink)',
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
    <title>Volúmenes desde Google Drive</title>
    <style>
        body{font-family:Arial,Helvetica,sans-serif;background:#111;color:#eee;padding:2rem}
        h1{margin-bottom:1rem}
        ul{list-style:none;padding:0}
        li{margin:.4rem 0}
        a{color:#4ec7ff;text-decoration:none}
        a:hover{text-decoration:underline}
    </style>
</head>
<body>
    <h1>Volúmenes disponibles</h1>

    <?php if ($archivos): ?>
        <ul>
            <?php foreach ($archivos as $f): ?>
                <li>
                    <a href="<?= htmlspecialchars($f['webViewLink']) ?>" target="_blank">
                        <?= htmlspecialchars($f['name']) ?>
                    </a>
                </li>
            <?php endforeach; ?>
        </ul>
    <?php else: ?>
        <p>No se encontraron archivos PDF en la carpeta.</p>
    <?php endif; ?>
</body>
</html>
