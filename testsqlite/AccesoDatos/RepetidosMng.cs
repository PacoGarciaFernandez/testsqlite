using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{


    public class RepetidosMng : DB
    {
        public RepetidosMng() : base()
        {
            this.tableName = "REPETIDOS";

            this.dt = GetData();
            
        }

        public override DataTable GetData()
        {
            this.CommandText = "SELECT ID, SELECCIONADO, FILENAME, TITULO, localizacion, AUTOR, JOINCOLUMNS " +
                " FROM " + this.tableName + " ORDER BY TITULO, localizacion ";
            return base.GetData();
        }
        
        public int Nuevo(Repetidos rep)
        {
            string id = rep.id.ToString();
            string seleccionado = "0";
            string filename = Sanitize(rep.FileName);
            string TITULO   =  Sanitize(rep.Titulo) ; 
            string localizacion   =  Sanitize(rep.localizacion);
            string autor   =  Sanitize(rep.Autor) ;
            string joincolumns = TITULO + " " + localizacion + " " + autor ;

            joincolumns = joincolumns.ToLower();
            if( rep.id == 0 )
            {
                id = NewId().ToString();
            }

            id = "'" + id + "', ";
            seleccionado = "'" + seleccionado + "', ";
            filename = "'" + filename + "', ";
            TITULO = "'" + TITULO + "', ";
            localizacion = "'" + localizacion + "', ";
            autor = "'" + autor + "', ";
            joincolumns = "'" + joincolumns + "'";
            
            string Query = "INSERT INTO " + tableName + " (id, seleccionado, FILENAME, TITULO, localizacion, autor, joincolumns)  values(" + 
                id + 
                seleccionado +
                filename +
                TITULO +
                localizacion +
                autor +
                joincolumns + ")";

            SQLiteCommand cmd = new SQLiteCommand(Query, Program.m_dbConnection);
            cmd.ExecuteNonQuery();
            
            return 1;
        }
        public void FullFilter(string text)
        {
          
        }

        public void DeleteNoDuplicates()
        {
            
            int currentRow = 0;
            while( currentRow < dt.Rows.Count )
            {
                DataRow row = dt.Rows[currentRow];
                string cancion = row["TITULO"].ToString();
                Int32 id = Convert.ToInt32(row["id"]);
                if (!ExisteCancion(cancion, id))
                {
                    dt.Rows.RemoveAt(currentRow);
                }
                else
                { currentRow++; }
            }
        }

        public override int Delete(Int32 id)
        {
            return this.Execute("DELETE FROM " + this.tableName + " WHERE ID = " + id.ToString());
        }

        public int DeleteAll()
        {
            return this.Execute("DELETE FROM " + this.tableName );
        }
        public bool ExisteCancion(string cancion, Int32 exceptId = 0)
        {
            cancion = Sanitize(cancion);

            SQLiteCommand cmd = new SQLiteCommand(Program.m_dbConnection);
            cmd.CommandText = "select count(id) from " + this.tableName + " where TITULO like '" + cancion + "' " +
                " or FILENAME like '" + cancion + "' ";
            if (exceptId != 0)
            {
                cmd.CommandText += " AND id <> " + exceptId.ToString() ;
            }
            
            cmd.CommandType = CommandType.Text;
            int RowCount = 0;

            RowCount = Convert.ToInt32(cmd.ExecuteScalar());
            return RowCount > 0;
        }
    }
}
