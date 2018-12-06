using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace biodanza
{
    public partial class FrmCountDown : Form
    {
        public int WaitSeconds = 0;
        public FrmCountDown()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void FrmCountDown_Load(object sender, EventArgs e)
        {

            
        }

        private void FrmCountDown_Shown(object sender, EventArgs e)
        {
            System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();
            pfc.AddFontFile(Path.Combine(Application.StartupPath, "digital-7.ttf"));
            label1.Font = new Font(pfc.Families[0], 170, FontStyle.Regular);

            for (int i = WaitSeconds; i > 0; i--)
            {
                label1.Text = i.ToString();
                Application.DoEvents();
                Thread.Sleep(1000);
            }
            Close();
        }
    }
}
