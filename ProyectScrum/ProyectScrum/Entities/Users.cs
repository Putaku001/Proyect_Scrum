using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectScrum.Entities
{
    public class Users
    {
        public int UsuarioID { get; set; }
        public DateTime? FechaFinSuscripcion { get; set; }
        public string Avatar { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string ContraseñaHash { get; set; }
        public bool EsPremium { get; set; }
        public int RolID { get; set; }
    }
}