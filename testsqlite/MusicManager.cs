using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;
using System.Net;
using System.Collections.Specialized;
using System.Speech.Synthesis;
using System.Collections;
using System.Runtime.InteropServices;
using System.Timers;
using System.Drawing.Drawing2D;
using ShellID3Reader;
using TagLib.Id3v2;
using TagLib;
using NAudio.Wave;

namespace biodanza
{
    public partial class MusicManager : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 2;



        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }
        enum modobusqueda
        {
            buscando_principio_cancion = 0,
            buscando_datos_cancion = 1,
            buscando_identificador = 2,
            leyendo_cancion = 3,
            fin_cancion = 4,
            finbusqueda=5,
            buscando_playlists=6,
            buscando_principiolista=7,
            buscando_idsCanciones=8,
            buscando_playlistitems=9
        };
        enum busqueda
        {
            canciones=0,
            listas=1
        };
        busqueda buscando = busqueda.listas;
        enum PlayState
        {
            Undefined=0,
            Stopped=1,
            Paused=2,
            Playing=3,
            ScanForward=4,
            ScanReverse=5,
            Buffering=6,
            Waiting=7,
            MediaEnded=8,
            Transitioning=9,
            Ready=10,
            Reconnecting=11,
            Recording=12
        };

        private PlayState program_state = PlayState.Stopped;

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;

        List<string> m_paths = new List<string>();

        SpeechSynthesizer reader;
        //PromptBuilder cultureSpain;

        
        bool isSound = false;
        Thread th1 = null;
        LoopbackRecorder recorder;
        string tmpfilename = string.Empty;
        bool isRecording = false;
        Stopwatch stopWatch = null;
        DateTime durationRec = new DateTime();
        Int32 tiempoespera = 0;
        TreeNode selected = null;
        //private string dragedItemText;

        ArrayList grids = new ArrayList();
        List<bool> configuredgrids = new List<bool>();
        CarpetaListaMng pm = new CarpetaListaMng();
        ItemMng item = new ItemMng();
        MusicaMng mm = new MusicaMng();
        RepetidosMng repes = new RepetidosMng();
       
        DataGridView curGrid = null;

        private Int32 LastClassUsed;

        private TreeNode nodeMusic;
        private TreeNode nodeProyect;
        private TreeNode nodeCuentos;

        private Int32 curRow = 0;
        private DataGridViewColumn curCol = null;
        
        Dictionary<Int32, TreeNode> carpetas;

        private bool isInLoop = false;
        private bool breakLoop = false;

        public MusicManager()
        {
            InitializeComponent();
            carpetas = new Dictionary<int, TreeNode>();

            grids.Add(gridItems);                //grids.Add(gridClases);
            
            grids.Add(gridMusica);
            grids.Add(gridRepetidos);            //grids.Add(gridClaseDef);

            //configuredgrids.Add(true);
            //configuredgrids.Add(false);
            configuredgrids.Add(false);
            configuredgrids.Add(false);
            configuredgrids.Add(false);
            configuredgrids.Add(false);

            reader = new SpeechSynthesizer();

            //LoadDeepFolder(@"c:\Users\francisco.garcia\Music");

            this.LoadTree();
            
        }


        private void MusicManager_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            tvListas.SelectedNode = tvListas.Nodes[0];
            trackBar1.ButtonSize = new Size(15, 15);
            m_paths.Add(@"C:\Users\francisco.garcia\Music\Romantica");
            wmPlayer.uiMode = "invisible";
            stopWatch = new Stopwatch();
            
            trackBar2.Value = wmPlayer.settings.volume;
            if (Properties.Settings.Default.size_MusicManager != Size.Empty)
            {
                Location = Properties.Settings.Default.location_MusicManager;
                Size = Properties.Settings.Default.size_MusicManager;
            }

        }

       
        //private void ConfigureGridClases()
        //{
        //    gridClases.RowHeadersVisible = false;
        //    gridClases.CellBorderStyle = DataGridViewCellBorderStyle.None;

        //    gridClases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //    gridClases.CellBorderStyle = DataGridViewCellBorderStyle.None;
        //    gridClases.RowHeadersVisible = false;

        //    gridClases.Columns[0].Visible = false;
        //    gridClases.Columns[1].Visible = false;
        //    gridClases.Columns[2].Visible = false;
        //    gridClases.Columns[3].Visible = false;
        //    gridClases.Columns[4].Visible = false;
        //    gridClases.Columns[5].HeaderText = "Nombre Clase";
        //    gridClases.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        //    gridClases.RowsDefaultCellStyle.BackColor = Color.White;

        //    gridClases.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245,245,245);

        //}
        private void ConfigurarGridMusica()
        {
            gridMusica.RowHeadersVisible = false;
            gridMusica.CellBorderStyle = DataGridViewCellBorderStyle.None;

            gridMusica.Columns["id"          ].Visible = false;
            gridMusica.Columns["repetir"     ].Visible = false;
            gridMusica.Columns["tipoitem"    ].Visible = false;
            gridMusica.Columns["localizacion"].Visible = false;
            gridMusica.Columns["idioma"      ].Visible = true;
 
            gridMusica.Columns["joincolumns" ].Visible = false;

            if (!gridMusica.Columns.Contains("loop"))
            {
                DataGridViewImageColumn img = new DataGridViewImageColumn();
                img.Name = "loop";
                img.DefaultCellStyle.SelectionBackColor = this.gridMusica.DefaultCellStyle.BackColor;
                img.DefaultCellStyle.SelectionForeColor = this.gridMusica.DefaultCellStyle.ForeColor;
                
                this.gridMusica.Columns.Add(img);
            }

            gridMusica.Columns["loop"].Visible = false;
            gridMusica.Columns["loop"].DisplayIndex = 0;
            gridMusica.Columns["loop"].HeaderText = "Rep.";
            gridMusica.Columns["loop"].Width = 40;

            gridMusica.Columns["titulo"].Visible = true;
            gridMusica.Columns["titulo"].DisplayIndex = 1;
            gridMusica.Columns["titulo"].HeaderText = "Nombre";
            gridMusica.Columns["titulo"].Width = 250;
            gridMusica.Columns["titulo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridMusica.Columns["duracion"].Visible = true;
            gridMusica.Columns["duracion"].DisplayIndex = 2;
            gridMusica.Columns["duracion"].HeaderText = "Duración";
            gridMusica.Columns["duracion"].Width = 90;
            gridMusica.Columns["duracion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridMusica.Columns["duracion"].DefaultCellStyle.Format = "HH:mm:ss";

            gridMusica.Columns["autor"].Visible = true;
            gridMusica.Columns["autor"].DisplayIndex = 3;
            gridMusica.Columns["autor"].HeaderText = "Artista";
            gridMusica.Columns["autor"].Width = 180;
            gridMusica.Columns["autor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridMusica.Columns["album"].Visible = true;
            gridMusica.Columns["album"].DisplayIndex = 4;
            gridMusica.Columns["album"].HeaderText = "Album";
            gridMusica.Columns["album"].Width = 250;
            gridMusica.Columns["album"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridMusica.Columns["genero"].Visible = true;
            gridMusica.Columns["genero"].DisplayIndex = 5;
            gridMusica.Columns["genero"].HeaderText = "Genero";
            gridMusica.Columns["genero"].Width = 250;
            gridMusica.Columns["genero"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridMusica.Columns["idioma"].Visible = true;
            gridMusica.Columns["idioma"].DisplayIndex = 6;
            gridMusica.Columns["idioma"].HeaderText = "Idioma";
            gridMusica.Columns["idioma"].Width = 150;
            gridMusica.Columns["idioma"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if (!gridMusica.Columns.Contains("lyric"))
            {
                DataGridViewImageColumn img2 = new DataGridViewImageColumn();
                img2.Name = "lyric";
                img2.DefaultCellStyle.SelectionBackColor = this.gridMusica.DefaultCellStyle.BackColor;
                img2.DefaultCellStyle.SelectionForeColor = this.gridMusica.DefaultCellStyle.ForeColor;

                this.gridMusica.Columns.Add(img2);
            }

        }
        
        private void ConfigurarGridItems()
        {
            gridItems.RowHeadersVisible = false;
            gridItems.CellBorderStyle = DataGridViewCellBorderStyle.None;

            foreach (DataGridViewColumn c in gridItems.Columns)
            {
                if (c.Name != "ORDEN")
                {
                    c.ReadOnly = true;
                }
            }

            gridItems.Columns["id"].Visible = false;
            gridItems.Columns["id_carpetalista"].Visible = false;
            gridItems.Columns["id_item"].Visible = false;
            gridItems.Columns["itemid"].Visible = false;
            gridItems.Columns["orden"].Visible = false;
            gridItems.Columns["desde"].Visible = true;
            gridItems.Columns["hasta"].Visible = true;

            gridItems.Columns["localizacion"].Visible = false;

            gridItems.Columns["repetir"].Visible = false;
            gridItems.Columns["repetir"].Width = 40;
            gridItems.Columns["repetir"].HeaderText = "Rep.";
            gridItems.Columns["repetir"].DisplayIndex = 4;
            gridItems.Columns["repetir"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if (!gridItems.Columns.Contains("loop"))
            {
                DataGridViewImageColumn img = new DataGridViewImageColumn();
                img.Name = "loop";
                //img.DefaultCellStyle.SelectionBackColor = this.gridItems.DefaultCellStyle.BackColor;
                //img.DefaultCellStyle.SelectionForeColor = this.gridItems.DefaultCellStyle.ForeColor;
                this.gridItems.Columns.Add(img);
            }
            gridItems.Columns["loop"].Visible = true;
            gridItems.Columns["loop"].DisplayIndex = 0;
            gridItems.Columns["loop"].HeaderText = "Rep.";
            gridItems.Columns["loop"].Width = 40;

            gridItems.Columns["ORDEN"].Width = 50;
            gridItems.Columns["ORDEN"].HeaderText = "Orden";
            gridItems.Columns["ORDEN"].DisplayIndex = 0;
            gridItems.Columns["ORDEN"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItems.Columns["TITULO"].Width = 250;
            gridItems.Columns["TITULO"].HeaderText = "Nombre";
            gridItems.Columns["TITULO"].DisplayIndex = 1;
            gridItems.Columns["TITULO"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItems.Columns["TIPOITEM"].Visible = false;
            gridItems.Columns["TIPOITEM"].HeaderText = "Tipo";
            gridItems.Columns["TIPOITEM"].Width = 40;
            gridItems.Columns["TIPOITEM"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItems.Columns["DURACION"].Width = 90;
            gridItems.Columns["DURACION"].HeaderText = "Duración";
            gridItems.Columns["DURACION"].DisplayIndex = 2;
            gridItems.Columns["DURACION"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridItems.Columns["duracion"].DefaultCellStyle.Format = "HH:mm:ss";

            gridItems.Columns["AUTOR"].Width = 180;
            gridItems.Columns["AUTOR"].HeaderText = "Artista";
            gridItems.Columns["AUTOR"].DisplayIndex = 3;
            gridItems.Columns["AUTOR"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItems.Columns["album"].Visible = true;
            gridItems.Columns["album"].DisplayIndex = 5;
            gridItems.Columns["album"].HeaderText = "Album";
            gridItems.Columns["album"].Width = 250;
            gridItems.Columns["album"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            

            gridItems.Columns["GENERO"].Width = 250;
            gridItems.Columns["GENERO"].HeaderText = "Género";
            gridItems.Columns["GENERO"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItems.Columns["desde"].Visible = true;
            gridItems.Columns["desde"].DisplayIndex = 7;
            gridItems.Columns["desde"].HeaderText = "Empieza";
            gridItems.Columns["desde"].Width = 70;
            gridItems.Columns["desde"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItems.Columns["hasta"].Visible = true;
            gridItems.Columns["hasta"].DisplayIndex = 8;
            gridItems.Columns["hasta"].HeaderText = "Termina";
            gridItems.Columns["hasta"].Width = 700;
            gridItems.Columns["hasta"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridItems.Columns["duracion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridItems.Columns["duracion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //gridCarpetaLista.RowsDefaultCellStyle.BackColor = Color.White;
            //gridCarpetaLista.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            

        }
        private void ConfigurarGridRepetidos()
        {
            gridRepetidos.RowHeadersVisible = false;
            gridRepetidos.CellBorderStyle = DataGridViewCellBorderStyle.None;
            gridRepetidos.Columns[0].Visible = false;
            gridRepetidos.Columns[1].Width = 40;
            gridRepetidos.Columns[2].Width = 300;
            gridRepetidos.Columns[3].Width = 500;

            gridRepetidos.RowsDefaultCellStyle.BackColor = Color.White;
            gridRepetidos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

        }
        void gridMusica_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
            if (e.RowIndex > -1 && gridMusica.Columns[e.ColumnIndex].Name == "loop")
            {
                // Your code would go here - below is just the code I used to test 
                
                var data = gridMusica.Rows[e.RowIndex].Cells["repetir"].Value;
                if (data != DBNull.Value && Convert.ToInt32(data) == 0)
                {
                    e.Value = Properties.Resources.repetiroff;
                }
                else
                {
                   e.Value = Properties.Resources.if_replay_326591;
                }
            }
            if (e.RowIndex > -1 && gridMusica.Columns[e.ColumnIndex].Name == "lyric")
            {
                // Your code would go here - below is just the code I used to test 

                var data = gridMusica.Rows[e.RowIndex].Cells["lyric"].Value;
                if (data != DBNull.Value && Convert.ToInt32(data) == 0)
                {
                    e.Value = Properties.Resources.repetiroff;
                }
                else
                {
                    e.Value = Properties.Resources.lyric;
                }
            }
            //if (gridMusica.Columns[e.ColumnIndex].Name == "perdido")
            //{
            //    if( Convert.ToBoolean(e.Value) )
            //    {
            //        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Strikeout);
            //    }
            //}

        }

        void gridItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                if(gridItems.Columns[e.ColumnIndex].Name == "loop")
                {
                        // Your code would go here - below is just the code I used to test 
                        var data = gridItems.Rows[e.RowIndex].Cells["repetir"].Value;
                        if (data != DBNull.Value && Convert.ToInt32(data) == 0)
                        {
                            e.Value = Properties.Resources.repetiroff;
                        }
                        else
                        {
                            e.Value = Properties.Resources.if_replay_326591;
                        }
                    }
                }

        }


        private void HideShowColumns(object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            string text = mi.Text;
            mi.Checked = !mi.Checked;
            gridMusica.Columns[text].Visible = mi.Checked;
        }

        private void LoadTree()
        {
            carpetas.Clear();

            pm.GetData();
            pm.Filter("borrada=0");
            item.GetData();
            mm.GetData();
            //lc.GetData();

            // Buscar el nodo "Proyectos"
            nodeMusic = GetNodeByText(tvListas.Nodes, "Música");
            nodeMusic.Nodes.Clear();
            nodeMusic.Tag = new Tuple<string, Int32>("musica", 0);
            
            nodeProyect = GetNodeByText(tvListas.Nodes, "Proyectos");
            nodeProyect.Nodes.Clear();
            nodeProyect.Tag = new Tuple<string, Int32>("musica", 1);
            nodeProyect.ContextMenu = AddContextMenuProyectos(true);

            LoadGridMusica();

            Int32 procesados = 0;

            List<CarpetaLista> lps = pm.Lista();
            Int32 total = lps.Count();

            // Cargo primero las carpetas

            TreeNode padre = null;

            foreach (CarpetaLista cl in lps)
            {
                if (cl.tipo == 0)
                {
                    if (cl.id_carpetalista != 0)   // subcarpeta
                    {
                        padre = carpetas[cl.id_carpetalista];
                    }
                    else
                    {
                        padre = nodeProyect;
                    }

                    TreeNode tn = padre.Nodes.Add(cl.titulo);
                    tn.SelectedImageIndex = 2;
                    tn.ImageIndex = 2;
                    tn.Tag = new Tuple<string, Int32>("carpeta", cl.id);
                    tn.ContextMenu = AddContextMenuProyectos(true);
                    carpetas.Add(cl.id, tn);
                }
            }

            // Cargamos las calses
            foreach (CarpetaLista cl in lps)
            {

                if (cl.tipo == 0)
                    continue;
                if (cl.id_carpetalista == 0)
                {
                    padre = nodeProyect;
                }
                else
                {
                    if (carpetas.ContainsKey(cl.id_carpetalista))
                    {
                        padre = carpetas[cl.id_carpetalista];
                    }
                    else
                        continue;
                }
                
                TreeNode tn = padre.Nodes.Add(cl.titulo);
                tn.SelectedImageIndex = 0;
                tn.ImageIndex = 0;
                tn.Tag = new Tuple<string, Int32>("lista", cl.id);
                tn.ContextMenu = AddContextMenuProyectos(false);
            }
            nodeProyect.Expand();
        }
        private void LoadGridRepetidos()
        {
            repes.Attach(gridRepetidos);
            ConfigurarGridRepetidos();
            this.ShowGrid(gridRepetidos);
        }
        private void LoadGridMusica()
        {
            mm.GetData();
            mm.Attach(gridMusica);
            ConfigurarGridMusica();
            this.ShowGrid(gridMusica);
        }
        
        //private void LoadGridClases(Int32 proyecto)
        //{
        //    ListaClasesProMng pm = new ListaClasesProMng();
        //    pm.Filter("id_proyecto =" + proyecto.ToString());
        //    pm.Attach(gridClases);
        //    ConfigureGridClases();
        //    this.ShowGrid(gridClases);

        //}
        //private void LoadGridClaseDef()
        //{
        //    ListaClasesMng pm = new ListaClasesMng();
        //    pm.Attach(gridClaseDef);
        //    ConfigureGridClaseDef();
        //    this.ShowGrid(gridClaseDef);
            
        //}
        private void LoadGridItems(Int32 clase)
        {
            ListaItemsMng lim = new ListaItemsMng();
            lim.Filter("ID_CARPETALISTA=" + clase.ToString());
            lim.Sort("ORDEN ASC");
            lim.Attach(gridItems);
            ConfigurarGridItems();
            this.ShowGrid(gridItems);

        }
        private void ShowGrid(DataGridView grid)
        {
            foreach(DataGridView g in grids)
            {
                if(g.Name == grid.Name)
                { g.Show(); }
                else
                { g.Hide(); }
            }
            curGrid = grid;
        }
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //Int32 i = gridMusica.SelectedCells[0].RowIndex;
            Int32 i = curGrid.SelectedCells[0].RowIndex;
            string file = Path.Combine(gridMusica.Rows[i].Cells["localizacion"].Value.ToString(), gridMusica.Rows[i].Cells["Titulo"].Value.ToString());
            Player p = new Player();
            p.SetMusic(file);
            p.Show();
        }
        private void gridItems_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void wmPlayer_PlayStateChange_1(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {

            Int32 i = curGrid.SelectedCells[0].RowIndex;
            bool repetir = false;
            bool clase = curGrid == gridItems;

            System.IO.File.AppendAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "estados.log"), e.newState.ToString()+Environment.NewLine);
            
            
            if (curGrid.Columns.Contains("repetir"))
            {
                repetir = Convert.ToBoolean(curGrid.Rows[i].Cells["repetir"].Value) && curGrid.Columns["repetir"].Visible;
            }

            if (e.newState == (int)PlayState.Playing)
            {
                trackBar2.Value = (int)wmPlayer.Ctlcontrols.currentPosition;
                btnPlay.Image = Properties.Resources.if_icon_pause_211871;// if_067_Pause_183196;
                isSound = true;
                txtCancion.Text = curGrid.Rows[i].Cells["titulo"].Value.ToString();
                txtAutor_Album.Text = curGrid.Rows[i].Cells["autor"].Value.ToString() + " - " + curGrid.Rows[i].Cells["album"].Value.ToString();
            }
            else if (e.newState == (int)PlayState.Stopped)
            {
                
                btnPlay.Image = Properties.Resources.if_play_arrow_326577_1_;
                isSound = false;
                program_state = PlayState.Stopped;
                
                Application.DoEvents();
                //
                //Application.DoEvents();

            }
            else if (e.newState == (int)PlayState.MediaEnded)
            {
                trackBar2.Value = (int)wmPlayer.Ctlcontrols.currentPosition;
                btnPlay.Image = Properties.Resources.if_play_arrow_326577_1_;
                isSound = false;
                program_state = PlayState.Stopped;
                th1.Suspend();

            }
            
        }
        private void gridCanciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            wmPlayer.Ctlcontrols.stop();
            trackBar2.Value = 0;
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            btnPlay.Visible = true;
            btnPause.Visible = false;
            program_state = PlayState.Stopped;
            stopWatch.Stop();
            wmPlayer.Ctlcontrols.pause();
            if (reader.State == SynthesizerState.Speaking)
            {
                reader.Pause();
                reader.SpeakAsyncCancelAll();
            }
            if (th1 != null && th1.IsAlive)
            {
                th1.Suspend();
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            
            btnPlay.Visible = false;
            btnPause.Visible = true;
            panelLogo.Visible = false;
            panelPlay.Visible = true;

            if (curGrid.SelectedCells.Count == 0)
                return;

            Int32 i = curGrid.SelectedCells[0].RowIndex;
            if (i < 0 || !curGrid.Columns.Contains("localizacion") )
                return;
            bool modeloop = false;

            if (curGrid.Columns.Contains("Repetir"))
            {
                //modeloop = Convert.ToBoolean(curGrid.Rows[i].Cells["Repetir"].Value);
            }


            string file = string.Empty;
            bool cuento = false;
            string letra = string.Empty;
            try
            {
                // Uri uriAddress = new Uri(Path.Combine(curGrid.Rows[i].Cells["localizacion"].Value.ToString(), curGrid.Rows[i].Cells["Titulo"].Value.ToString()));
                string titulo = curGrid.Rows[i].Cells["titulo"].Value.ToString();

                cuento = titulo.ToUpper().Contains("(CUENTO)");

                //Uri uriAddress = new Uri(curGrid.Rows[i].Cells["localizacion"].Value.ToString());
                //file = uriAddress.ToString().Replace(@"file:///", "");
                file = curGrid.Rows[i].Cells["localizacion"].Value.ToString();


                TagLib.File y = null;
                var id3 = y;
                try
                {
                    id3 = TagLib.File.Create(file);
                }
                catch { }

                if (id3 != null && !string.IsNullOrEmpty(id3.Tag.Lyrics))
                {
                    letra = id3.Tag.Lyrics;                    
                }

            }
            catch
            {
                return;
            }
            

            //wmPlayer.settings.setMode("loop", modeloop);

            if (curGrid.Columns.Contains("duracion"))
            {
                
                string duration = (curGrid.Rows[i].Cells["duracion"].Value.ToString());
                if (duration.Contains(":"))
                {
                    TimeSpan tiempo = TimeSpan.Parse("00:" + duration);
                    trackBar2.Maximum = (int)tiempo.TotalSeconds;
                }
                else
                {
                    trackBar2.Maximum = Convert.ToInt32(duration)/1000;
                }
            }

            if (program_state == PlayState.Playing )
            {
                program_state = PlayState.Stopped;
                stopWatch.Stop();
                wmPlayer.Ctlcontrols.stop();
                if (th1 != null && th1.IsAlive )
                {
                    th1.Suspend();
                }

            }

            stopWatch.Start();
            program_state = PlayState.Playing;
            if (th1 == null)
            {
                th1 = new Thread(new ThreadStart(UpdateLabelThreadProc));
                th1.Start();
            }
            else
            {
                if (th1.IsAlive)
                {
                    try
                    {
                        th1.Resume();
                    }
                    catch
                    {
                        th1.Suspend();
                    }
                }
            }

            
            if (wmPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                wmPlayer.Ctlcontrols.play();
            }
            else
            {
                wmPlayer.URL = file;
                wmPlayer.Ctlcontrols.play();
            }
            

            if (cuento && !string.IsNullOrEmpty(letra))
            {

                Hablar(letra,-1);
            }
            
            
            UpdatePositionPlayer();
        }
        void UpdateLabelThreadProc()
        {

            while (isRecording || program_state == PlayState.Playing)
            {
                this.BeginInvoke(new MethodInvoker(UpdateLabel));
                System.Threading.Thread.Sleep(1000);
            }
        }
        private void UpdateLabel()
        {
            if (isRecording || program_state == PlayState.Playing)
            {
                
                txtResta.Text = wmPlayer.Ctlcontrols.currentPositionString;
                TimeSpan t = TimeSpan.FromSeconds(trackBar2.Maximum - Convert.ToInt32(wmPlayer.Ctlcontrols.currentPosition));

                txtDuracion.Text = t.ToString(@"mm\:ss");
               
                trackBar2.Value = (int)wmPlayer.Ctlcontrols.currentPosition;
                //txtResta.Text = wmPlayer.Ctlcontrols.currentPosition.ToString();
            }

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            
            wmPlayer.settings.volume = trackBar1.Value;
        }
        private void wmPlayer_PositionChange(object sender, AxWMPLib._WMPOCXEvents_PositionChangeEvent e)
        {
            trackBar2.Value = (int)wmPlayer.Ctlcontrols.currentPosition;
            txtResta.Text = wmPlayer.Ctlcontrols.currentPosition.ToString();
        }
        private void wmPlayer_Enter(object sender, EventArgs e)
        {

        }
        private void wmPlayer_StatusChange(object sender, EventArgs e)
        {

        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            
        }
        private void trackBar2_LocationChanged(object sender, EventArgs e)
        {
            //wmPlayer.Ctlcontrols.currentPosition = trackBar2.Value;
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
        }
        private void trackBar2_MouseCaptureChanged(object sender, EventArgs e)
        {
            wmPlayer.Ctlcontrols.currentPosition = trackBar2.Value;
        }
        private void UpdatePositionPlayer()
        {
            program_state = PlayState.Playing;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {

            curGrid.Focus();
            if (program_state  == PlayState.Playing)
            {
                wmPlayer.Ctlcontrols.stop();
            }
            curGrid.Focus();
            SendKeys.Send("{UP}");
            Application.DoEvents();
            //Thread.Sleep(200);
            btnPlay.PerformClick();
        }
        private void btnRight_Click(object sender, EventArgs e)
        {

            if (isInLoop)
            {
                if (wmPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    wmPlayer.Ctlcontrols.stop();
                }
                breakLoop = true;
                return;
            }

            GoNextRow();
            wmPlayer.Ctlcontrols.stop();
            btnPlay_Click(null, null);

        }
 
        private void btnRecord_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            FormRecord fr = new FormRecord();
            fr.ShowDialog();
            this.WindowState = FormWindowState.Normal;

        }
        private void btnStop_click(object sender, EventArgs e)
        {
            isRecording = false;

            stopWatch.Stop();
            btnStop.Visible = false;
            btnRecord.Visible = true;
            recorder.StopRecording();
            th1.Abort();

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

        }
        public void Song(string filePath)
        {
            var id3 = TagLib.File.Create(filePath);

            string[] Artist = id3.Tag.Performers;
            //id3.Tag.Artists[0] = "Paco";
           
        }
        private Bitmap LoadPicture(TagLib.File file)
        {
            Image currentImage = null;
            if (file.Tag.Pictures.Length > 0)
            {
                TagLib.IPicture pic = file.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                if (ms.Length > 0)
                    currentImage = Image.FromStream(ms);
                ms.Close();
                currentImage.Save(Path.Combine(Application.ExecutablePath,"images")+ file );
            }

            return (Bitmap)currentImage;
        }
        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node != null)
            {
                string tipo = string.Empty;
                Int32 id = 0;

                if (e.Node.Tag != null)
                {
                    tipo = ((Tuple<string, Int32>)e.Node.Tag).Item1;
                    id = ((Tuple<string, Int32>)e.Node.Tag).Item2;
                }

                if (tipo == "lista")
                {
                    
                    LoadGridItems(id);
                }
                else if( tipo == "carpeta")
                {
                    e.Node.SelectedImageIndex= (e.Node.IsExpanded ? 3 : 2);
                }
                //else if (tipo == "proyecto")
                //{
                //    LoadGridClases(id);
                //}
                else if (tipo == "proyectos")
                {
                    LoadGridMusica();
                }
                else if (tipo == "musica")
                {
                    LoadGridMusica();
                }
                //else if (tipo == "clasedef")
                //{
                //    LoadGridClaseDef();
                //}

            }
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "toolStripCrearClase")
            {
                MessageBox.Show("revisar");
                return;
                //TreeNode node = new TreeNode("Nueva clase");
                //this.selected.Nodes.Add(node);
                //this.selected.Expand();
                //node.Tag = CrearClase().ToString();
            }
        }
        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.selected = e.Node;
        }
        //private void CrearClase(Int32 idProyecto=-1)
        //{
        //    AddClase adc = new AddClase();
        //    if (adc.ShowDialog() == DialogResult.OK)
        //    {
        //        LoadTree();
        //    }
        //}
        private TreeNode GetNodeByName(TreeNodeCollection nodes, string searchtext)
        {
            TreeNode n_found_node = null;
            bool b_node_found = false;

            foreach (TreeNode node in nodes)
            {

                if (node.Name == searchtext)
                {
                    b_node_found = true;
                    n_found_node = node;

                    return n_found_node;
                }

                if (!b_node_found)
                {
                    n_found_node = GetNodeByName(node.Nodes, searchtext);

                    if (n_found_node != null)
                    {
                        return n_found_node;
                    }
                }
            }
            return null;
        }
        private TreeNode GetNodeByText(TreeNodeCollection nodes, string searchtext)
        {
            TreeNode n_found_node = null;
            bool b_node_found = false;

            foreach (TreeNode node in nodes)
            {

                if (node.Text == searchtext)
                {
                    b_node_found = true;
                    n_found_node = node;

                    return n_found_node;
                }

                if (!b_node_found)
                {
                    n_found_node = GetNodeByText(node.Nodes, searchtext);

                    if (n_found_node != null)
                    {
                        return n_found_node;
                    }
                }
            }
            return null;
        }
        private TreeNode GetNodeById(TreeNodeCollection nodes, Int32 id)
        {
            TreeNode n_found_node = null;
            bool b_node_found = false;

            foreach (TreeNode node in nodes)
            {
                Int32 idNode = ((Tuple<string, Int32>)node.Tag).Item2;

                if (idNode == id)
                {
                    b_node_found = true;
                    n_found_node = node;

                    return n_found_node;
                }

                if (!b_node_found)
                {
                    n_found_node = GetNodeById(node.Nodes, id);

                    if (n_found_node != null)
                    {
                        return n_found_node;
                    }
                }
            }
            return null;
        }
        private string SecurePath( string path )
        {
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }
            return path;
        }
        private void LoadDeepFolderEx(string sDir)
        {
            string file = Path.GetTempFileName();

            sDir = SecurePath(sDir);

            System.IO.File.WriteAllText("myrun.bat", @"dir " + sDir + "*.mp3 /s/b > " + file);
            
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardInput = true,
                Arguments = "/c myrun.bat"
            };
            Process proc = new Process() { StartInfo = psi };

            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            proc.Close();

            string[] lines = System.IO.File.ReadAllLines(file, Encoding.GetEncoding(850));

            Dictionary<string, ArrayList> dups = new Dictionary<string, ArrayList>();

            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding(850)))
            {
                while (sr.Peek() >= 0)
                {
                    string f = sr.ReadLine();
                    try
                    {
                        FileInfo fi = new FileInfo(f);
                        if (!item.ExisteCancion(Path.GetFileName(fi.FullName)))
                        {
                            this.item.Nuevo(fi);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        private void LoadDeepFolder(string sDir)
        {
            //try
            //{
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    try
                    {
                        FileInfo fi = new FileInfo(f);
                        if (!item.ExisteCancion(Path.GetFileName(fi.FullName)))
                        {
                            this.item.Nuevo(fi);
                        }
                    }
                    catch(Exception)
                    {

                    }

                }

                LoadDeepFolder(d);
            }
            
             
           // }
            /*catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }*/
        }
        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearch_Click(this, new EventArgs());
            }
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string[] words = textBoxSearch.Text.Split(' ');
            string tiposearch = "programa";

            if (!string.IsNullOrEmpty(textBoxSearch.Text.Trim()))
            {
                ToolStripMenuItem mi = GetItemcontextMenuSearchChecked();

                if (mi != null)
                {
                    tiposearch = mi.Tag.ToString();
                }

                if (tiposearch == "cancion" || words[0].ToLower() == "cancion")
                {
                    String searchRequest = "indexOf " + textBoxSearch.Text.Substring("cancion".Length);
                    System.Diagnostics.Process.Start("http://www.google.com.au/search?q=IndexOf " + System.Uri.EscapeDataString(searchRequest));
                    return;
                }
                else if (tiposearch == "video")

                {
                    String searchRequest = textBoxSearch.Text;
                    System.Diagnostics.Process.Start("https://www.google.com.au/search?q=" + System.Uri.EscapeDataString(searchRequest) + "&source=lnms&tbm=vid&sa=X&ved=0ahUKEwiZmZWRmfTaAhWKhaYKHbANBVYQ_AUICigB&biw=1920&bih=943");
                        //http://www.google.com.au/search?q=vídeo " + System.Uri.EscapeDataString(searchRequest));
                    return;
                }
                else if (tiposearch == "letra")
                {
                    String searchRequest = textBoxSearch.Text;
                    System.Diagnostics.Process.Start("http://www.google.com.au/search?q=Letra " + System.Uri.EscapeDataString(searchRequest));
                    return;
                }
            }
            string query = string.Empty;

            for(int i=0; i<words.Count();i++)
            {
                string palabra = words[i];
                palabra = palabra.Replace("+", " ");
                query += "joincolumns like '%" + palabra + "%'";
                if (i < words.Count() - 1)
                {
                    query += " and ";
                }
            }
            mm.Filter(query);
        }
        public static bool ShowFileProperties(string Filename)
        {
            TagLib.File y = null;
            var id3 = y;
            try
            {
                id3 = TagLib.File.Create(Filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }


            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = Filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            info.lpParameters = "Detalles";
            return ShellExecuteEx(ref info);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            reader.SpeakAsyncCancelAll();
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Int32 i = gridItems.SelectedCells[0].RowIndex;
            string file = gridMusica.Rows[i].Cells["localizacion"].Value.ToString();

            ListaItemsMng lim = new ListaItemsMng();
            lista_items li = new lista_items();
            li.id = Convert.ToInt32(gridItems.Rows[i].Cells["id"].Value);
            li.id_carpetalista = Convert.ToInt32(gridItems.Rows[i].Cells["id_carpetalista"].Value);
            li.id_item = Convert.ToInt32(gridItems.Rows[i].Cells["id_item"].Value);
            li.orden = Convert.ToInt32(gridItems.Rows[i].Cells["orden"].Value);
            li.repetir = Convert.ToInt32(gridItems.Rows[i].Cells["repetir"].Value);
            lim.Update(li);

            //MessageBox.Show(gridItems.Rows[i].Cells["ORDEN"].Value.ToString());


        }
        private Int32 NumLines(string file)
        {
            Int32 lineCount = 0;
            using (var reader = System.IO.File.OpenText(file))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return lineCount;
        }
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = string.Empty;
            modobusqueda estado = modobusqueda.buscando_principio_cancion;

            

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            { return; }

            file = openFileDialog1.FileName;

            trackBar2.Maximum = NumLines(file);
            trackBar2.Minimum = 0;
            int canciones = 0;
            using (StreamReader sr = new StreamReader(file))
            {
                string idCancion = string.Empty;
                string name = string.Empty;
                string trackid = string.Empty;
                string kind = string.Empty;
                string size = string.Empty;
                string duracion = string.Empty;
                string dateMod = string.Empty;
                string dateAdded = string.Empty;
                string bitrate = string.Empty;
                string localizacion = string.Empty;
                string Album = string.Empty;
                string Artist = string.Empty;
                string genero = string.Empty;
                string extension = string.Empty;

                int nEn = 0;
                int nLinea = 0;

                while (sr.Peek() >= 0)
                {
                    string linea = sr.ReadLine();
                    
                    trackBar2.Value = ++nLinea;

                    if (estado == modobusqueda.buscando_principio_cancion)
                    {
                        if ((nEn = linea.IndexOf("<key>Tracks")) != -1)
                        {
                            estado = modobusqueda.buscando_identificador;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (estado == modobusqueda.buscando_identificador)
                    {
                        if (( nEn = linea.IndexOf("<key>")) != -1)
                        {
                            nEn += "<key>".Length;
                            idCancion = linea.Substring(nEn, linea.IndexOf("</key>") - nEn);
                            estado = modobusqueda.buscando_datos_cancion;
                            continue;
                        }
                    }
                    else if(estado == modobusqueda.buscando_datos_cancion)
                       {
                        if ((nEn = linea.IndexOf("<key>Name</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<string>");
                            n1 += "<string>".Length;
                            int n2 = linea.IndexOf("</string>");
                            name = linea.Substring(n1, n2 - n1) ;
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Track ID</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<integer>");
                            n1 += "<integer>".Length;
                            int n2 = linea.IndexOf("</integer>");
                            trackid = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Kind</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<string>");
                            n1 += "<string>".Length;
                            int n2 = linea.IndexOf("</string>");
                            kind = linea.Substring(n1, n2 - n1);
                            if (kind == "Archivo de audio MPEG")
                            {
                                extension = ".mp3";
                            }
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Size</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<integer>");
                            n1 += "<integer>".Length;
                            int n2 = linea.IndexOf("</integer>");
                            size = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Total Time</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<integer>");
                            n1 += "<integer>".Length;
                            int n2 = linea.IndexOf("</integer>");
                            duracion = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Date Modified</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<date>");
                            n1 += "<date>".Length;
                            int n2 = linea.IndexOf("</date>");
                            dateMod = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Date Added</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<date>");
                            n1 += "<date>".Length;
                            int n2 = linea.IndexOf("</date>");
                            dateAdded = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Bit Rate</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<integer>");
                            n1 += "<integer>".Length;
                            int n2 = linea.IndexOf("</integer>");
                            bitrate = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Album</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<string>");
                            n1 += "<string>".Length;
                            int n2 = linea.IndexOf("</string>");
                            Album = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Artist</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<string>");
                            n1 += "<string>".Length;
                            int n2 = linea.IndexOf("</string>");
                            Artist = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Genre</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<string>");
                            n1 += "<string>".Length;
                            int n2 = linea.IndexOf("</string>");
                            genero = linea.Substring(n1, n2 - n1);
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Has Video</key>")) != -1)
                        {
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>HD</key>")) != -1)
                        {
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Video Width</key>")) != -1)
                        {
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Video Height</key>")) != -1)
                        {
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Location</key>")) != -1)
                        {
                            int n1 = linea.IndexOf("<string>");
                            n1 += "<string>".Length;
                            int n2 = linea.IndexOf("</string>");
                            localizacion = linea.Substring(n1, n2 - n1);
                            continue;

                        }
                        else if ((nEn = linea.IndexOf("<key>File Folder Count</key>")) != -1)
                        {
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Library Folder Count</key>")) != -1)
                        {
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("</dict>")) != -1)
                        {
                            Item it = new Item();
                            it.Id = int.Parse(trackid);
                            it.Album = Album;
                            it.Autor = Artist;
                            it.Duracion = duracion;
                            it.genero = genero;
                            it.tipo = "Música";
                            it.Titulo = name+extension;
                            it.Localizacion = localizacion;

                            item.Nuevo(it);
                            txtCanciones.Text = "Importada canción " + canciones.ToString();

                            estado = modobusqueda.buscando_identificador;
                            continue;
                        }
                        else if ((nEn = linea.IndexOf("<key>Playlists</key>")) != -1)
                        {
                            break;
                        }
                    }
                }
            }
        }
        private void cargarMúsicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            item.dt = item.GetData();
            LoadGridMusica();
        }
        private void borrarRepetidosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            repes.DeleteAll();
            // ExecuteCommand(); // LoadDeepFolder(@"c:\Users\francisco.garcia\Music", true);
            BuscaDuplicados(@"c:\Users\francisco.garcia\Music");
            
            LoadGridRepetidos();
        }
        public void BuscaDuplicados( string path)
        {
            string file = Path.GetTempFileName();

            path = SecurePath(path);

            System.IO.File.WriteAllText("myrun.bat", @"dir " + path + "*.mp3 /s/b > " + file);
            
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardInput = true,
                Arguments = "/c myrun.bat"
            };
            Process proc = new Process() { StartInfo = psi };

            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            proc.Close();

            string[] lines = System.IO.File.ReadAllLines(file, Encoding.GetEncoding(850));

            Dictionary<string, ArrayList> dups = new Dictionary<string, ArrayList>();

            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding(850)))
            {
                while (sr.Peek() >= 0)
                {
                    string linea    = sr.ReadLine();
                    string filename = Path.GetFileName(linea);
                    string title    = string.Empty;

                    if (dups.ContainsKey(filename))
                    {
                        dups[filename].Add(linea);
                    }
                    else
                    {
                        ArrayList files = new ArrayList();
                        files.Add(linea);
                        dups.Add(filename, files);
                    }
                }
            }
            int id = 0;
            foreach (KeyValuePair<string, ArrayList> entry in dups)
            {
                if (entry.Value.Count > 1)
                {
                    foreach (string fichero in entry.Value)
                    {
                        id++;
                        DataRow row = repes.dt.NewRow();
                        row["id"] = id;
                        row["localizacion"] = Path.GetDirectoryName(fichero);
                        row["filename"] = Path.GetFileName(fichero);
                        row["titulo"] = Path.GetFileName(fichero);

                        TagLib.File y = null;
                        var id3 = y;

                        try
                        {
                            id3 = TagLib.File.Create(fichero);
                            row["Autor"] = id3.Tag.FirstArtist;
                        }
                        catch (Exception)
                        {
                            
                        }
                        repes.dt.Rows.Add(row);
                    }
                }
            }
            


        }
        private void loadRepetidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadGridRepetidos();
        }
        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 i = curGrid.SelectedCells[0].RowIndex;
            //string file = Path.Combine(curGrid.Rows[i].Cells["localizacion"].Value.ToString(), curGrid.Rows[i].Cells["Titulo"].Value.ToString());
            string file = curGrid.Rows[i].Cells["localizacion"].Value.ToString();
            string fileName = Path.GetFileNameWithoutExtension(file);
            string className = "#32770";

                if(!ShowFileProperties(file))
                { return; }
         
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = gridMusica.SelectedRows;

            foreach (DataGridViewRow ro in rows)
            {

            }


            //ListaDeClases ldc = new ListaDeClases();
            //ldc.ShowDialog();
        }
        private void gridMusica_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;

            if (curCol == curGrid.Columns["loop"])
            {
                Int32 i = grid.SelectedCells[0].RowIndex;
                grid.Rows[i].Cells["repetir"].Value = Convert.ToInt32(grid.Rows[i].Cells["repetir"].Value) == 0 ? 1 : 0;
            }
            else
            {
                btnPlay_Click(sender, null);
            }
        }

        private void gridItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            curRow = grid.SelectedCells[0].RowIndex;

            btnPlay_Click(sender, null);
        }
            
        private void gridMusica_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (gridMusica.Rows[e.RowIndex].Cells["perdido"].Value.ToString() == "1")
                {
                    gridMusica.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(gridMusica.Font, FontStyle.Strikeout);
                }
            }

            //if (e.RowIndex == -1 && gridMusica.Columns[e.ColumnIndex].Name == "loop")
            //{
            //    // Your code would go here - below is just the code I used to test 
            //      e.Graphics.DrawImage(Properties.Resources.repetiroff, e.CellBounds);
            //    e.Handled = true;
            //}
        }

        private void gridMusica_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            curRow = grid.SelectedCells[0].RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    this.gridMusica.ClearSelection();
                    this.gridMusica.Rows[rowSelected].Selected = true;
                }
                AddContextMenuMusica(e);
            }
        }

        private void AddContextMenuMusica(DataGridViewCellMouseEventArgs e)
        {

            ContextMenu mnuContextMenu = new ContextMenu();

            mnuContextMenu.MenuItems.Add("Propiedades", new EventHandler(MngGridMusicaContext));
            MenuItem m = mnuContextMenu.MenuItems.Add("Añadir a clase...", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Buscar letra", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Añadir letra", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Ver letra", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Traducir letra", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Buscar video", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Pantalla completa", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Copiar", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Draw Wave", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Idioma", new EventHandler(MngGridMusicaContext));
            mnuContextMenu.MenuItems.Add("Borrar", new EventHandler(MngGridMusicaContext));
            if (curGrid == gridItems)
            {
                mnuContextMenu.MenuItems.Add("Editar", new EventHandler(MngGridMusicaContext));
                mnuContextMenu.MenuItems.Add("Ordenar", new EventHandler(MngGridMusicaContext));
            }
            curGrid.ContextMenu = mnuContextMenu;
            this.ContextMenu = mnuContextMenu;
            mnuContextMenu.Show(curGrid, curGrid.PointToClient(Cursor.Position));
            curGrid.ContextMenu = null;
            this.ContextMenu = null;
        }

        private ContextMenu AddContextMenuProyectos( bool carpeta )
        {

            ContextMenu mnuContextMenu = new ContextMenu();

            if (carpeta)
            {
                mnuContextMenu.MenuItems.Add("Nueva carpeta", new EventHandler(MngProyectoContext));
            }
            mnuContextMenu.MenuItems.Add("-");
            mnuContextMenu.MenuItems.Add("Nueva clase", new EventHandler(MngProyectoContext));
            mnuContextMenu.MenuItems.Add("-");
            mnuContextMenu.MenuItems.Add("Cambiar nombre", new EventHandler(MngProyectoContext));
            mnuContextMenu.MenuItems.Add("Eliminar", new EventHandler(MngProyectoContext));
            if (!carpeta)
            {
                mnuContextMenu.MenuItems.Add("-");
                mnuContextMenu.MenuItems.Add("Reproducir", new EventHandler(MngProyectoContext));
                mnuContextMenu.MenuItems.Add("Crear acceso directo", new EventHandler(MngProyectoContext));
            }

            return mnuContextMenu;
        }
        private void MngProyectoContext(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            string text = mi.Text;
            TreeNode node = tvListas.SelectedNode;

            string tipo = string.Empty;
            Int32 id = 0;

            if (node != null)
            {
                tipo = ((Tuple<string, Int32>)tvListas.SelectedNode.Tag).Item1;
                id = ((Tuple<string, Int32>)tvListas.SelectedNode.Tag).Item2;
            }

            if (text == "Cambiar nombre")
            {
                if (node != null)
                {
                    node.BeginEdit();
                    return;
                }
            }
            else if (text == "Eliminar")
            {
                if (node.Text == "Música" || node.Text == "Proyectos")
                {
                    MessageBox.Show("No se puede borrar la carpeta principal", "Atención");
                    return;
                }
                // ¿Qué queremos eliminar, una carpeta o una clase?
                if (MessageBox.Show("¿Estás seguro de borrar?", "Atención", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CarpetaListaMng clm = new CarpetaListaMng();
                    clm.Delete(id);
                    LoadTree();
                }

            }
            else if (text == "Nueva carpeta" || text == "Nueva clase")
            {
                if (node == nodeMusic)
                    return;

                CarpetaListaMng clm = new CarpetaListaMng();
                CarpetaLista cl = new CarpetaLista();
                Int32 id_carpetalista = 0;
                if (node == nodeProyect)
                {
                    id_carpetalista = 0;
                }
                else
                {
                    CarpetaLista cl2 = clm.GetCarpetaLista(id);

                    cl.tipo = (text == "Nueva carpeta" ? 0 : 1);
                    id_carpetalista = cl2.id;

                }
                cl.id_carpetalista = id_carpetalista;

                cl.titulo = text;
                cl.cerrada = false;
                cl.comentario = "";
                clm.Nuevo(cl);
                LoadTree();
            }
            else if (text == "Crear acceso directo")
            {
                
                
                //var pl = wmPlayer.playlistCollection.newPlaylist("plList");
                //WMPLib.IWMPMedia item = wmPlayer.newMedia(@"D:\nosazi.wmv");
                //item.


                //pl.appendItem(item);
                //pl.appendItem(wmPlayer.newMedia(@"D:\tasadof.mp4"));

                //wmPlayer.currentPlaylist = pl;
                //wmPlayer.Ctlcontrols.play();


            }
            else if (text == "Reproducir")
            {
                ListaItemsMng lim = new ListaItemsMng();
                lim.Filter("id_carpetalista=" + id.ToString());
                int wait = 0;

                GoFirstRow();

                int iItems = lim.dv.Count;

                while( curRow <= iItems)     //                foreach (DataRowView row in lim.dv)
                {
                    isInLoop = true;
                    gridItems.Rows[curRow].Selected = true;

                    btnPlay_Click(null,null);
                    try
                    {
                        while (wmPlayer != null && (wmPlayer.playState == WMPLib.WMPPlayState.wmppsTransitioning || wmPlayer.playState != WMPLib.WMPPlayState.wmppsStopped))
                        {
                            Application.DoEvents();
                            if (breakLoop)
                            {
                                break;
                            }
                        }
                        Int32.TryParse(gridItems.Rows[curRow].Cells["esperar"].Value.ToString(), out wait);
                    }
                    catch
                    {

                    }

                    if (gridItems.Rows.Count == 0)
                    {
                        return;
                    }

                    if (breakLoop)
                    {
                        breakLoop = false;
                    }
                    else
                    {
                        if (wait > 0)
                        {
                            FrmCountDown frm = new FrmCountDown();
                            frm.WaitSeconds = wait;
                            frm.ShowDialog();
                        }

                        if (Convert.ToBoolean(gridItems.Rows[curRow].Cells["repetir"].Value))
                        {
                            continue;
                        }
                    }
                    GoNextRow();
                }
                isInLoop = false;

                


            }
        }
        private void GoFirstRow()
        {
            if (curGrid.SelectedRows.Count > 0)
            {
                for (int i = 0; i < curGrid.Columns.Count - 1; i++)
                {
                    if (curGrid.Columns[i].Visible)
                    {
                        curGrid.CurrentCell = curGrid.Rows[curRow].Cells[i];
                        curRow = 0;
                        break;
                    }
                }
            }
        }

        private bool GoNextRow()
        {
            if (curGrid.Rows.Count > 0 && curRow < curGrid.Rows.Count-1)
            {
                curRow++;
                for (int i = 0; i < curGrid.Columns.Count - 1; i++)
                {
                    if (curGrid.Columns[i].Visible)
                    {
                        curGrid.CurrentCell = curGrid.Rows[curRow].Cells[i];
                        return true;
                    }
                }
            }
            return false;
        }



        private void MngGridMusicaContext(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            //Access the clicked item here..
            string text = mi.Text;
            Int32 i = gridMusica.SelectedCells[0].RowIndex;
            if (text == "Añadir a clase...")
            {
                FrmTreeClases frmtc = new FrmTreeClases();
                frmtc.CopyTreeNodes(tvListas);
                if (frmtc.ShowDialog() == DialogResult.OK)  //      SelectClase sc = new SelectClase();    if (sc.ShowDialog() == DialogResult.OK)
                {
                    ListaItemsMng lim = new ListaItemsMng();
                    lista_items li = new lista_items();
                    li.id_carpetalista = frmtc.id;
                    li.id_item = Convert.ToInt32(gridMusica.Rows[i].Cells["id"].Value);
                    li.orden = -1;
                    li.repetir = 0;
                    li.desde = "00:00";
                    li.hasta = Convert.ToString(gridMusica.Rows[i].Cells["duracion"].Value);
                    li.esperar = 0;
                    lim.Nuevo(li);
                    TreeNode sel = GetNodeById(tvListas.Nodes, frmtc.id);
                    if (sel != null)
                    {
                        tvListas.SelectedNode = sel;
                        tvListas.Focus();
                    }
                }
            }
            else if (text == "Propiedades")
            {
                //string file = Path.Combine(gridMusica.Rows[i].Cells["localizacion"].Value.ToString(), gridMusica.Rows[i].Cells["Titulo"].Value.ToString());
                string file = gridMusica.Rows[i].Cells["localizacion"].Value.ToString();
                ShowFileProperties(file);

            }
            else if (text == "Idioma")
            {
                Int32 j = gridMusica.SelectedCells[0].RowIndex;
                string texto = gridMusica.Rows[j].Cells["titulo"].Value.ToString();
                texto = texto.Substring(0, texto.Trim().Length);

                Translator t = new Translator();
                MessageBox.Show(t.Language(texto));
            }
            else if (text == "Buscar letra")
            {
                Int32 j = gridMusica.SelectedCells[0].RowIndex;
                string texto = gridMusica.Rows[j].Cells["titulo"].Value.ToString();
                texto = texto.Substring(0, texto.Trim().Length);

                Translator t = new Translator();
                t.Language(texto);

                String searchRequest = "letra canción \"" + texto + "\"";
                ////searchRequest = new System.Text.RegularExpressions.Regex("(?<=for ?).+$").Match(searchRequest).Value;

                System.Diagnostics.Process.Start("http://www.google.com.au/search?q=" + System.Uri.EscapeDataString(searchRequest));
            }
            else if (text == "Añadir letra")
            {

                Int32 j = curGrid.SelectedCells[0].RowIndex;
                string file = curGrid.Rows[j].Cells["localizacion"].Value.ToString();
                string textoCancion = Clipboard.GetText();
                if (MessageBox.Show("¿Deseas añadir esta letra al fichero?" + Environment.NewLine + Environment.NewLine + textoCancion, "Atención", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    TagLib.File y = null;
                    var id3 = y;
                    file = file.Replace('\n', ' ');
                    try
                    {
                        id3 = TagLib.File.Create(file.Replace(@"file:///",""));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    id3.Tag.Lyrics = textoCancion;
                    
                    try { id3.Save(); }
                    catch { MessageBox.Show("Probablemente esté en uso. Prueba más tarde"); }

                }
            }
            else if (text == "Borrar")
            {

                Int32 j = curGrid.SelectedCells[0].RowIndex;
                Int32 id = Convert.ToInt32(curGrid.Rows[j].Cells["id"].Value);
                string titulo = curGrid.Rows[j].Cells["titulo"].Value.ToString();
                if (MessageBox.Show("¿Deseas borrar " + titulo + "?" + Environment.NewLine + "El fichero permanecerá en el disco.", "Atención", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (curGrid == gridMusica)
                    {
                        ItemMng im = new ItemMng();
                        im.Delete(id);
                        LoadGridMusica();
                    }
                    else
                    {
                        ListaItemsMng lim = new ListaItemsMng();
                        lim.Delete(id);

                        Int32 clase = Convert.ToInt32(curGrid.Rows[j].Cells["id_carpetalista"].Value);
                        LoadGridItems(clase);
                    }


                }

            }
            else if (text == "Editar")
            {

                Int32 j = curGrid.SelectedCells[0].RowIndex;
                Int32 id = Convert.ToInt32(curGrid.Rows[j].Cells["id"].Value);
                Int32 id_carpetalista = Convert.ToInt32(curGrid.Rows[j].Cells["id_carpetalista"].Value);
                ListaItemsMng lim = new ListaItemsMng();
                lim.Filter("id=" + id.ToString());
                
                Int32 idItem = Convert.ToInt32(lim.dv[0]["id_item"]);
                FrmEditClassSong frmECS = new FrmEditClassSong();
                frmECS.id = id;
                frmECS.id_item = idItem;
                frmECS.id_carpetalista = id_carpetalista;
                if (frmECS.ShowDialog() == DialogResult.OK)
                {
                    LoadGridItems(id_carpetalista);
                }
                gridItems.Rows[j].Selected = true;


            }
            else if (text == "Ordenar")
            {

                Int32 j = curGrid.SelectedCells[0].RowIndex;
                Int32 id = Convert.ToInt32(curGrid.Rows[j].Cells["id"].Value);
                Int32 id_carpetalista = Convert.ToInt32(curGrid.Rows[j].Cells["id_carpetalista"].Value);

                string titulo = curGrid.Rows[j].Cells["titulo"].Value.ToString();

                if (curGrid == gridItems)
                {
                    FrmSortItems fsi = new FrmSortItems();
                    fsi.LoadCarpetaLista(id_carpetalista, j);
                    if (fsi.ShowDialog() == DialogResult.OK)
                    {
                        ListaItemsMng lim = new ListaItemsMng();
                        lim.Filter("id_carpetalista=" + id_carpetalista.ToString());
                        foreach (DataRowView row in lim.dv)
                        {
                            Int32 idSong = Convert.ToInt32(row["id"]);
                            lim.UpdateOrden(idSong, fsi.ordenados[idSong]);
                        }
                        lim.GetData();
                        lim.Filter("id_carpetalista=" + id_carpetalista.ToString());
                        LoadGridItems(id_carpetalista);
                    }
                }

            }
            else if (text == "Ver letra")
            {

                Int32 j = gridMusica.SelectedCells[0].RowIndex;

                //Uri uriAddress = new Uri(curGrid.Rows[i].Cells["localizacion"].Value.ToString());
                //string file = uriAddress.ToString().Replace(@"file:///", "");
                string file = curGrid.Rows[i].Cells["localizacion"].Value.ToString();

                TagLib.File y = null;
                var id3 = y;
                try
                {
                    id3 = TagLib.File.Create(file);
                }
                catch { }

                if (id3 != null && !string.IsNullOrEmpty(id3.Tag.Lyrics))
                {
                    string fileLyrics = Path.GetTempFileName();
                    TextEditor te = new TextEditor();
                    te.editor.Text = id3.Tag.Lyrics;
                    te.Show();
                }

            }
            else if (text == "Traducir letra")
            {
                Int32 j = gridMusica.SelectedCells[0].RowIndex;
                string file = gridMusica.Rows[j].Cells["localizacion"].Value.ToString();
                TagLib.File y = null;
                var id3 = y;
                id3 = TagLib.File.Create(file);
                string textoCancion = id3.Tag.Lyrics;
                if (!string.IsNullOrEmpty(textoCancion))
                {
                    textoCancion = Traducir_a_Español(textoCancion);
                    MessageBox.Show(textoCancion);
                }

            }
            else if (text == "Buscar video")
            {
                Int32 j = gridMusica.SelectedCells[0].RowIndex;
                string texto = gridMusica.Rows[j].Cells["titulo"].Value.ToString();
                texto = texto.Substring(0, texto.Length - 4);

                String searchRequest = "video canción \"" + texto + "\"";
                ////searchRequest = new System.Text.RegularExpressions.Regex("(?<=for ?).+$").Match(searchRequest).Value;

                System.Diagnostics.Process.Start("http://www.google.com.au/search?q=" + System.Uri.EscapeDataString(searchRequest));
            }
            else if (text == "Pantalla completa")
            {
                visorvideo vv = new visorvideo();
                Int32 j = gridMusica.SelectedCells[0].RowIndex;
                string file = gridMusica.Rows[j].Cells["localizacion"].Value.ToString();
                vv.axWindowsMediaPlayer1.URL =  file;
                vv.axWindowsMediaPlayer1.windowlessVideo = true;
                vv.ShowDialog();
            }
            else if (text == "Copiar")
            {
                Int32 j = gridMusica.SelectedCells[0].RowIndex;
                string cancion = gridMusica.Rows[j].Cells["Titulo"].Value.ToString();
                //cancion = cancion.Substring(0, cancion.Length - 4);

                //textBoxSearch.Copy();
                Clipboard.Clear();
                try
                {
                    Clipboard.SetText(cancion);
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (text == "Draw Wave")
            {
                Int32 j = curGrid.SelectedCells[0].RowIndex;
                string file = curGrid.Rows[j].Cells["localizacion"].Value.ToString();

                WaveAudio wa = new WaveAudio();
                wa.Mp3File = file;
                wa.ShowDialog();
            }
        }
        public static string Traducir_a_Español(string text, bool decode = true)
        {
            string ret = string.Empty;
            Translator t = new Translator();
            string textTra = t.Translate(text, "auto", "Spanish", decode).Replace("~ ",Environment.NewLine);
            return textTra;
        }


        private ContextMenu AddContextMenuClases()
        {

            ContextMenu mnuContextMenu = new ContextMenu();

            mnuContextMenu.MenuItems.Add("Añadir música", new EventHandler(MngTreeItemClase));
            mnuContextMenu.MenuItems.Add("Añadir documento", new EventHandler(MngTreeItemClase));
            mnuContextMenu.MenuItems.Add("Cerrar", new EventHandler(MngTreeItemClase));
            mnuContextMenu.MenuItems.Add("Ejecutar", new EventHandler(MngTreeItemClase));
            mnuContextMenu.MenuItems.Add("Salvar como Lista de reproducción", new EventHandler(MngTreeItemClase));

            return mnuContextMenu;
        }
        private void MngTreeItemClase(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;

            string text = mi.Text;
            if (text == "Añadir música")
            {
                tvListas.SelectedNode = tvListas.Nodes[0];
                textBoxSearch.Focus();
                Color micolor = textBoxSearch.BackColor;
                textBoxSearch.BackColor = Color.FromArgb(51, 153, 255);
                Application.DoEvents();
                Thread.Sleep(1000);
                textBoxSearch.BackColor = micolor;

            }
            else if (text == "Añadir documento")
            {
                MessageBox.Show(text);
            }
            else if (text == "Cerrar")
            {
                MessageBox.Show(text);
            }
            else if (text == "Ejecutar")
            {
                MessageBox.Show(text);
            }
            else if (text == "Salvar como Lista de reproducción")
            {
                List<Item> listitems = new List<Item>();
                foreach(DataGridViewRow row in curGrid.Rows)
                {
                    Item it = new Item();
                    it.Localizacion = row.Cells["localizacion"].Value.ToString();
                    it.Titulo = row.Cells["titulo"].Value.ToString();
                    listitems.Add(it);

                }
                SaveList(listitems, "Nueva lista", @"C:\Users\francisco.garcia\Music");
            }
        }

        private void MngTreeItemProyecto(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            
            string text = mi.Text;
            if (text == "Crear clase...")
            {
                MessageBox.Show(text);
            }
        }

        //private void crearClaseToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    CrearClase();
        //}
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            visorvideo vv = new visorvideo();

            //vv.axWindowsMediaPlayer1.URL = @"C:\Users\francisco.garcia\Videos\Mercedes Sosa   Gracias a La Vida.avi";
            vv.axWindowsMediaPlayer1.URL = @"C:\Users\francisco.garcia\Music\Biodanzaya version 4\Ya 4 - CIMEB 2012\A chorus line.mp3";
            //vv.axWindowsMediaPlayer1.fullScreen = true;
            vv.axWindowsMediaPlayer1.windowlessVideo = true;
            vv.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void gridClases_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            string s = string.Empty;
            foreach (DataGridViewColumn c in gridItems.Columns)
            {
                s += c.HeaderText+ "-" + c.Width.ToString() + Environment.NewLine;
            }
            MessageBox.Show(s);
        }

        private void button2_Click_2(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void MusicManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th1 != null && th1.IsAlive)
            {
                if( th1.ThreadState == System.Threading.ThreadState.Suspended)
                    th1.Resume();

                th1.Abort();
            }
            
            Properties.Settings.Default.location_MusicManager = Location;
            Properties.Settings.Default.size_MusicManager = Size;
            Properties.Settings.Default.Save();

        }

        private void gridItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void gridMusica_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void curGrid_SelectionChanged(object sender, EventArgs e)
        {
            if ( curGrid != null && curGrid.SelectedCells.Count > 0)
            {
                int selectedrowindex = curGrid.SelectedRows[0].Index;

                DataGridViewRow selectedRow = curGrid.Rows[selectedrowindex];
                curGrid.Rows[selectedrowindex].Selected = true;
                
            }

        }

        private void trackBar2_MouseDown(object sender, MouseEventArgs e)
        {
            if (th1 != null)
            {
                th1.Suspend();
            }
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            if (th1 != null)
            {
                wmPlayer.Ctlcontrols.currentPosition = trackBar2.Value;
                th1.Resume();
            }
            


        }



        private void listaReproduccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog bukaFile = new OpenFileDialog();
            bukaFile.Multiselect = false;
            if (bukaFile.ShowDialog() == DialogResult.OK)
            {
                /// create playlist
                wmPlayer.currentPlaylist = wmPlayer.newPlaylist("nueva", bukaFile.FileName);
                wmPlayer.Ctlcontrols.play();        ////play
            }
        }
        private void SaveList(List<Item> listItems, string listName, string pathName )
        {
            string fileOut = Path.Combine(pathName, listName) + ".m3u";
            if(System.IO.File.Exists(fileOut))
            {
                System.IO.File.Delete(fileOut);
            }
            StringBuilder sb = new StringBuilder();
            foreach (Item item in listItems)
            {
                string cancion = Path.Combine(item.Localizacion, item.Titulo);
                sb.Append(cancion + Environment.NewLine);
            }
            System.IO.File.WriteAllText(fileOut, sb.ToString());

            Process.Start(fileOut);
            return;
        }
        private void LoadMusicLibrary()
        {


        }

        private void LoadMusicLibrary2(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            { return; }

            string file = openFileDialog1.FileName;

            trackBar2.Maximum = NumLines(file);
            trackBar2.Minimum = 0;
            int canciones = 0;
            using (StreamReader sr = new StreamReader(file))
            {
                string idCancion = string.Empty;
                string name = string.Empty;
                string trackid = string.Empty;
                string kind = string.Empty;
                string size = string.Empty;
                string duracion = string.Empty;
                string dateMod = string.Empty;
                string dateAdded = string.Empty;
                string bitrate = string.Empty;
                string localizacion = string.Empty;
                string Album = string.Empty;
                string Artist = string.Empty;
                string genero = string.Empty;
                string extension = string.Empty;

                string idLista = string.Empty;
                List<string> idCanciones = new List<string>();



                int nEn = 0;
                int nLinea = 0;

                modobusqueda estado = modobusqueda.buscando_principio_cancion;
                estado = modobusqueda.buscando_playlists;


                ArrayList datosCancion = new ArrayList();

                while (!sr.EndOfStream )
                {
                    string linea = sr.ReadLine();

                    #region BUSCANDO CANCIONES
                    if (buscando == busqueda.canciones)
                    {
                        if (estado == modobusqueda.buscando_principio_cancion)
                        {
                            if ((nEn = linea.IndexOf("<key>Tracks")) != -1)
                            {
                                estado = modobusqueda.buscando_identificador;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (estado == modobusqueda.buscando_identificador)
                        {
                            if ((nEn = linea.IndexOf("<key>")) != -1)
                            {
                                nEn += "<key>".Length;
                                idCancion = linea.Substring(nEn, linea.IndexOf("</key>") - nEn);
                                estado = modobusqueda.buscando_datos_cancion;
                                continue;
                            }
                            else if ((nEn = linea.IndexOf("</dict>")) != -1)
                            {
                                estado = modobusqueda.buscando_playlists;
                                continue;
                            }
                        }
                        else if (estado == modobusqueda.buscando_datos_cancion)
                        {
                            if ((nEn = linea.IndexOf("<key>Name</key>")) != -1)
                            {
                                int n1 = linea.IndexOf("<string>");
                                n1 += "<string>".Length;
                                int n2 = linea.IndexOf("</string>");
                                name = linea.Substring(n1, n2 - n1);
                                continue;
                            }
                            else if ((nEn = linea.IndexOf("<key>Track ID</key>")) != -1)
                            {
                                int n1 = linea.IndexOf("<integer>");
                                n1 += "<integer>".Length;
                                int n2 = linea.IndexOf("</integer>");
                                trackid = linea.Substring(n1, n2 - n1);
                                continue;
                            }
                            else if ((nEn = linea.IndexOf("<key>Total Time</key>")) != -1)
                            {
                                int n1 = linea.IndexOf("<integer>");
                                n1 += "<integer>".Length;
                                int n2 = linea.IndexOf("</integer>");
                                duracion = linea.Substring(n1, n2 - n1);
                                continue;
                            }
                            else if ((nEn = linea.IndexOf("<key>Album</key>")) != -1)
                            {
                                int n1 = linea.IndexOf("<string>");
                                n1 += "<string>".Length;
                                int n2 = linea.IndexOf("</string>");
                                Album = linea.Substring(n1, n2 - n1);
                                continue;
                            }
                            else if ((nEn = linea.IndexOf("<key>Artist</key>")) != -1)
                            {
                                int n1 = linea.IndexOf("<string>");
                                n1 += "<string>".Length;
                                int n2 = linea.IndexOf("</string>");
                                Artist = linea.Substring(n1, n2 - n1);
                                continue;
                            }
                            else if ((nEn = linea.IndexOf("<key>Genre</key>")) != -1)
                            {
                                int n1 = linea.IndexOf("<string>");
                                n1 += "<string>".Length;
                                int n2 = linea.IndexOf("</string>");
                                genero = linea.Substring(n1, n2 - n1);
                                continue;
                            }
                            else if ((nEn = linea.IndexOf("<key>Location</key>")) != -1)
                            {
                                int n1 = linea.IndexOf("<string>");
                                n1 += "<string>".Length;
                                int n2 = linea.IndexOf("</string>");
                                localizacion = linea.Substring(n1, n2 - n1);
                                Uri uriAddress = new Uri(localizacion);
                                localizacion = uriAddress.LocalPath.Replace(@"\\localhost\", "");
                                continue;

                            }
                            else if ((nEn = linea.IndexOf("</dict>")) != -1)
                            {
                                Item it = new Item();
                                it.Id = int.Parse(trackid);
                                it.Album = Album;
                                it.Autor = Artist;
                                it.Duracion = duracion;
                                it.genero = genero;
                                it.tipo = "Música";
                                it.Titulo = name + extension;
                                it.Localizacion = localizacion.Replace(@"\\", @"\");

                                item.Nuevo(it);
                                canciones++;
                                txtCanciones.Text = "Importada canción " + canciones.ToString();
                                Application.DoEvents();

                                idCancion = string.Empty;
                                name = string.Empty;
                                trackid = string.Empty;
                                kind = string.Empty;
                                size = string.Empty;
                                duracion = string.Empty;
                                dateMod = string.Empty;
                                dateAdded = string.Empty;
                                bitrate = string.Empty;
                                localizacion = string.Empty;
                                Album = string.Empty;
                                Artist = string.Empty;
                                genero = string.Empty;
                                extension = string.Empty;

                                estado = modobusqueda.buscando_identificador;

                                continue;
                            }
                            else if ((nEn = linea.IndexOf("<key>Playlists</key>")) != -1)
                            {
                                estado = modobusqueda.buscando_playlists;
                                buscando = busqueda.listas;
                            }
                        } 
                        #endregion
                            #region BUSCANDO LISTAS
                    }     
                   
                    #endregion
                    if (estado == modobusqueda.finbusqueda)
                    {
                        return;
                    }
                    
                }
            }

        }

        /*
            <key>Name</key><string>curso 16/17</string>
			<key>Playlist ID</key><integer>22911</integer>
			<key>Playlist Persistent ID</key><string>46F4CB7DEDAA7F9D</string>
			<key>All Items</key><true/>
			<key>Folder</key><true/>
			<key>Playlist Items</key>
             */


        private void LoadMusicLibrary(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            { return; }

            string file = openFileDialog1.FileName;
            vaciarToolStripMenuItem_Click(null, null);

            ITunesLibrary ITuLib = new ITunesLibrary();
            var a = ITuLib.Parse(file);

            var b = ITuLib.ParseList(file);
            int ii = b.Count();

            ItemMng itm = new ItemMng();
            Int32 total = a.Count() + b.Count();

            pgBarImport.Minimum = 0;
            pgBarImport.Maximum = total;
            Int32 count = 0;
            foreach (Track t in a)
            {
                Item it = new Item();
                it.Id = t.TrackId;
                it.Album = t.Album;
                it.Autor = t.Artist;
                it.Duracion = t.PlayingTime;
                it.genero = t.Genre;
                it.Localizacion = t.Location;
                it.Titulo = t.Name;
                it.idioma = ""; // tr.Language(it.Titulo);
                itm.Nuevo(it);
                pgBarImport.Value = ++count;
                txtCanciones.Text = count.ToString() + "/" + total.ToString();
                Application.DoEvents();
            }

            LoadGridMusica();

            

           
            CarpetaListaMng pm = new CarpetaListaMng();

            gridMusica.DataSource = null;

            // identifico las carpetas
            Dictionary<string, PlayList> carpetas = new Dictionary<string, PlayList>();
            foreach (PlayList t in b)
            {
                
                if (t.Folder || t.PlaylistItems.Count == 0)
                {
                    carpetas.Add(t.PlaylistPersistentID, t);
                }
            }


            List<string> exclude = new List<string>();
            exclude.Add("Biblioteca");
            exclude.Add("Descargado");
            exclude.Add("Música");
            exclude.Add("Películas");
            exclude.Add("Programas de TV");
            exclude.Add("Podcasts");
            exclude.Add("Audiolibros");
            exclude.Add("Genius");
            exclude.Add("Vídeos");
            exclude.Add("iTunes U");
            exclude.Add("Libros");
            exclude.Add("Tonos");

            foreach (PlayList t in b)
            {
                if (exclude.Contains(t.Name))
                {
                    pgBarImport.Value = ++count;
                    txtCanciones.Text = count.ToString() + "/" + total.ToString();
                    Application.DoEvents();
                    continue;
                }
                CarpetaLista p = new CarpetaLista();
                    p.id = t.PlayListID;
                    p.titulo = t.Name;
                    p.tipo = t.Folder ? 0 : 1;
                    if (t.ParentPersistentID != null && carpetas.ContainsKey(t.ParentPersistentID.ToString()))
                    {
                        p.id_carpetalista = carpetas[t.ParentPersistentID].PlayListID;
                    }
                    pm.Nuevo(p);

                if (t.PlaylistItems.Count > 0)
                {
                    Int32 orden = 1;
                    ListaItemsMng lim = new ListaItemsMng();
                    foreach (Int32 id in t.PlaylistItems)
                    {
                        lista_items li = new lista_items();

                        li.id_item = id;
                        li.id_carpetalista = p.id;
                        li.orden = orden++;
                        li.desde = "00:00";
                        li.hasta = itm.GetItem(id).Duracion;
                        
                        lim.Nuevo(li);
                    }
                }

                //}
                pgBarImport.Value = ++count;
                txtCanciones.Text = count.ToString() + "/" + total.ToString();
                Application.DoEvents();
            }
            LoadTree();
            tvListas.SelectedNode = nodeMusic;
            tvListas.Focus();
            
                     

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Windows.Forms.Panel p = (System.Windows.Forms.Panel) sender ;
            int height = p.Size.Height;
            int width = p.Size.Width;
            Pen blackpen = new Pen(Color.LightGray, 1);
            Rectangle rect = new Rectangle(0, 0, width, height);


            Graphics g = e.Graphics;

            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(230,230,230), Color.FromArgb(208,208,208), LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, rect);
            }

            g.DrawLine(blackpen, 0, height-1, width, height-1);

            //ControlPaint.DrawCaptionButton(g, new Rectangle(width - 47, 0, 42, 16), CaptionButton.Close, ButtonState.Normal);

            g.Dispose();
        }

        private void gridMusica_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle min = new Rectangle(0, 0, 25, 16);
            Rectangle max = new Rectangle(26, 0, 25, 16);
            Rectangle close = new Rectangle(50, 0, 42, 16);

            if (min.Contains(e.Location))
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (max.Contains(e.Location))
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.pictureBox2.Image = Properties.Resources.captionbtns;
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                    this.pictureBox2.Image = Properties.Resources.captionbtnsmax;
                }
            }
            else if (close.Contains(e.Location))
            {
                if (MessageBox.Show("¿Deseas cerrar el programa?", "Atención", MessageBoxButtons.YesNo) != DialogResult.No)
                {
                    this.Close();
                }
            }
        }
        const int WS_CAPTION = 0xC00000;
        const int WS_THICKFRAME = 0x00040000;
        const int WS_SIZEBOX = WS_THICKFRAME;
        const int WS_BORDER = 0x00800000;
        const int WM_NCLBUTTONUP = 0xA2;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                p.Style &= ~WS_CAPTION;
                return p;
            }
        }
      

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void ficheroToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void letraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckedAllMenuitems();
            ToolStripMenuItem mi = (ToolStripMenuItem)sender;
            mi.Checked = true;
        }
        private void UncheckedAllMenuitems()
        {
            for (int i = 0; i < contextMenuSearch.Items.Count; i++)
            {
                if (contextMenuSearch.Items[i] is ToolStripSeparator)
                    continue;
                (contextMenuSearch.Items[i] as ToolStripMenuItem).Checked = false;
            }
        }
        private ToolStripMenuItem GetItemcontextMenuSearchChecked()
        {
            ToolStripMenuItem mi = null;
            for (int i = 0; i < contextMenuSearch.Items.Count; i++)
            {
                if (contextMenuSearch.Items[i] is ToolStripSeparator)
                    continue;

                if ((contextMenuSearch.Items[i] as ToolStripMenuItem).Checked)
                {
                    mi = (contextMenuSearch.Items[i] as ToolStripMenuItem);
                    break;
                }
            }
            return mi;
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxSearch.Cut();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxSearch.Copy();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxSearch.Paste();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int a = textBoxSearch.SelectionLength;
            textBoxSearch.Text = textBoxSearch.Text.Remove(textBoxSearch.SelectionStart, a);
        }

        private void seleccionarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxSearch.SelectAll();
            textBoxSearch.Focus();
        }

        private void panelLogo_Resize(object sender, EventArgs e)
        {
            Size sz = panelLogo.Size;
            Int32 left = sz.Width / 2;
            logoBio.Left = left - logoBio.Width / 2;
            lblBiodanza.Left = left - lblBiodanza.Width / 2;
        }
        public SpeechSynthesizer Hablar(string text, Int32 rate = 0)
        {
            //SpeechSynthesizer reader = new SpeechSynthesizer();
            PromptBuilder cultureSpain = new PromptBuilder(new System.Globalization.CultureInfo("es"));
            reader.Rate = rate;
            cultureSpain.AppendText(text);
            //reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);
            //reader.SpeakProgress += new EventHandler<SpeakProgressEventArgs>(reader_SpeakProgress);
            reader.SpeakAsync(cultureSpain);
            return reader;
        }

        private void añadirCancionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All Supported Audio | *.mp3; *.wma | MP3s | *.mp3 | WMAs | *.wma";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            string file = openFileDialog1.FileName;
            string letra = string.Empty;

            TagLib.File y = null;
            var id3 = y;
            try
            {
                id3 = TagLib.File.Create(file);
                            }
            catch { }

            if (id3 != null && !string.IsNullOrEmpty(id3.Tag.Lyrics))
            {
                letra = id3.Tag.Lyrics;
            }

            MP3File mp3file = ShellID3TagReader.ReadID3Tags(file);

            ItemMng im = new ItemMng();
            Item it = new Item();

            it.Album = id3.Tag.Album;
            it.Autor = id3.Tag.FirstArtist; ;
            string duracion = mp3file.Duration;
            it.Duracion = duracion.Substring(duracion.Length-5,5);
            it.genero = id3.Tag.FirstGenre; ;
            it.tipo = "Música";
            it.Titulo = id3.Tag.Title;
            it.perdido = false;
            it.tieneletra = !string.IsNullOrEmpty(id3.Tag.Lyrics);
            if (string.IsNullOrEmpty(it.Titulo))
            {
                it.Titulo = Path.GetFileNameWithoutExtension(file);
            }
            it.Localizacion = file.Replace(@"\\",@"\");
           
            im.Nuevo(it);
            LoadGridMusica();            

        }

        private void tvListas_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (tvListas.SelectedNode != null && e.Label != null)
            {
                string tipo = ((Tuple<string, Int32>)tvListas.SelectedNode.Tag).Item1;
                Int32 id = ((Tuple<string, Int32>)tvListas.SelectedNode.Tag).Item2;

                CarpetaListaMng clm = new CarpetaListaMng();
                clm.UpdateTitle(id, e.Label);
            }
            
        }

        private void tvListas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void vaciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemMng itm = new ItemMng();
            itm.Erase();
            CarpetaListaMng clm = new CarpetaListaMng();
            clm.Erase();
            ListaItemsMng lim = new ListaItemsMng();
            lim.Erase();
            LoadTree();
            LoadGridMusica();
            LoadGridItems(0);
        }

        private void copiaDeSeguridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string bbdd = Path.Combine( Path.GetDirectoryName( Application.ExecutablePath), "Biodanza.sqlite");
            string biobak = "Biodanza";
            DateTime dt = DateTime.Now;
            
            biobak =  biobak + dt.Day.ToString() + "_" +
                dt.Month.ToString() + "_" +
                dt.Year.ToString() + "_" +
                dt.Hour.ToString() + "_" +
                dt.Minute.ToString() + "_" +
                dt.Second.ToString();
            saveFileDialog1.FileName = biobak + ".biobak";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                    BackupDB(bbdd, saveFileDialog1.FileName);
            }
        }

        private void RestoreDB(string srcFile, string destFile)
        {


            try
            {
                if (System.IO.File.Exists(destFile))
                {
                    DateTime dt = DateTime.Now;
                    string destAuxFile = destFile.Replace(".sqlite", "." + dt.Day.ToString() + "_" +
                    dt.Month.ToString() + "_" +
                    dt.Year.ToString() + "_" +
                    dt.Hour.ToString() + "_" +
                    dt.Minute.ToString() + "_" +
                    dt.Second.ToString());
                    System.IO.File.Move(destFile, destAuxFile);
                }
                System.IO.File.Copy(srcFile, destFile);
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message);
            }
            catch (PathTooLongException pex)
            {
                MessageBox.Show(pex.Message);
            }
            catch (DirectoryNotFoundException dnfex)
            {
                MessageBox.Show(dnfex.Message);
            }
            catch (FileNotFoundException fnfex)
            {
                MessageBox.Show(fnfex.Message);
            }
            catch (IOException ioex)
            {
                MessageBox.Show(ioex.Message);
            }
            catch (NotSupportedException nsex)
            {
                MessageBox.Show(nsex.Message);
            }

        }
        private void BackupDB(string srcFile, string destFile)
        {

            if (System.IO.File.Exists(destFile))
            {
                System.IO.File.Move(destFile, destFile.Replace(".biobak", "." + DateTime.Now.ToString()));
                DateTime dt = DateTime.Now;
                string destAuxFile = destFile.Replace(".biobak", "." + dt.Day.ToString() + "_" +
                dt.Month.ToString() + "_" +
                dt.Year.ToString() + "_" +
                dt.Hour.ToString() + "_" +
                dt.Minute.ToString() + "_" +
                dt.Second.ToString());
                System.IO.File.Move(destFile, destAuxFile);

            }

            try
            {
                System.IO.File.Copy(srcFile, destFile);
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message);
            }
            catch (PathTooLongException pex)
            {
                MessageBox.Show(pex.Message);
            }
            catch (DirectoryNotFoundException dnfex)
            {
                MessageBox.Show(dnfex.Message);
            }
            catch (FileNotFoundException fnfex)
            {
                MessageBox.Show(fnfex.Message);
            }
            catch (IOException ioex)
            {
                MessageBox.Show(ioex.Message);
            }
            catch (NotSupportedException nsex)
            {
                MessageBox.Show(nsex.Message);
            }
        }

        private void restaurarCopiaDeSeguridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "BioCopia (*.biobak)|*.biobak";
            openFileDialog1.DefaultExt = "biobak";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string bbdd = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Biodanza.sqlite");
                Program.m_dbConnection.Close();
                RestoreDB(openFileDialog1.FileName, bbdd);
                Program.connectToDatabase();
                LoadTree();
            }
        }

        private void tvListas_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            string tipo = string.Empty;
            int id = 0;

            if (node != null && node.Tag != null)
            {
                if (node == nodeMusic || node == nodeProyect)
                    return;
                tipo = ((Tuple<string, Int32>)e.Node.Tag).Item1;
                id = ((Tuple<string, Int32>)e.Node.Tag).Item2;
                if (tipo == "carpeta")
                {
                    node.ImageIndex = 3;
                }
            }
        }

        private void tvListas_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            string tipo = string.Empty;
            int id = 0;

            if (node != null && node.Tag != null)
            {
                if (node == nodeMusic || node == nodeProyect)
                    return;
                tipo = ((Tuple<string, Int32>)e.Node.Tag).Item1;
                id = ((Tuple<string, Int32>)e.Node.Tag).Item2;
                if (tipo == "carpeta")
                {
                    node.ImageIndex = 2;
                }
            }
        }

        private void gridItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            curCol = curGrid.Columns[e.ColumnIndex];            
        }

        private void MusicManager_Activated(object sender, EventArgs e)
        {
            
        }

        private void comprobarSiExistenEnElDiscoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pgBarImport.Minimum = 0;
            pgBarImport.Maximum = gridMusica.Rows.Count;

            int count = 0;
            foreach (DataGridViewRow row in gridMusica.Rows)
            {
                pgBarImport.Value = ++count;
                string fichero = Convert.ToString(row.Cells["localizacion"].Value);
                Int32 id = Convert.ToInt32(row.Cells["id"].Value);
                bool perdido = Convert.ToBoolean(row.Cells["perdido"].Value);
                bool perdidoActual = !System.IO.File.Exists(fichero);
                if ( perdido != perdidoActual )
                {                    
                    ItemMng it = new ItemMng();
                    Item item = it.GetItem(id);
                    item.perdido = true;
                    it.UpdateLost(item);
                }
            }
            LoadGridMusica();
        }
    }
}

