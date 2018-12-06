using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace biodanza
{
    public partial class NewQuery : UserControl
    {
        public DataTable dt; 
        public NewQuery()
        {
            InitializeComponent();
            CreateDataTable();
        }

        private void CreateDataTable()
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("add", typeof(string)));
            dt.Columns.Add(new DataColumn("del", typeof(string)));
            
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 i = grid.SelectedCells[0].RowIndex;
            if (i > -1 && grid.SelectedCells[0].ColumnIndex == 0)
            {
                // añadimos fila
                AddRow();
            }
            else if(i > -1 && grid.SelectedCells[0].ColumnIndex == 1)
            {
                DeleteRow();
            }
        }

        private void AddRow()
        {
            grid.Rows.Add();
        }
        private void DeleteRow()
        {
            Int32 i = grid.SelectedCells[0].RowIndex;
            grid.Rows.RemoveAt(i);
        }
    }
}
