using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace biodanza
{


    public class ItemMng : DB
    {
        public string tipo = "*";

        public ItemMng() : base()
        {
            this.tableName = "ITEMS";

            this.dt = GetData();
        }

        public override DataTable GetData()
        {
            this.CommandText = "SELECT A.ID           AS ID, "+
                                      "A.TIPOITEM     AS TIPO, " +
                                      "A.TITULO       AS TITULO, " + 
                                      "A.AUTOR        AS AUTOR, " + 
                                      "A.ALBUM        AS ALBUM, " + 
                                      "A.DURACION     AS DURACION, " + 
                                      "A.GENERO       AS GENERO, " +
                                      "A.IDIOMA       AS IDIOMA, " + //  "A.TIENELETRA   AS TIENELETRA, " +  "A.PERDIDO      AS PERDIDO, " +
                                      "A.LOCALIZACION AS LOCALIZACION " +
                                    "FROM " + this.tableName + " A ";
            return base.GetData();
        }
        
        public int Nuevo(FileInfo fi)
        {
            Item item = new Item();

            if (fi.Extension.ToUpper() != ".MP3")
                return 0;

            TagLib.File y = null;
            var id3 = y;

            try
            {
                id3 = TagLib.File.Create(fi.FullName);
            }
            catch (Exception)
            {
                return 0;
            }
            string tipoItem = "'Música', ";
            string Album    =  Sanitize(id3.Tag.Album) ;
            string Titulo   =  Sanitize(Path.GetFileName(fi.FullName)) ;
            string Artist   =  Sanitize(id3.Tag.FirstArtist) ;
            string duracion =  Sanitize(id3.Properties.Duration.ToString(@"mm\:ss")) ;

            string genero   = "'', ";
            if (id3.Tag.Genres.Count() > 0)
            {
                genero = Sanitize(id3.Tag.Genres[0]);
            }
            string file = "'" + Sanitize(fi.DirectoryName) + "'";

            if (string.IsNullOrEmpty(Album))
            {
                return 0 ;
            }
            string idioma = "";
            bool tieneletra = !string.IsNullOrEmpty(id3.Tag.Lyrics);
            
            string joincolumns = ", '" + Album + " " + Titulo + " " + Artist + " " + genero + "' ";
            joincolumns = joincolumns.ToLower();
                        
            Album = "'" + Album + "', ";
            Titulo = "'" + Titulo + "', ";
            Artist = "'" + Artist + "', ";
            duracion = "'" + duracion + "', ";
            genero = "'', ";
            if (id3.Tag.Genres.Count() > 0)
            {
                genero = "'" + Sanitize(id3.Tag.Genres[0]) + "', ";
            }

            
            string fichero = fi.DirectoryName;
            file = fichero.Replace(@"file:///", "");
            bool perdido = !File.Exists(file);

            //file = "'" + Sanitize(fi.DirectoryName) + "'";

            string Query = "INSERT INTO " + tableName + " (Id, tipoitem, Album, Titulo, Autor, Duracion, genero, Localizacion, idioma, joincolumns, tieneletra)  values(" + NewId().ToString() + ", " + 
                tipoItem + 
                Album +
                Titulo +
                Artist +
                duracion + 
                genero +
                file + 
                idioma +
                (tieneletra?"1":"0") +
                joincolumns + ")";

            SQLiteCommand cmd = new SQLiteCommand(Query, Program.m_dbConnection);
            cmd.ExecuteNonQuery();
            
            return 1;
        }
        public int Nuevo(Item item)
        {
            string file = string.Empty;

            if (item.Id != 0 && ExisteID(item.Id))
            {
                return 1;
            }

            //if (item.Titulo.Substring(item.Titulo.Length-4).ToLower() != ".mp3")
            //    return 0;
            int id = item.Id;
            if (id == 0)
            {
                id = NewId();
            }

            string tipoItem = "'Música', ";
            string Album = Sanitize(item.Album);
            string Titulo = Sanitize(item.Titulo);
            string Artist = Sanitize(item.Autor);
            string duracion = item.Duracion;
            string genero = Sanitize(item.genero);
            string idioma = item.idioma;
            string tieneletra = item.tieneletra?"1":"0"+", ";
            string perdido = item.perdido?"1":"0";
            string joincolumns = "'" + Album + " " + Titulo + " " + Artist + " " + genero + "', ";

            //Uri uriAddress = new Uri(item.Localizacion);
            //item.Localizacion = uriAddress.ToString();
            item.Localizacion = Sanitize(item.Localizacion.Replace("file://localhost/", ""));
            //item.Localizacion = Sanitize(item.Localizacion.Replace(@"file:///", "").Replace(@"\\\\", @"/"));
            item.Localizacion = item.Localizacion.Replace(@"file:///", "").Replace(@"\\\\", @"/");
            joincolumns = joincolumns.ToLower();

            Album = "'" + Album + "', ";
            Titulo = "'" + Titulo + "', ";
            Artist = "'" + Artist + "', ";
            duracion = "'" + duracion + "', ";
            genero = "'" + genero + "', ";
            idioma = "'" + idioma + "', ";
            
            file = "'" + item.Localizacion + "', ";
            //file = Sanitize(file);
            
            string Query = "INSERT INTO " + tableName + " (Id, tipoitem, Album, Titulo, Autor, Duracion, genero, Localizacion, idioma, joincolumns, tieneletra, perdido)  values(" + 
                id.ToString() + ", " +
                tipoItem +
                Album +
                Titulo +
                Artist +
                duracion +
                genero +
                file +
                idioma +
                joincolumns + 
                tieneletra + 
                perdido + ")";

            

            SQLiteCommand cmd = new SQLiteCommand(Query, Program.m_dbConnection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(file + Environment.NewLine);
                File.AppendAllText(e.Message + "-" + Path.Combine(Directory.GetCurrentDirectory(), "log.txt"), sb.ToString());
                sb.Clear();
            }

            return 1;
        }

        public void FullFilter(string text)
        {
            string query = "Titulo, Autor, Duracion, genero";
        }
        
        public override int Delete(Int32 id)
        {
            return this.Execute("DELETE FROM " + this.tableName + " WHERE ID = " + id.ToString());
        }
        public bool ExisteCancion(string cancion)
        {
            cancion = Sanitize(cancion);

            SQLiteCommand cmd = new SQLiteCommand(Program.m_dbConnection);
            cmd.CommandText = "select count(id) from " + this.tableName + " where titulo like '" + cancion + "';";
            cmd.CommandType = CommandType.Text;
            int RowCount = 0;

            RowCount = Convert.ToInt32(cmd.ExecuteScalar());
            return RowCount > 0;
        }
        
        public Item GetItem(Int32 id)
        {
            SQLiteCommand cmd = new SQLiteCommand(Program.m_dbConnection);
            cmd.CommandText = "SELECT A.ID AS ID, " +
                                     "A.TIPOITEM     AS TIPO, " +
                                     "A.TITULO       AS TITULO, " +
                                     "A.AUTOR        AS AUTOR, " +
                                     "A.ALBUM        AS ALBUM, " +
                                     "A.DURACION     AS DURACION, " +
                                     "A.GENERO       AS GENERO, " +
                                     "A.IDIOMA       AS IDIOMA, " +
                                     "A.LOCALIZACION AS LOCALIZACION, " +
                                     "A.TIENELETRA   AS TIENELETRA, " +
                                     "A.PERDIDO      AS PERDIDO " +
                                   "FROM " + this.tableName + " A WHERE ID ='" + id.ToString() + "'"  ;
            SQLiteDataReader row = cmd.ExecuteReader();
            row.Read();

            Item item = new Item();

            item.Id = row.GetInt32(0);    // Convert.ToInt32(row.Item["id"]);
            item.tipo = row.GetString(1);  // Convert.ToString(row["tipo"]);
            item.Titulo = row.GetString(2);  // Convert.ToString(row["Titulo"]);
            item.Autor = row.GetString(3);  // Convert.ToString(row["Autor"]);
            item.Album = row.GetString(4);  // Convert.ToString(row["Album"]);
            item.Duracion = row.GetString(5);  // Convert.ToString(row["Duracion"]);
            item.genero = row.GetString(6);  // Convert.ToString(row["genero"]);
            item.idioma = row.GetString(7);  // Convert.ToString(row["idioma"]);
            item.Localizacion = row.GetString(8);  // Convert.ToString(row["Localizacion"]);
            item.tieneletra = row.GetBoolean(9);
            item.perdido = row.GetBoolean(10);
            return item;
        }
        public List<Item> Lista()
        {
            List<Item> li = new List<Item>();

            foreach (DataRowView row in dv)
            {
                Item item = new Item();
                item.Id = Convert.ToInt32(row["id"]);
                item.tipo = Convert.ToString(row["tipo"]);
                item.Titulo = Convert.ToString(row["Titulo"]);
                item.Autor = Convert.ToString(row["Autor"]);
                item.Album = Convert.ToString(row["Album"]);
                item.Duracion = Convert.ToString(row["Duracion"]);
                item.genero = Convert.ToString(row["genero"]);
                item.idioma = Convert.ToString(row["idioma"]);
                item.Localizacion = Convert.ToString(row["Localizacion"]);
                item.tieneletra = Convert.ToBoolean(row["tieneletra"]);
                item.perdido = Convert.ToBoolean(row["perdido"]);
                li.Add(item);
            }
            return li;
        }
        public void Update(Item it)
        {
            it.Titulo = Sanitize(it.Titulo);


            string sql = "UPDATE " + tableName + " SET ID=" + it.Id.ToString() + ", " +
                                                      "TIPOITEM     ='"+ it.tipo   +"', " +
                                                      "TITULO       ='"+ it.Titulo + "', " +
                                                      "AUTOR        ='" + it.Autor +"', " +
                                                      "ALBUM        ='" + it.Album + "', " +
                                                      "DURACION     ='" + it.Duracion + "', " +
                                                      "GENERO       ='" + it.genero + "', " +
                                                      "IDIOMA       ='" + it.idioma + "', " +
                                                      "TIENELETRA   =" + (it.tieneletra?"1":"0") + ", "+
                                                      "PERDIDO      =" + (it.perdido ? "1" : "0") + ", " +
                                                      "LOCALIZACION ='" + it.Localizacion + "' " +
                            " where ID = " + it.Id.ToString();

            this.Execute(sql);
        }
        public void UpdateLost(Item it)
        {
            it.Titulo = Sanitize(it.Titulo);


            string sql = "UPDATE " + tableName + " SET ID=" + it.Id.ToString() + ", " +
                                                      "PERDIDO      =" + (it.perdido ? "1" : "0") +
                            " where ID = " + it.Id.ToString();

            this.Execute(sql);
        }

    }

    public class CancionMng : ItemMng
    {
        public CancionMng()
        {
            tipo = "Música";
        }
    }
}
