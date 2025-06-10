<?php
/*-----------------------------------------------------------
 * eliminar_manga.php   (GET ?id=123)
 *   • Borra manga (Drive + disco + BD)
 *----------------------------------------------------------*/
session_start();
require_once 'db.php';
require_once __DIR__.'/vendor/autoload.php';
$client = require 'drive_auth_admin.php';
$drive  = new Google\Service\Drive($client);

const COVER_DIR  = __DIR__.'/imgs/covers';
const TABLAS_HIJAS = ['ProgresoLectura','Favoritos','TitulosAlternativos'];

function idFromUrl(string $url): string {
    // /folders/xxxxxxxx   ó   id=xxxxxxxx   ó  /d/xxxxxxxx/
    if (preg_match('#/folders/([^/?]+)#',$url,$m)) return $m[1];
    if (preg_match('#id=([^&]+)#'              ,$url,$m)) return $m[1];
    if (preg_match('#/d/([^/]+)#'             ,$url,$m)) return $m[1];
    return '';
}

/*── 1. Validar y leer datos del manga ─────────────────────*/
$mangaId = (int)($_GET['id'] ?? 0);
if ($mangaId<=0){ die('ID inválido'); }

$q = sqlsrv_query($conn,"SELECT URLMangaDrive,URLPortadaWeb FROM Mangas WHERE MangaID=?",[$mangaId]);
if(!$row = sqlsrv_fetch_array($q,SQLSRV_FETCH_ASSOC)){ die('Manga no encontrado'); }

$volsUrl   = $row['URLMangaDrive'];
$coverPath = $row['URLPortadaWeb'];

$volsId = idFromUrl($volsUrl);
if(!$volsId){ die('URL de volúmenes inválida'); }

/*── 2.  Obtener carpeta raíz y eliminar en Drive ───────────*/
try{
    $file = $drive->files->get($volsId,['fields'=>'parents']);
    if(empty($file->parents[0])) throw new Exception('Sin padre');
    $rootId = $file->parents[0];

    // ¡Un disparo, una muerte!
    $drive->files->delete($rootId);
}catch(Exception $e){
    die('Error Drive: '.$e->getMessage());
}

/*── 3.  Borrar portada local (si existe) ───────────────────*/
if($coverPath){
    $abs = COVER_DIR.'/'.basename($coverPath);
    if(is_file($abs)) unlink($abs);
}

/*── 4.  Limpiar BD (tablas hijas → tabla Mangas) ───────────*/
sqlsrv_begin_transaction($conn);
try{
    foreach(TABLAS_HIJAS as $t){
        sqlsrv_query($conn,"DELETE FROM $t WHERE MangaID=?",[$mangaId]);
    }
    sqlsrv_query($conn,"DELETE FROM Mangas WHERE MangaID=?",[$mangaId]);
    sqlsrv_commit($conn);
}catch(Throwable $e){
    sqlsrv_rollback($conn);
    die('BD: '.$e->getMessage());
}

/*── 5.  Listo ──────────────────────────────────────────────*/
header('Location: catalogo_admin.php?del='.$mangaId);
exit;
