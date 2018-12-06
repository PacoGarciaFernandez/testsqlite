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
    public partial class FrmSortItems : Form
    {
        private Int32 id_carpetalista;
        private Dictionary<string, Int32> ids;
        public Dictionary<Int32, Int32> ordenados;

        public FrmSortItems()
        {
            InitializeComponent();
        }

        private void FrmSortItems_Load(object sender, EventArgs e)
        {
            ordenados = new Dictionary<int, int>();
        }
        public void LoadCarpetaLista(Int32 id_clase, int iSelect)
        {
            id_carpetalista = id_clase;
            ids = new Dictionary<string,int>();
            ListaItemsMng lim = new ListaItemsMng();
            lim.Filter("ID_CARPETALISTA=" + id_clase.ToString());
            lim.Sort("ORDEN ASC");
            foreach (DataRowView row in lim.dv)
            {
                lbxCanciones.Items.Add(row["titulo"].ToString());
                ids.Add(row["titulo"].ToString(), Convert.ToInt32(row["id"]));
            }

            lbxCanciones.Focus();
            lbxCanciones.SelectedIndex = iSelect;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbxCanciones.SelectedItems.Count > 0)
            {
                object selected = lbxCanciones.SelectedItem;
                int indx = lbxCanciones.Items.IndexOf(selected);
                int totl = lbxCanciones.Items.Count;

                if (indx == 0)
                {
                    lbxCanciones.Items.Remove(selected);
                    lbxCanciones.Items.Insert(totl - 1, selected);
                    lbxCanciones.SetSelected(totl - 1, true);
                }
                else
                {
                    lbxCanciones.Items.Remove(selected);
                    lbxCanciones.Items.Insert(indx - 1, selected);
                    lbxCanciones.SetSelected(indx - 1, true);
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lbxCanciones.SelectedItems.Count > 0)
            {
                object selected = lbxCanciones.SelectedItem;
                int indx = lbxCanciones.Items.IndexOf(selected);
                int totl = lbxCanciones.Items.Count;

                if (indx == totl - 1)
                {
                    lbxCanciones.Items.Remove(selected);
                    lbxCanciones.Items.Insert(0, selected);
                    lbxCanciones.SetSelected(0, true);
                }
                else
                {
                    lbxCanciones.Items.Remove(selected);
                    lbxCanciones.Items.Insert(indx + 1, selected);
                    lbxCanciones.SetSelected(indx + 1, true);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Int32 indice = 0;
            foreach (String item in lbxCanciones.Items)
            {
                indice++;
                ordenados.Add(ids[item], indice);
            }

        }

        private void lbxCanciones_DrawItem(object sender, DrawItemEventArgs e)
        {
            bool isSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            if (e.Index > -1)
            {
                /* If the item is selected set the background color to SystemColors.Highlight 
                 or else set the color to either WhiteSmoke or White depending if the item index is even or odd */
                Color color = isSelected ? SystemColors.Highlight :
                    e.Index % 2 == 0 ? Color.White : Color.WhiteSmoke;

                // Background item brush
                SolidBrush backgroundBrush = new SolidBrush(color);
                // Text color brush
                SolidBrush textBrush = new SolidBrush(e.ForeColor);

                // Draw the background
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                // Draw the text
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.FormatFlags = StringFormatFlags.NoWrap;

                e.Graphics.DrawString(lbxCanciones.GetItemText(lbxCanciones.Items[e.Index]), e.Font, textBrush, e.Bounds, stringFormat);

                // Clean up
                backgroundBrush.Dispose();
                textBrush.Dispose();
            }
            e.DrawFocusRectangle();
        }
    }
}
