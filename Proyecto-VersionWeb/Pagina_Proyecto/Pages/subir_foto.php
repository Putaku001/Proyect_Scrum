<?php
session_start();

$targetDir = "../assets/imgs/perfiles/";
if (!file_exists($targetDir)) {
    mkdir($targetDir, 0755, true);
}

if (isset($_FILES["foto"]) && $_FILES["foto"]["error"] === UPLOAD_ERR_OK) {
    $extension = pathinfo($_FILES["foto"]["name"], PATHINFO_EXTENSION);
    $filename = "perfil_" . $_SESSION['usuario_id'] . "." . $extension;
    $filePath = $targetDir . $filename;

    move_uploaded_file($_FILES["foto"]["tmp_name"], $filePath);
    $_SESSION['foto_perfil'] = $filePath;
}

header("Location: ./Client/dashboard.php");
exit();
?>  