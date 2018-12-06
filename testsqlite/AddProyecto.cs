using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections;

namespace biodanza
{
    public partial class AddProyecto : Form
    {
        public DataGridView dgv = null;
        private ArrayList idsCombo;

        public AddProyecto()
        {
            InitializeComponent();
            idsCombo = new ArrayList();
        }

        private void AddProyecto_Load(object sender, EventArgs e)
        {
            CarpetaListaMng pm = new CarpetaListaMng();
            List<Proyecto> lp = pm.Lista();
            foreach (Proyecto prj in lp)
            {
                comboProyecto.Items.Add(prj.titulo);
                idsCombo.Add(prj.id);
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string titulo = txtTitulo.Text;
            string proyecto = comboProyecto.Text;

            // comprobación de título
            if (string.IsNullOrEmpty(titulo))
            {
                MessageBox.Show("Añade un título al proyecto", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Proyecto pro = new Proyecto();
            if (comboProyecto.SelectedIndex != -1)
            {
                pro.id_proyecto = Convert.ToInt32(idsCombo[comboProyecto.SelectedIndex]);
            }
            pro.titulo = titulo;
            
            CarpetaListaMng prom = new CarpetaListaMng();
            Int32 idPro = prom.Nuevo(pro);
        }

        

        //private bool Create(Genero genero)
        //{
        //    bool bRet = false;

        //    SQLiteCommand cmd = new SQLiteCommand(Program.m_dbConnection);
        //    cmd.CommandText = "SELECT MAX(id) FROM generos";
        //    object result = cmd.ExecuteScalar();

        //    int id = 0;
        //    if (result.GetType() != typeof(DBNull))
        //    {
        //        id = Convert.ToInt32(result);
        //    }
        //    id++;


        //    SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Generos (id, titulo) VALUES (?,?)", Program.m_dbConnection);
        //    insertSQL.Parameters.AddWithValue("@id", id);
        //    insertSQL.Parameters.AddWithValue("@titulo", genero.titulo);

        //    try
        //    {
        //        insertSQL.ExecuteNonQuery();
        //        bRet = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return bRet;
        //}

    }
}
