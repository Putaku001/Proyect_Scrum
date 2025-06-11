<?php
session_start();
require_once '../../Config/db.php';

if (!isset($_SESSION['usuario_id']) || !isset($_POST['manga_id'])) {
    http_response_code(403);
    echo "No autorizado";
    exit;
}

$usuarioId = $_SESSION['usuario_id'];
$mangaId = intval($_POST['manga_id']);

$sql = "DELETE FROM Favoritos WHERE UsuarioID = ? AND MangaID = ?";
$stmt = sqlsrv_query($conn, $sql, [$usuarioId, $mangaId]);

if ($stmt) {
    echo "ok";
} else {
    http_response_code(500);
    echo "Error al eliminar";
}
