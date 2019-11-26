using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace biodanza
{
    public class CarpetaListaMng: DB
    {
        public Int32 idCarpetaLista = 0; 

        public CarpetaListaMng():base()
        {
            this.tableName = "CARPETALISTA";
            this.dt = GetData();
        }

        public override DataTable GetData()
        {

            this.CommandText = "SELECT A.ID, A.TITULO, A.ID_CARPETALISTA, A.TIPO, A.COMENTARIO, A.CERRADA, A.BORRADA " +
                                 "FROM " + this.tableName + " A"; 
            return base.GetData();
        }

        public int Nuevo(CarpetaLista p)
        {
            Int32 id = this.NewId();
            if (p.id != 0)
            {
                id = p.id;
            }

            string sql = "INSERT INTO " + tableName + " (id, titulo, id_carpetalista, tipo, comentario, cerrada, borrada ) " +
                   string.Format(" VALUES ({0},{1},{2},{3},{4},{5},{6})", id, "'" + p.titulo + "'", p.id_carpetalista, p.tipo, "'" + p.comentario + "'", p.cerrada?0:1, 0);
            this.Execute(sql);
            return id;
        }

        public override  int Delete(Int32 id)
        {
            
            string sql = "DELETE FROM " + tableName + " where ID = " + id.ToString();
            this.Execute(sql);

            // Borro las canciones de esta clase
            ListaItemsMng lim = new ListaItemsMng();
            lim.Delete(id);

            // Si existen carpetalistas que tengan como id_carpetalista (padre) a mi id, las borro
            SQLiteCommand cmd = new SQLiteCommand(Program.m_dbConnection);
            cmd.CommandText = "select count(id) from " + this.tableName + " where id_carpetalista=" + id.ToString();
            Int32 count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count > 0)
            {
                CarpetaListaMng clm = new CarpetaListaMng();
                clm.Delete(id.ToString());
            }

            return 0;
        }

        public void UpdateTitle(Int32 id, string titulo)
        {
            string sql = "UPDATE " + tableName + " SET titulo=" + "'" + titulo + "'" +
                            " where ID = " + id.ToString();

            this.Execute(sql);

            return;
        }


        public List<CarpetaLista> Lista()
        {
            List<CarpetaLista> lp = new List<CarpetaLista>();

            if (this.idCarpetaLista != 0)
            {
                this.Filter("id_carpetalista " + this.idCarpetaLista.ToString());
            }

            foreach (DataRowView row in dv)
            {
                CarpetaLista p = new CarpetaLista();
                p.id              = Convert.ToInt32  (row["id"]);
                p.titulo          = Convert.ToString (row["titulo"]);
                p.procesado       = false;
                p.id_carpetalista = Convert.ToInt32  (row["id_carpetalista"]);
                p.tipo            = Convert.ToInt32  (row["tipo"]);
                p.comentario      = Convert.ToString (row["comentario"]);
                p.cerrada         = Convert.ToBoolean(row["cerrada"]);
                lp.Add(p);
            }
            return lp;
        }
        public CarpetaLista GetCarpetaLista(Int32 id)
        {
            SQLiteCommand cmd = new SQLiteCommand(Program.m_dbConnection);
            cmd.CommandText = "SELECT A.ID           AS ID, " +
                                     "A.TITULO       AS TITULO, " +
                                     "A.ID_CARPETALISTA AS ID_CARPETALISTA, " +
                                     "A.TIPO         AS TIPO, " +
                                     "A.COMENTARIO   AS COMENTARIO, " +
                                     "A.CERRADA      AS CERRADA " +
                                     "FROM " + this.tableName + " A WHERE ID ='" + id.ToString() + "'";

            SQLiteDataReader row = cmd.ExecuteReader();
            row.Read();

            CarpetaLista item = new CarpetaLista();

            item.id = row.GetInt32(0);    
            item.titulo = row.GetString(1);  
            item.id_carpetalista = row.GetInt32(2);
            item.tipo = row.GetInt32(3);             
            item.comentario = row.GetString(4); 
            item.cerrada = row.GetBoolean(5);

            return item;

        }
    }
}
