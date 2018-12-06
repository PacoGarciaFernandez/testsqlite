using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    class ListaItemsMng: DB
    {
        //public int idClase;

        public ListaItemsMng():base()
        {
            this.tableName = "LISTA_ITEMS";

            this.dt = GetData();
            
        }
        public override DataTable GetData()
        {
            this.CommandText = "SELECT  A.ID AS ID, " + 
                                       "A.ID_CARPETALISTA AS ID_CARPETALISTA, " + 
                                       "A.ID_ITEM         AS ID_ITEM, " + 
                                       "A.ORDEN           AS ORDEN, " + 
                                       "A.REPETIR         AS REPETIR, " +
                                       "A.DESDE           AS DESDE, " +
                                       "A.HASTA           AS HASTA, " +
                                       "A.ESPERAR         AS ESPERAR, " +
                                       "B.ID              AS ITEMID, " + 
                                       "B.TITULO          AS TITULO, " + 
                                       "B.TIPOITEM        AS TIPOITEM, " + 
                                       "B.AUTOR           AS AUTOR, " + 
                                       "B.DURACION        AS DURACION, " + 
                                       "B.ALBUM           AS ALBUM, " + 
                                       "B.GENERO          AS GENERO, " + 
                                       "B.IDIOMA          AS IDIOMA, " + 
                                       "B.LOCALIZACION    AS LOCALIZACION " + //   "B.TIENELETRA      AS TIENELETRA " +
                " FROM " + this.tableName + " A LEFT JOIN ITEMS B ON A.ID_ITEM=B.ID ";
           
            return base.GetData();
        }
        
        public int Nuevo(lista_items li)
        {
            if (ExisteID(li.id))
            {
                return 1;
            }

            if (li.orden < 0)
            {
                // Dar como número el máximo + 1
                li.orden = NextOrden(li.id_carpetalista);
            }

            string sql = "INSERT INTO " + tableName + " (id, id_carpetalista, id_item, orden, repetir, desde, hasta ) " +
                   string.Format(" VALUES ({0},{1},{2},{3},{4},{5},{6})", this.NewId(), li.id_carpetalista, li.id_item,li.orden,li.repetir,"'"+li.desde+"'", "'" + li.hasta + "'",li.esperar);
            return this.Execute(sql);
        }

        public Int32 NextOrden( Int32 id_carpetalista )
        {
            int id = 0;
            SQLiteCommand cmd = new SQLiteCommand("SELECT MAX(orden) FROM " + this.tableName + " WHERE id_carpetalista=" + id_carpetalista.ToString(), Program.m_dbConnection);
            try
            {
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            id++;

            return id;
        }


        public override int Delete(Int32 idClase )
        {
            return this.Execute("DELETE FROM " + this.tableName + " WHERE ID_CARPETALISTA = " + idClase.ToString());
            //return this.Execute("UPDATE FROM " + this.tableName + " SET borrada=1 WHERE ID = " + idClase.ToString());

        }

        public void Update(lista_items li)
        {
            string sql = "UPDATE " + tableName + " SET id = " + li.id.ToString() +
                                                    ", id_carpetalista=" + li.id_carpetalista.ToString() + 
                                                    ", id_item="         + li.id_item.ToString() + 
                                                    ", orden="           + li.orden.ToString() + 
                                                    ", repetir="         + li.repetir.ToString()  +
                                                    ", desde="           + "'" +li.desde + "'" +
                                                    ", hasta="           + "'" + li.hasta +"'" +
                                                    ", esperar="         + li.esperar  +
               " where ID = " + li.id.ToString();

            this.Execute(sql);

            return;
        }

        public void UpdateOrden(Int32 id, Int32 orden)
        {
            string sql = "UPDATE " + tableName + " SET orden=" + orden.ToString() +
               " where ID = " + id.ToString();

            this.Execute(sql);

            return;
        }

    }
}
