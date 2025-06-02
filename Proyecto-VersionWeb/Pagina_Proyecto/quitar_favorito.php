<?php
session_start();
require_once 'db.php';

if (!isset($_SESSION['usuario_id'])) {
    header("Location: login.html");
    exit();
}

if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['manga_id'])) {
    $usuarioId = $_SESSION['usuario_id'];
    $mangaId = intval($_POST['manga_id']);

    $sql = "DELETE FROM Favoritos WHERE UsuarioID = ? AND MangaID = ?";
    $stmt = sqlsrv_query($conn, $sql, [$usuarioId, $mangaId]);

    header("Location: detalle_manga.php?id=$mangaId&quitar=1");
    exit();
}
?>
