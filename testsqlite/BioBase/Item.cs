using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    public class Item
    {
        
        public int Id { get; set; }
        public string tipo { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Album { get; set; }
        public string Duracion { get; set; }
        public string genero { get; set; }
        public string Localizacion { get; set; }
        public string idioma { get; set; }
        public string joincolumns { get; set; }
        public bool tieneletra { get; set; }
        public bool perdido { get; set; }
    }
}
