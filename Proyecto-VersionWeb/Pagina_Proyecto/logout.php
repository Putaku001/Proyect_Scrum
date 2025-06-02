<?php
session_start();

/* Borra solo la info de tu app y el access-token */
unset($_SESSION['usuario_id'], $_SESSION['nombre'], $_SESSION['rol'],
      $_SESSION['access_token'], $_SESSION['token_expires']);

session_regenerate_id(true);          // nuevo ID por seguridad
header('Location: login.html');
exit;
?>
