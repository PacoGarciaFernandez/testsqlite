namespace biodanza
{
    partial class FormQuery
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
            this.newQuery1 = new biodanza.NewQuery();
            this.SuspendLayout();
            // 
            // newQuery1
            // 
            this.newQuery1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newQuery1.Location = new System.Drawing.Point(0, 0);
            this.newQuery1.Name = "newQuery1";
            this.newQuery1.Size = new System.Drawing.Size(894, 360);
            this.newQuery1.TabIndex = 0;
            // 
            // FormQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 360);
            this.Controls.Add(this.newQuery1);
            this.Name = "FormQuery";
            this.Text = "FormQuery";
            this.ResumeLayout(false);

        }

        #endregion

        private NewQuery newQuery1;
    }
}