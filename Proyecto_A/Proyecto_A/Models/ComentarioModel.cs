using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_A.Models;

public class ComentarioModel
{
    public int ComentarioID { get; set; }
    public int UsuarioID { get; set; }
    public int ContenidoID { get; set; }
    public string Texto { get; set; }
    public DateTime FechaComentario { get; set; }
}

