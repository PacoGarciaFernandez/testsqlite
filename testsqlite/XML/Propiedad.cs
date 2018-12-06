using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    class Propiedad
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }

        public Propiedad(string n, object o)
        {
            Nombre = n;
            Valor = o;
        }
    }
}
