using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace biodanza
{
    public partial class ListaDeClases : Form
    {
        private ArrayList ids = null;
        public Int32 idClase = -1;

        public ListaDeClases()
        {
            InitializeComponent();
            ids = new ArrayList();
        }

        private void ListaDeClases_Load(object sender, EventArgs e)
        {
            ListaClasesMng lcm = new ListaClasesMng();
            foreach (DataRow row in lcm.dt.Rows)
            {
                lbxClases.Items.Add(row["titulo"].ToString());
                ids.Add(Convert.ToInt32(row["id"]));
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (lbxClases.SelectedIndex != -1)
            {
                this.idClase = Convert.ToInt32(ids[lbxClases.SelectedIndex]);
            }
        }
    }
}
