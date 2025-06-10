
<?php
if (session_status() === PHP_SESSION_NONE) session_start();
require_once __DIR__ . '/vendor/autoload.php';

const GD_CREDENTIALS = __DIR__ . '/Api/client_secret_719046572033-6q6ip50hqb30bhp43cjqo7jaht5ci1a1.apps.googleusercontent.com.json';
const GD_TOKEN       = __DIR__ . '/Api/token_admin.json';

if (!file_exists(GD_CREDENTIALS)) die("❌ Credenciales no encontradas");

$creds = json_decode(file_get_contents(GD_CREDENTIALS), true);
$clientId     = $creds['web']['client_id'];
$clientSecret = $creds['web']['client_secret'];
$redirectUri  = $creds['web']['redirect_uris'][0];

$client = new Google_Client();
$client->setClientId($clientId);
$client->setClientSecret($clientSecret);
$client->setRedirectUri($redirectUri);
$client->setAccessType('offline');
$client->setPrompt('select_account consent');
$client->addScope(Google_Service_Drive::DRIVE);

if (file_exists(GD_TOKEN)) {
    $token = json_decode(file_get_contents(GD_TOKEN), true);
    $client->setAccessToken($token);

    if ($client->isAccessTokenExpired()) {
        if (!empty($token['refresh_token'])) {
            $postFields = http_build_query([
                'client_id' => $clientId,
                'client_secret' => $clientSecret,
                'refresh_token' => $token['refresh_token'],
                'grant_type' => 'refresh_token',
            ]);
            $ch = curl_init('https://oauth2.googleapis.com/token');
            curl_setopt_array($ch, [
                CURLOPT_RETURNTRANSFER => true,
                CURLOPT_POST => true,
                CURLOPT_POSTFIELDS => $postFields,
                CURLOPT_HTTPHEADER => ['Content-Type: application/x-www-form-urlencoded'],
            ]);
            $response = curl_exec($ch);
            curl_close($ch);

            $newToken = json_decode($response, true);
            if (!empty($newToken['access_token'])) {
                $newToken['refresh_token'] = $token['refresh_token'];
                file_put_contents(GD_TOKEN, json_encode($newToken));
                $client->setAccessToken($newToken);
            } else {
                unlink(GD_TOKEN);
                header("Location: drive_auth_admin.php");
                exit;
            }
        } else {
            unlink(GD_TOKEN);
            header("Location: drive_auth_admin.php");
            exit;
        }
    }
} elseif (!isset($_GET['code'])) {
    $_SESSION['redirect'] = $_SERVER['REQUEST_URI'];
    header("Location: " . $client->createAuthUrl());
    exit;
} else {
    $postFields = http_build_query([
        'code' => $_GET['code'],
        'client_id' => $clientId,
        'client_secret' => $clientSecret,
        'redirect_uri' => $redirectUri,
        'grant_type' => 'authorization_code',
    ]);
    $ch = curl_init('https://oauth2.googleapis.com/token');
    curl_setopt_array($ch, [
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_POST => true,
        CURLOPT_POSTFIELDS => $postFields,
        CURLOPT_HTTPHEADER => ['Content-Type: application/x-www-form-urlencoded'],
    ]);
    $response = curl_exec($ch);
    curl_close($ch);

    $token = json_decode($response, true);
    if (!is_array($token) || isset($token['error'])) {
        $msg = $token['error_description'] ?? 'Error desconocido.';
        die("❌ Autenticación fallida: $msg");
    }

    file_put_contents(GD_TOKEN, json_encode($token));
    header("Location: " . ($_SESSION['redirect'] ?? 'subidaMangaForm.php'));
    exit;
}

return $client;
?>
