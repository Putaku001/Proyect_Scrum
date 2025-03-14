using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_A.Models;

public class ContenidoModel
{
    public int ContenidoID { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public int CategoriaID { get; set; }
    public DateTime FechaPublicacion { get; set; }
    public string RutaImagen { get; set; } 
}


