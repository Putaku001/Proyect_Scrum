<?php
/**
 * Devuelve la ruta/URI del avatar que debe mostrarse.
 * Prioridad:
 *   1) BLOB almacenado en la sesión   → data-URI
 *   2) Imagen por defecto             → ./imgs/default.png
 */
function avatarSrcFromSession(): string
{
    return isset($_SESSION['avatar_bin']) && $_SESSION['avatar_bin'] !== ''
        ? 'data:image/png;base64,' . base64_encode($_SESSION['avatar_bin'])
        : './imgs/default.png';
}
