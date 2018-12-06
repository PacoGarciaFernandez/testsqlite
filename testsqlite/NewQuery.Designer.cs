namespace biodanza
{
    partial class NewQuery
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grid = new System.Windows.Forms.DataGridView();
            this.add = new System.Windows.Forms.DataGridViewButtonColumn();
            this.delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Y_O = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Campo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Operador1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.add,
            this.delete,
            this.Y_O,
            this.Campo,
            this.Operador1,
            this.Valor});
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(822, 384);
            this.grid.TabIndex = 0;
            this.grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellContentClick);
            // 
            // add
            // 
            this.add.HeaderText = "Nuevo";
            this.add.Name = "add";
            // 
            // delete
            // 
            this.delete.HeaderText = "Borrar";
            this.delete.Name = "delete";
            // 
            // Y_O
            // 
            this.Y_O.HeaderText = "Y/O";
            this.Y_O.Name = "Y_O";
            // 
            // Campo
            // 
            this.Campo.HeaderText = "Campo";
            this.Campo.Name = "Campo";
            this.Campo.Width = 150;
            // 
            // Operador1
            // 
            this.Operador1.HeaderText = "Operador";
            this.Operador1.Name = "Operador1";
            // 
            // Valor
            // 
            this.Valor.HeaderText = "Valor";
            this.Valor.Name = "Valor";
            this.Valor.Width = 300;
            // 
            // NewQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grid);
            this.Name = "NewQuery";
            this.Size = new System.Drawing.Size(822, 384);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.DataGridViewButtonColumn add;
        private System.Windows.Forms.DataGridViewButtonColumn delete;
        private System.Windows.Forms.DataGridViewComboBoxColumn Y_O;
        private System.Windows.Forms.DataGridViewComboBoxColumn Campo;
        private System.Windows.Forms.DataGridViewComboBoxColumn Operador1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
    }
}
