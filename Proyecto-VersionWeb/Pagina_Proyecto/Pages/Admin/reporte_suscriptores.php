<?php
session_start();
include("../../Config/db.php");
include_once("limpiar_suscripciones.php");

if (!isset($_SESSION['usuario_id']) || $_SESSION['rol'] != 2) {
    header("Location: ../../Public/login.html");
    exit();
}

require_once('../../vendor/tecnickcom/tcpdf/tcpdf.php');

// Trae el campo Cancelada
$sql = "SELECT u.NombreUsuario, u.Email, s.TipoSuscripcion, s.FechaInicio, s.FechaFin, s.Cancelada
        FROM Suscripciones s
        INNER JOIN Usuarios u ON s.UsuarioID = u.UsuarioID
        WHERE s.FechaFin >= CONVERT(date, GETDATE())
        ORDER BY s.FechaInicio DESC";
$stmt = sqlsrv_query($conn, $sql);

$pdf = new TCPDF();
$pdf->SetCreator('MangaVerse Admin');
$pdf->SetAuthor('MangaVerse');
$pdf->SetTitle('Reporte de Suscriptores');
$pdf->SetHeaderData('', 0, 'Reporte de Suscriptores', 'MangaVerse 2025');
$pdf->setHeaderFont(array('helvetica', '', 12));
$pdf->setFooterFont(array('helvetica', '', 10));
$pdf->SetMargins(15, 27, 15);
$pdf->SetAutoPageBreak(TRUE, 25);
$pdf->AddPage();
$pdf->SetFont('helvetica', '', 12);

// Título
$pdf->Cell(0, 10, 'Reporte de Usuarios Suscritos', 0, 1, 'C');
$pdf->Ln(5);

// Tabla
$html = '<table border="1" cellpadding="4">
            <tr style="background-color:#f0f0f0;">
                <th><b>Nombre de Usuario</b></th>
                <th><b>Email</b></th>
                <th><b>Tipo de Suscripción</b></th>
                <th><b>Fecha Inicio</b></th>
                <th><b>Fecha Fin</b></th>
                <th><b>Estado</b></th>
            </tr>';

while ($row = sqlsrv_fetch_array($stmt, SQLSRV_FETCH_ASSOC)) {
    // Determina el estado
    if (!empty($row['Cancelada']) && $row['Cancelada'] == 1) {
        $estado = "Cancelada (activa)";
    } else {
        $estado = "Activa";
    }

    $html .= '<tr>
                <td>' . htmlspecialchars($row['NombreUsuario']) . '</td>
                <td>' . htmlspecialchars($row['Email']) . '</td>
                <td>' . htmlspecialchars($row['TipoSuscripcion']) . '</td>
                <td>' . ($row['FechaInicio'] instanceof DateTime ? $row['FechaInicio']->format('Y-m-d') : date('Y-m-d', strtotime($row['FechaInicio']))) . '</td>
                <td>' . ($row['FechaFin'] instanceof DateTime ? $row['FechaFin']->format('Y-m-d') : date('Y-m-d', strtotime($row['FechaFin']))) . '</td>
                <td>' . $estado . '</td>
              </tr>';
}

$html .= '</table>';

$pdf->writeHTML($html, true, false, true, false, '');

$pdf->Output('reporte_suscriptores.pdf', 'D');
