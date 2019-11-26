using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace biodanza
{
    public class DB
    {
        public DataTable dt = null;
        public DataView dv = null;
        public string tableName;
        public string CommandText;

        public DB()
        {
            dt = new DataTable();
        }

        public int Execute(string commandText)
        {
            SQLiteCommand sqlCommand = new SQLiteCommand( commandText, Program.m_dbConnection);
            
            int ret = sqlCommand.ExecuteNonQuery();

            return ret;
        }

        public virtual DataTable GetData()
        {
            DataTable data = new DataTable();

            SQLiteCommand sqlcmd = Program.m_dbConnection.CreateCommand();
            SQLiteDataAdapter sqlda = new SQLiteDataAdapter(this.CommandText, Program.m_dbConnection);
            sqlda.Fill(data);
            this.dt = data;
            this.dv = new DataView(dt);

            return data;
        }

        public virtual int Delete( Int32 id )
        {
            return this.Execute( "DELETE FROM " + tableName + " WHERE id=" + id.ToString() );            
        }

        public virtual int Delete(string where)
        {
            return this.Execute("DELETE FROM " + tableName + " " + where );
        }

        public DataView Filter(string query)
        {
            dv.RowFilter = query;
            return dv;
        }

        public void Sort(string query)
        {
            dv.Sort = query;
            return ;
        }

        public void Attach(DataGridView dvg)
        {
            dvg.DataSource = dv; 
        }

        public Int32 NewId()
        {
            int id = 0;
            SQLiteCommand cmd = new SQLiteCommand("SELECT MAX(id) FROM " + this.tableName, Program.m_dbConnection);
            try
            {
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch( Exception )
            {

            }
            id++;

            return id;
        }
        public string Sanitize(string text)
        {
            if(text == null)
            { text = ""; }
            text = text.Replace("'", " ");
            text = text.Replace(@"\", @"\\");
           
            //text = text.Replace(@"/", @"");
            //text = text.Replace(@"_", @"%5F");
            return text;
        }
        public bool ExisteID(Int32 id)
        {
            SQLiteCommand cmd = new SQLiteCommand(Program.m_dbConnection);
            cmd.CommandText = "select count(id) from " + this.tableName + " where ID=" + id.ToString();
            cmd.CommandType = CommandType.Text;
            int RowCount = 0;

            RowCount = Convert.ToInt32(cmd.ExecuteScalar());
            return RowCount > 0;
        }

        public void Erase()
        {
            this.Execute("DELETE FROM " + tableName );
            return;
        }

    }
    
}
