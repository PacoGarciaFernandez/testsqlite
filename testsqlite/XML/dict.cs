using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    class Diccionario
    {
        public List<Propiedad> claves = null;

        public Diccionario()
        {
            claves = new List<Propiedad>();
        }

        public void Add( string key, object o)
        {
            claves.Add(new Propiedad(key, o));
        }
    }
}
