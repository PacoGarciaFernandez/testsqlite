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
    public partial class Clase : Form
    {
        CarpetaLista clase = null;
        Int32 curRow = 0;
        public Clase()
        {
            InitializeComponent();
        }

        public Clase(CarpetaLista cl)
        {
            InitializeComponent();
            clase = cl;
            SetData();
            ConfigurarGridItems();
        }

        public void SetData()
        {
            ListaItemsMng lim = new ListaItemsMng();
            lim.Filter("ID_CARPETALISTA=" + clase.id.ToString());
            lim.Sort("ORDEN ASC");
            lim.Attach(gridItemsClase);
        }

        private void gridItemsClase_SelectionChanged(object sender, EventArgs e)
        {
            curRow = gridItemsClase.CurrentRow.Index;
            if (curRow >= gridItemsClase.Rows.Count-1)
                return;
            txtTitulo.Text = gridItemsClase.Rows[curRow].Cells["titulo"].Value.ToString();
            txtDuracion.Text = gridItemsClase.Rows[curRow].Cells["duracion"].Value.ToString();
            txtArtista.Text = gridItemsClase.Rows[curRow].Cells["autor"].Value.ToString();

            chRepetir.Checked = Convert.ToBoolean(gridItemsClase.Rows[curRow].Cells["repetir"].Value);

            txtAlbum.Text = gridItemsClase.Rows[curRow].Cells["album"].Value.ToString();
            txtIni.Text = gridItemsClase.Rows[curRow].Cells["desde"].Value.ToString();
            txtFin.Text = gridItemsClase.Rows[curRow].Cells["hasta"].Value.ToString();
            
        }

        private void gridItemsClase_MouseClick(object sender, MouseEventArgs e)
        {
            curRow = gridItemsClase.SelectedCells[0].RowIndex;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            curRow = gridItemsClase.CurrentRow.Index;
            if (curRow == 0 || gridItemsClase.Rows.Count < 2)
                return;
            Int32 curOrder = Convert.ToInt32(gridItemsClase.Rows[curRow].Cells["orden"].Value);
            gridItemsClase.Rows[curOrder].Selected = false;
            Int32 curId = Convert.ToInt32(gridItemsClase.Rows[curRow].Cells["id"].Value);
            Int32 upOrder = Convert.ToInt32(gridItemsClase.Rows[curRow-1].Cells["orden"].Value);
            Int32 upId = Convert.ToInt32(gridItemsClase.Rows[curRow-1].Cells["id"].Value);

            ListaItemsMng lim = new ListaItemsMng();
            lim.UpdateOrden(curId, upOrder);
            lim.UpdateOrden(upId, curOrder);
            SetData();
            
            gridItemsClase.Rows[curOrder-1].Selected = true;

        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            curRow = gridItemsClase.CurrentRow.Index;
            if (curRow == gridItemsClase.Rows.Count || gridItemsClase.Rows.Count < 2)
                return;
            Int32 curOrder = Convert.ToInt32(gridItemsClase.Rows[curRow].Cells["orden"].Value);
            gridItemsClase.Rows[curOrder].Selected = false;
            Int32 curId = Convert.ToInt32(gridItemsClase.Rows[curRow].Cells["id"].Value);
            Int32 upOrder = Convert.ToInt32(gridItemsClase.Rows[curRow + 1].Cells["orden"].Value);
            Int32 upId = Convert.ToInt32(gridItemsClase.Rows[curRow + 1].Cells["id"].Value);

            ListaItemsMng lim = new ListaItemsMng();
            lim.UpdateOrden(curId, upOrder);
            lim.UpdateOrden(upId, curOrder);
            SetData();
            gridItemsClase.Rows[curOrder].Selected = true;


        }

        private void ConfigurarGridItems()
        {
            gridItemsClase.RowHeadersVisible = false;
            gridItemsClase.CellBorderStyle = DataGridViewCellBorderStyle.None;

            foreach (DataGridViewColumn c in gridItemsClase.Columns)
            {
                if (c.Name != "ORDEN")
                {
                    c.ReadOnly = true;
                }
            }

            gridItemsClase.Columns["id"].Visible = false;
            gridItemsClase.Columns["id_carpetalista"].Visible = false;
            gridItemsClase.Columns["id_item"].Visible = false;
            gridItemsClase.Columns["itemid"].Visible = false;
            gridItemsClase.Columns["orden"].Visible = false;
            gridItemsClase.Columns["desde"].Visible = true;
            gridItemsClase.Columns["hasta"].Visible = true;

            gridItemsClase.Columns["localizacion"].Visible = false;

            gridItemsClase.Columns["repetir"].Visible = false;
            gridItemsClase.Columns["repetir"].Width = 40;
            gridItemsClase.Columns["repetir"].HeaderText = "Rep.";
            gridItemsClase.Columns["repetir"].DisplayIndex = 4;
            gridItemsClase.Columns["repetir"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if (!gridItemsClase.Columns.Contains("loop"))
            {
                DataGridViewImageColumn img = new DataGridViewImageColumn();
                img.Name = "loop";
                //img.DefaultCellStyle.SelectionBackColor = this.gridItems.DefaultCellStyle.BackColor;
                //img.DefaultCellStyle.SelectionForeColor = this.gridItems.DefaultCellStyle.ForeColor;
                this.gridItemsClase.Columns.Add(img);
            }
            gridItemsClase.Columns["loop"].Visible = true;
            gridItemsClase.Columns["loop"].DisplayIndex = 0;
            gridItemsClase.Columns["loop"].HeaderText = "Rep.";
            gridItemsClase.Columns["loop"].Width = 40;

            gridItemsClase.Columns["ORDEN"].Width = 50;
            gridItemsClase.Columns["ORDEN"].HeaderText = "Orden";
            gridItemsClase.Columns["ORDEN"].DisplayIndex = 0;
            gridItemsClase.Columns["ORDEN"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItemsClase.Columns["TITULO"].Width = 250;
            gridItemsClase.Columns["TITULO"].HeaderText = "Nombre";
            gridItemsClase.Columns["TITULO"].DisplayIndex = 1;
            gridItemsClase.Columns["TITULO"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItemsClase.Columns["TIPOITEM"].Visible = false;
            gridItemsClase.Columns["TIPOITEM"].HeaderText = "Tipo";
            gridItemsClase.Columns["TIPOITEM"].Width = 40;
            gridItemsClase.Columns["TIPOITEM"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItemsClase.Columns["DURACION"].Width = 90;
            gridItemsClase.Columns["DURACION"].HeaderText = "Duración";
            gridItemsClase.Columns["DURACION"].DisplayIndex = 2;
            gridItemsClase.Columns["DURACION"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridItemsClase.Columns["duracion"].DefaultCellStyle.Format = "HH:mm:ss";

            gridItemsClase.Columns["AUTOR"].Width = 180;
            gridItemsClase.Columns["AUTOR"].HeaderText = "Artista";
            gridItemsClase.Columns["AUTOR"].DisplayIndex = 3;
            gridItemsClase.Columns["AUTOR"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItemsClase.Columns["album"].Visible = true;
            gridItemsClase.Columns["album"].DisplayIndex = 5;
            gridItemsClase.Columns["album"].HeaderText = "Album";
            gridItemsClase.Columns["album"].Width = 250;
            gridItemsClase.Columns["album"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;



            gridItemsClase.Columns["GENERO"].Width = 250;
            gridItemsClase.Columns["GENERO"].HeaderText = "Género";
            gridItemsClase.Columns["GENERO"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItemsClase.Columns["desde"].Visible = true;
            gridItemsClase.Columns["desde"].DisplayIndex = 7;
            gridItemsClase.Columns["desde"].HeaderText = "Empieza";
            gridItemsClase.Columns["desde"].Width = 70;
            gridItemsClase.Columns["desde"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItemsClase.Columns["hasta"].Visible = true;
            gridItemsClase.Columns["hasta"].DisplayIndex = 8;
            gridItemsClase.Columns["hasta"].HeaderText = "Termina";
            gridItemsClase.Columns["hasta"].Width = 700;
            gridItemsClase.Columns["hasta"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItemsClase.Columns["duracion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridItemsClase.Columns["duracion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //gridCarpetaLista.RowsDefaultCellStyle.BackColor = Color.White;
            //gridCarpetaLista.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);


        }

       
    }
}
