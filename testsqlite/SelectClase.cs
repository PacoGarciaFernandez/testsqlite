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
    public partial class SelectClase : Form
    {
        private ArrayList ids = new ArrayList();
        public Int32 id;

        public SelectClase()
        {
            InitializeComponent();
        }

        private void SelectClase_Load(object sender, EventArgs e)
        {
            CarpetaListaMng cm = new CarpetaListaMng();
            foreach(CarpetaLista cl in cm.Lista())
            {
                if( cl.tipo == 1) // lista
                listBox1.Items.Add(cl.titulo);
                ids.Add(cl.id);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 index = listBox1.SelectedIndex;
            id = Convert.ToInt32(ids[index]);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
                button1.PerformClick();
        }
    }
}
