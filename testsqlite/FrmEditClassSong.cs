using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace biodanza
{
    public partial class FrmEditClassSong : Form
    {
        public Int32 id;
        public Int32 id_item;
        public Int32 id_carpetalista;
        private ListaItemsMng lim;

        public FrmEditClassSong()
        {
            InitializeComponent();
        }

        private void FromEditClassSong_Load(object sender, EventArgs e)
        {
            lim = new ListaItemsMng();
            lim.Filter("id=" + id.ToString());
            

            txtNombre.Text = lim.dv[0]["Titulo"].ToString();
            txtDuracion.Text = lim.dv[0]["duracion"].ToString();
            txtArtista.Text = lim.dv[0]["Autor"].ToString();
            chkRepetir.Checked = Convert.ToBoolean(lim.dv[0]["repetir"]);
            txtAlbum.Text = lim.dv[0]["Album"].ToString();
            txtEmpieza.Text = lim.dv[0]["desde"].ToString().PadLeft(5, '0');
            txtTermina.Text = lim.dv[0]["hasta"].ToString().PadLeft(5, '0');
            txtEsperar.Text = lim.dv[0]["esperar"].ToString();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            lista_items li = new lista_items();
            li.id = this.id;
            li.id_carpetalista = this.id_carpetalista;
            li.id_item = this.id_item;
            li.repetir = Convert.ToInt32(chkRepetir.Checked);
            li.desde = txtEmpieza.Text;
            li.hasta = txtTermina.Text;
            li.orden = Convert.ToInt32(lim.dv[0]["orden"]);
            Int32 esperar = 0;
            Int32.TryParse(txtEsperar.Text, out esperar);
            li.esperar = esperar;


            lim.Update(li);
            Close();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    
    }
}
