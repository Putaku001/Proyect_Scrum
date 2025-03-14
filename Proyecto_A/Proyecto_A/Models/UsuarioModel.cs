using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_A.Models;

public class UsuarioModel
{
    public int UsuarioID { get; set; }
    public string NombreUsuario { get; set; }
    public string Email { get; set; }
    public string ContrasenaHash { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int RolID { get; set; }
}


