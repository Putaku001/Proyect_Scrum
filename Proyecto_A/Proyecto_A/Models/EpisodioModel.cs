using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_A.Models;

public class EpisodioModel
{
    public int EpisodioID { get; set; }
    public int ContenidoID { get; set; }
    public int NumeroEpisodio { get; set; }
    public string Titulo { get; set; }
    public string RutaArchivo { get; set; }
    public DateTime FechaSubida { get; set; }
}

