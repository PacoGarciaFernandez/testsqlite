using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    public class CarpetaLista
    {
        public Int32  id              { get; set; }
        public string titulo          { get; set; }
        public Int32  id_carpetalista { get; set; }
        public bool   procesado       { get; set; }
        public Int32  tipo            { get; set; }  // 0 Carpeta  1 lista
        public string comentario      { get; set; }
        public bool   cerrada         { get; set; }


    }
}
