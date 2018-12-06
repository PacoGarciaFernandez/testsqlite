using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace biodanza
{
    public partial class Player : Form
    {
        public Player()
        {
            InitializeComponent();
        }
        public void SetMusic(string cFileName)
        {
            this.wmPlayer.URL = cFileName;
        }
        public void PlayList()
        {
            WMPLib.IWMPPlaylist playlist = this.wmPlayer.playlistCollection.newPlaylist("myplaylist");
            WMPLib.IWMPMedia media;
            if (ofdSong.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofdSong.FileNames)
                {
                    media = this.wmPlayer.newMedia(file);
                    playlist.appendItem(media);
                }
            }
            this.wmPlayer.currentPlaylist = playlist;
            this.wmPlayer.CurrentItemChange += WmPlayer_CurrentItemChange;// PlaylistCollectionChange += WmPlayer_PlaylistCollectionChange;
            this.wmPlayer.Ctlcontrols.play();
            this.Show();

        }

        private void WmPlayer_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e)
        {
            wmPlayer.Ctlcontrols.play();
        }

        private void WmPlayer_PlaylistCollectionChange(object sender, EventArgs e)
        {
            
            //throw new NotImplementedException();
        }

        private void wmPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8 )
            {
                
                
            }
        }
    }
}
