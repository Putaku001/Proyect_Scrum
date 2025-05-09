using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectScrum.Entities
{
    public static class CapturedData
    {
        public static int UsuarioID { get; set; }
        public static DateTime? FechaFinSuscripcion { get; set; }
        public static string Avatar { get; set; }
        public static string NombreUsuario { get; set; }
        public static string Email { get; set; }
        public static string ContraseñaHash { get; set; }
        public static bool EsPremium { get; set; }
        public static int RolID { get; set; }

    }
}