using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace biodanza
{
    public partial class Cancion : Form
    {
        public ItemMng oCancion;
        public Int32 id = 0;
        private string oldTitulo = string.Empty;
        private string oldLocalizacion = string.Empty;
        private DataGridViewRow row;

        public Cancion()
        {
            InitializeComponent();
        }
        public Cancion(DataGridViewRow row)
        {
            this.row = row;
            InitializeComponent();
        }

        private void btnOk(object sender, EventArgs e)
        {
            Item oItem = new Item();
            oItem.Id           = this.id;
            oItem.Titulo       = txtTitulo.Text.Trim();
            oItem.Autor        = txtArtista.Text.Trim();
            oItem.Duracion     = txtDuracion.Text.Trim();
            oItem.genero       = txtGenero.Text.Trim();
            oItem.Localizacion = txtLocalizacion.Text.Trim();
            oItem.Album        = txtAlbum.Text.Trim();

            if (oldTitulo.Trim() != oItem.Titulo.Trim() && oldLocalizacion == oItem.Localizacion)
            {
                // Renombrar el fichero
                string fichero = Path.Combine(oldLocalizacion, oldTitulo);
                string newfichero = Path.Combine(oldLocalizacion, oItem.Titulo);
                try
                {
                    if (!File.Exists(newfichero))
                    {
                        File.Copy(fichero, newfichero);
                    }
                }
                catch(IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

                ItemMng oItemMng = new ItemMng();
            if (id != 0 && oItemMng.ExisteID(id))
            {
                oItemMng.Update(oItem);
            }
            else
            {
                oItemMng.Nuevo(oItem);
            }
        }

        private void Cancion_Load(object sender, EventArgs e)
        {
            this.id = Convert.ToInt32(this.row.Cells["id"].Value);
            oldTitulo = this.row.Cells["titulo"].Value.ToString();
            txtTitulo.Text = oldTitulo;
            txtArtista.Text = this.row.Cells["autor"].Value.ToString();
            txtDuracion.Text = this.row.Cells["duracion"].Value.ToString();
            txtGenero.Text = this.row.Cells["genero"].Value.ToString();
            oldLocalizacion = this.row.Cells["localizacion"].Value.ToString();
            txtLocalizacion.Text = oldLocalizacion;
            txtAlbum.Text = this.row.Cells["album"].Value.ToString();

            

        }
    }


}
