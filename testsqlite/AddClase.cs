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
    public partial class AddClase : Form
    {
        public DataGridView dgv = null;
        private Int32 idProyecto;
        private ArrayList idsCombo;

        public AddClase(Int32 idProyecto = -1)
        {
            InitializeComponent();
            idsCombo = new ArrayList();
            this.idProyecto = idProyecto;
        }

        private void AddClase_Load(object sender, EventArgs e)
        {
            Int32 id = -1;
            CarpetaListaMng pm = new CarpetaListaMng();
            List<Proyecto> lp = pm.Lista();
            int count = 0;
            foreach (Proyecto prj in lp)
            {
                comboProyecto.Items.Add(prj.titulo);
                idsCombo.Add(prj.id);
                if (this.idProyecto == prj.id)
                {
                    id = count;
                }
                count ++;
            }
            if (this.idProyecto != -1)
            {
                comboProyecto.SelectedIndex=id;
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string titulo = txtTitulo.Text;
            string comentarios = textComentarios.Text;
            string proyecto = comboProyecto.Text;
            Int32 idProyecto = -1;

            // comprobación de título
            if (string.IsNullOrEmpty(titulo))
            {
                MessageBox.Show("Añade un título a la clase", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Clase clase = new Clase();
            clase.titulo = titulo;
            clase.cerrada = false;
            clase.comentarios = comentarios;
            ListaClasesMng lcm = new ListaClasesMng();
            Int32 idClase = lcm.Nuevo(clase);

            if (!string.IsNullOrEmpty(proyecto))
            {
                // añadimos la clase al proyecto
                idProyecto = Convert.ToInt32(idsCombo[comboProyecto.SelectedIndex]);
                ListaClasesProMng lcpm = new ListaClasesProMng();
                lista_clases lc = new lista_clases();
                lc.id_proyecto = idProyecto;
                lc.id_clase = idClase;
                lcpm.Nuevo(lc);
            }
                            
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
