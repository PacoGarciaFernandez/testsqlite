using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace biodanza
{
    class MusicaMng: DB
    {
        public Int32 idProyecto = 0;
        /*         public int Id { get; set; }
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
    }*/
        public MusicaMng():base()
        {
            this.tableName = "ITEMS";
            
            this.dt = GetData();
            
        }

        public override DataTable GetData()
        {

            this.CommandText = "SELECT 0 AS REPETIR, " + 
                                     "ID, " + 
                                     "TIPOITEM, " + 
                                     "TITULO, " + 
                                     "AUTOR, " + 
                                     "ALBUM, " + 
                                     "DURACION, " + 
                                     "GENERO, " + 
                                     "IDIOMA, " + 
                                     "LOCALIZACION, " +
                                     "JOINCOLUMNS, " +
                                     "IDIOMA " + // "TIENELETRA, " + "PERDIDO " +
                                 "FROM " + this.tableName ; 

            return base.GetData();
        }
        public Dictionary<string, Int32> GetClasses()
        {
            Dictionary<string, Int32> ret = new Dictionary<string, int>();
            foreach (DataRow row in GetData().Rows)
            {
                ret.Add(row["titulo"].ToString(), Convert.ToInt32(row["id"]));
            }
            return ret;
        }
    }
}
