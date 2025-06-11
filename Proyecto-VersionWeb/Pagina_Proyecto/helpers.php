<?php
/* helpers.php
 * ——— Funciones comunes ——— */
function avatarSrcFromDB($conn, $usuarioID) {
    $sql = "SELECT Avatar FROM Usuarios WHERE UsuarioID = ?";
    $stmt = sqlsrv_query($conn, $sql, [$usuarioID]);
    $row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC);
    return ($row && $row['Avatar'])
        ? 'data:image/png;base64,' . base64_encode($row['Avatar'])
        : './assets/imgs/default.png';
}
