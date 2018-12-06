namespace biodanza
{
    partial class FormRecord
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
            this.btnRecord = new System.Windows.Forms.Button();
            this.txtTime = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // btnRecord
            // 
            this.btnRecord.Image = global::biodanza.Properties.Resources.if_1_5_977961;
            this.btnRecord.Location = new System.Drawing.Point(12, 12);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(48, 44);
            this.btnRecord.TabIndex = 0;
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // txtTime
            // 
            this.txtTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.Location = new System.Drawing.Point(66, 22);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(50, 23);
            this.txtTime.TabIndex = 1;
            this.txtTime.Text = "00:00";
            // 
            // btnStop
            // 
            this.btnStop.Image = global::biodanza.Properties.Resources.if_controls_stop_103461;
            this.btnStop.Location = new System.Drawing.Point(12, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(48, 44);
            this.btnStop.TabIndex = 2;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // FormRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(128, 68);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.btnRecord);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grabar";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.RecordForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Label txtTime;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}