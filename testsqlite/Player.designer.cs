namespace biodanza
{
    partial class Player
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            this.ofdSong = new System.Windows.Forms.OpenFileDialog();
            this.wmPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.wmPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // ofdSong
            // 
            this.ofdSong.InitialDirectory = "c:\\Users\\francisco.garcia\\Music";
            this.ofdSong.Multiselect = true;
            // 
            // wmPlayer
            // 
            this.wmPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wmPlayer.Enabled = true;
            this.wmPlayer.Location = new System.Drawing.Point(0, 0);
            this.wmPlayer.Name = "wmPlayer";
            this.wmPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wmPlayer.OcxState")));
            this.wmPlayer.Size = new System.Drawing.Size(471, 325);
            this.wmPlayer.TabIndex = 0;
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 325);
            this.Controls.Add(this.wmPlayer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Player";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            ((System.ComponentModel.ISupportInitialize)(this.wmPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog ofdSong;
        private AxWMPLib.AxWindowsMediaPlayer wmPlayer;
    }
}