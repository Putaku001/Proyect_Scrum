<?php
/*-----------------------------------------------------------
 * eliminar_manga.php   (GET ?id=123)
 *   • Borra manga (Drive + disco + BD)
 *----------------------------------------------------------*/
session_start();
require_once '../../Config/db.php';
require_once __DIR__ . '/../../vendor/autoload.php';
$client = require '../../drive_auth_admin.php';
$drive  = new Google\Service\Drive($client);

const COVER_DIR  = __DIR__ . '/../../assets/imgs/covers';
const TABLAS_HIJAS = ['ProgresoLectura', 'Favoritos', 'TitulosAlternativos'];

function idFromUrl(string $url): string {
    // /folders/xxxxxxxx   ó   id=xxxxxxxx   ó  /d/xxxxxxxx/
    if (preg_match('#/folders/([^/?]+)#', $url, $m)) return $m[1];
    if (preg_match('#id=([^&]+)#', $url, $m)) return $m[1];
    if (preg_match('#/d/([^/]+)#', $url, $m)) return $m[1];
    return '';
}

/*── 1. Validar y leer datos del manga ─────────────────────*/
/*── 1.  Validar ID recibido (GET o POST) ─────────────────────*/
$idParam = $_GET['id']           // URL: …/eliminar_manga.php?id=123
          ?? $_POST['manga_id']  // Formulario oculto (por si acaso)
          ?? '';

if ($idParam === '' || !ctype_digit((string)$idParam)) {
    die('ID inválido');
}
$mangaId = (int) $idParam;       // «0» también es válido


$q = sqlsrv_query($conn, "SELECT URLMangaDrive, URLPortadaWeb FROM Mangas WHERE MangaID = ?", [$mangaId]);
if (!$row = sqlsrv_fetch_array($q, SQLSRV_FETCH_ASSOC)) {
    die('Manga no encontrado');
}

$volsUrl   = $row['URLMangaDrive'];
$coverPath = $row['URLPortadaWeb'];

$volsId = idFromUrl($volsUrl);
if (!$volsId) {
    die('URL de volúmenes inválida');
}

/*── 2.  Obtener carpeta raíz y eliminar en Drive ───────────*/
/*── 2.  Eliminar carpeta en Drive (si existe) ───────────────*/
try {
    $file = $drive->files->get($volsId, ['fields' => 'parents']);
    if (!empty($file->parents[0])) {
        $drive->files->delete($file->parents[0]); // carpeta raíz
    }
} catch (\Google\Service\Exception $e) {
    /* 404  = archivo/carpeta no existe
       403  = sin permisos (la carpeta fue borrada o no es accesible)  */
    if (!in_array($e->getCode(), [403, 404])) {
        die('Error Drive: ' . $e->getMessage()); // cualquier otro error real
    }
    // 403/404 → la carpeta ya no está; seguimos con la eliminación local
}


/*── 4.  Limpiar BD (tablas hijas → tabla Mangas) ───────────*/
sqlsrv_begin_transaction($conn);
try {
    foreach (TABLAS_HIJAS as $t) {
        sqlsrv_query($conn, "DELETE FROM $t WHERE MangaID = ?", [$mangaId]);
    }
    sqlsrv_query($conn, "DELETE FROM Mangas WHERE MangaID = ?", [$mangaId]);
    sqlsrv_commit($conn);
} catch (Throwable $e) {
    sqlsrv_rollback($conn);
    die('BD: ' . $e->getMessage());
}

/*── 5.  Redirigir correctamente ───────────────────────────*/
/*── 5.  Redirigir correctamente ───────────────────────────*/
// Estando en pages/Admin/, un “..” te lleva a pages/
header('Location: ../catalogo_admin.php?del=' . $mangaId);
exit;
t;


