using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace biodanza
{
    static class Program
    {
        public static SQLiteConnection m_dbConnection;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            connectToDatabase();

            Application.Run(new MusicManager());

            m_dbConnection.Close();
        }
        // Creates a connection with our database file.
        public static void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=Biodanza.sqlite;Version=3; ");
            m_dbConnection.Open();
        }
    }
}
