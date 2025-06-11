<?php
session_start();
require_once '../../Config/db.php';

if (!isset($_SESSION['usuario_id'])) {
    header("Location: ../../Public/login.html");
    exit();
}

if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['manga_id'])) {
    $usuarioId = $_SESSION['usuario_id'];
    $mangaId = intval($_POST['manga_id']);

    // Verificar si ya está en favoritos
    $sqlCheck = "SELECT 1 FROM Favoritos WHERE UsuarioID = ? AND MangaID = ?";
    $stmtCheck = sqlsrv_query($conn, $sqlCheck, [$usuarioId, $mangaId]);

    if (!$stmtCheck) {
        die("Error al verificar favoritos.");
    }

    if (!sqlsrv_fetch($stmtCheck)) {
        // Insertar favorito si no existe
        $sqlInsert = "INSERT INTO Favoritos (UsuarioID, MangaID) VALUES (?, ?)";
        $stmtInsert = sqlsrv_query($conn, $sqlInsert, [$usuarioId, $mangaId]);

        if (!$stmtInsert) {
            die("Error al agregar a favoritos.");
        }
    }

    // Redirigir de vuelta a detalle
    header("Location: ../detalle_manga.php?id=$mangaId");
    exit();
} else {
    die("Solicitud inválida.");
}
?>
