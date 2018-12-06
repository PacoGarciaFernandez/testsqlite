using System;
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
    public partial class MusicaFrm : Form
    {

        List<string> m_paths = new List<string>();
        DataTable dtCanciones = new DataTable();

        public MusicaFrm()
        {
            InitializeComponent();
        }

        private void MusicaFrm_Load(object sender, EventArgs e)
        {
            m_paths.Add(@"C:\Users\francisco.garcia\Music\Romantica");


            DataGridViewCheckBoxColumn doWork = new DataGridViewCheckBoxColumn();
            doWork.HeaderText = "Selecionar";
            doWork.FalseValue = "0";
            doWork.TrueValue = "1";
            gridCanciones.Columns.Insert(0, doWork);
        }
    }
}
