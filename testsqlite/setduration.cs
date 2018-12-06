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
    public partial class setduration : Form
    {
        public setduration()
        {
            InitializeComponent();
        }
        public DateTime GetDuration()
        {
            DateTime dur = DateTime.Parse(txbDuration.Text);
            return dur;                
        }
        public Int32 GetEspera()
        {
            Int32 espera = Convert.ToInt32(txbEspera);
            return espera;
        }
    }
}
