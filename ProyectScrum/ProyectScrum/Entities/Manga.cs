using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectScrum.Entities
{
    public class Manga
    {
        public int MangaID { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string URLMangaDrive { get; set; }
        public string URLPortada { get; set; }
    }
}
