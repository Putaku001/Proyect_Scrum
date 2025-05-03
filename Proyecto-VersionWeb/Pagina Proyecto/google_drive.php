<?php
session_start();

$client_id = '719046572033-27o8382k35lnbvkeo2fn4j0hu7bfvev9.apps.googleusercontent.com';
$client_secret = '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';
$redirect_uri = 'http://localhost/Pagina%20Proyecto/google_drive.php';
$scope = 'https://www.googleapis.com/auth/drive.readonly';

$manga_id = $_GET['id'] ?? null;

if (!isset($_GET['code']) && !isset($_SESSION['access_token'])) {
    $auth_url = "https://accounts.google.com/o/oauth2/auth?" . http_build_query([
        'response_type' => 'code',
        'client_id'     => $client_id,
        'redirect_uri'  => $redirect_uri,
        'scope'         => $scope,
        'access_type'   => 'offline',
        'prompt'        => 'consent',
        'state'         => json_encode(['page' => 'detalle_manga', 'id' => $manga_id])
    ]);
    header('Location: ' . $auth_url);
    exit();
}

if (isset($_GET['code'])) {
    $code = $_GET['code'];

    $ch = curl_init('https://oauth2.googleapis.com/token');
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query([
        'code'          => $code,
        'client_id'     => $client_id,
        'client_secret' => $client_secret,
        'redirect_uri'  => $redirect_uri,
        'grant_type'    => 'authorization_code'
    ]));

    $response = curl_exec($ch);
    $token_data = json_decode($response, true);
    curl_close($ch);

    if (isset($token_data['access_token'])) {
        $_SESSION['access_token'] = $token_data['access_token'];

        if (isset($token_data['refresh_token'])) {
            $_SESSION['refresh_token'] = $token_data['refresh_token'];
        }

        if (isset($_GET['state'])) {
            $state = json_decode($_GET['state'], true);
            if ($state && $state['page'] == 'detalle_manga' && isset($state['id'])) {
                header("Location: detalle_manga.php?id=" . urlencode($state['id']));
                exit();
            }
        }

        header('Location: google_drive.php');
        exit();
    } else {
        echo "<h2>Error al obtener el token:</h2>";
        echo "<pre>" . print_r($token_data, true) . "</pre>";
        exit();
    }
}

if (!isset($_SESSION['access_token']) && isset($_SESSION['refresh_token'])) {
    $ch = curl_init('https://oauth2.googleapis.com/token');
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query([
        'client_id'     => $client_id,
        'client_secret' => $client_secret,
        'refresh_token' => $_SESSION['refresh_token'],
        'grant_type'    => 'refresh_token'
    ]));

    $response = curl_exec($ch);
    $new_token_data = json_decode($response, true);
    curl_close($ch);

    if (isset($new_token_data['access_token'])) {
        $_SESSION['access_token'] = $new_token_data['access_token'];
    } else {
        echo "<h2>Error al refrescar el token:</h2>";
        echo "<pre>" . print_r($new_token_data, true) . "</pre>";
        exit();
    }
}

// Listar archivos PDF desde Drive
$access_token = $_SESSION['access_token'];

// ⚠️ Cambia esto al ID real de la carpeta donde están los tomos
$folder_id = '1yg7WSwfztQeGgGWybI1ngNSn2nIs494O';

$ch = curl_init("https://www.googleapis.com/drive/v3/files?q=" . urlencode("'$folder_id' in parents and mimeType='application/pdf'") . "&fields=files(id,name,webViewLink)&pageSize=100");
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, [
    "Authorization: Bearer $access_token"
]);
$response = curl_exec($ch);
$data = json_decode($response, true);
curl_close($ch);

$archivos = $data['files'] ?? [];
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Volúmenes desde Google Drive</title>
</head>
<body>
    <h1>Volúmenes Disponibles</h1>
    <?php if (!empty($archivos)): ?>
        <ul>
            <?php foreach ($archivos as $archivo): ?>
                <li>
                    <a href="<?= htmlspecialchars($archivo['webViewLink']) ?>" target="_blank">
                        <?= htmlspecialchars($archivo['name']) ?>
                    </a>
                </li>
            <?php endforeach; ?>
        </ul>
    <?php else: ?>
        <p>No se encontraron archivos PDF en esta carpeta.</p>
    <?php endif; ?>
</body>
</html>