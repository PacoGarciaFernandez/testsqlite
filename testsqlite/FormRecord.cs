using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace biodanza
{
    public partial class FormRecord : Form
    {
        Stopwatch stopWatch = null;
        LoopbackRecorder recorder;
        string tmpfilename;
        System.Windows.Forms.Timer ti;



        public FormRecord()
        {
            InitializeComponent();
            recorder = new LoopbackRecorder();
            stopWatch = new Stopwatch();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {

            
            for (int i = 3; i>0; i--)
            {
                txtTime.Text = "-" + i.ToString();
                Application.DoEvents();
                Thread.Sleep(1000);
            }


            ti.Tick += new EventHandler(OnTimedEvent);
            ti.Interval = 1000;
            ti.Enabled = true;
            stopWatch.Start();
            btnRecord.Visible = false;
            btnStop.Visible = true;
            this.tmpfilename = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".wav";
            recorder.StartRecording(tmpfilename);
        }

        private  void OnTimedEvent(object source, EventArgs e)
        {
            txtTime.Text = stopWatch.Elapsed.ToString(@"mm\:ss");
        }

        private void RecordForm_Load(object sender, EventArgs e)
        {
            ti = new System.Windows.Forms.Timer();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
       
            stopWatch.Stop();
            btnStop.Visible = false;
            btnRecord.Visible = true;
            recorder.StopRecording();

            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string newFileName = saveFileDialog1.FileName;

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.FileName = Application.StartupPath + @"\lame.exe";
                psi.Arguments = "-b" + "128" + " --resample " + "22.05" + " -m j " +
                               "\"" + tmpfilename + "\"" + " " +
                                "\"" + newFileName + ".mp3" + "\"";
                Process p = Process.Start(psi);
                p.WaitForExit();
                p.Close();
                p.Dispose();
            }
            System.IO.File.Delete(tmpfilename);
            Close();
            ti.Stop();
        }
    }
}
