using NAudio.Wave;
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
    public partial class WaveAudio : Form
    {
        public string Mp3File;
        public WaveAudio()
        {
            InitializeComponent();
        }

        private void waveViewer1_Load(object sender, EventArgs e)
        {
            DrawMusic(Mp3File);
        }
        public void Mp3ToWav(string mp3File, string outputFile)
        {
            using (Mp3FileReader reader = new Mp3FileReader(mp3File))
            {
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    WaveFileWriter.CreateWaveFile(outputFile, pcmStream);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Música (*.mp3)|*.mp3";
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            DrawMusic(openFileDialog1.FileName);
        }
        public void DrawMusic(string Mp3File )
        {
            if (string.IsNullOrEmpty(this.Mp3File))
            { return;  }
            string waveFile = Mp3File.Replace(".mp3", ".wav");
            Mp3ToWav(Mp3File, waveFile);
            waveViewer1.SamplesPerPixel = 400;
            waveViewer1.StartPosition = 40000;

            waveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(waveFile);
        }
    }
}
