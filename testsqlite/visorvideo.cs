using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace biodanza
{
    public partial class visorvideo : Form
    {
        
        public visorvideo()
        {
            InitializeComponent();
        }

        private void visorvideo_Load(object sender, EventArgs e)
        {
            
        }

               
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
        }

        private void visorvideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }
    }
}
