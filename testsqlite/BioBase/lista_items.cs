using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    class lista_items
    {
        public int id              { get; set; }
        public int id_carpetalista { get; set; }
        public int id_item         { get; set; }
        public int orden           { get; set; }
        public int repetir         { get; set; }
        public string desde        { get; set; }
        public string hasta        { get; set; }
        public int esperar      { get; set; }
    }
}
