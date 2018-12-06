namespace biodanza
{
    partial class MusicaFrm
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
            this.gridCanciones = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridCanciones)).BeginInit();
            this.SuspendLayout();
            // 
            // gridCanciones
            // 
            this.gridCanciones.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridCanciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCanciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCanciones.Location = new System.Drawing.Point(0, 0);
            this.gridCanciones.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridCanciones.Name = "gridCanciones";
            this.gridCanciones.ReadOnly = true;
            this.gridCanciones.Size = new System.Drawing.Size(627, 438);
            this.gridCanciones.TabIndex = 2;
            // 
            // MusicaFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 438);
            this.Controls.Add(this.gridCanciones);
            this.Name = "MusicaFrm";
            this.Text = "MusicaFrm";
            this.Load += new System.EventHandler(this.MusicaFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridCanciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridCanciones;
    }
}