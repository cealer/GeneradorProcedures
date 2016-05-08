namespace Procedures
{
    partial class GeneradoCodigo
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
            this.btnGenerarBOL = new System.Windows.Forms.Button();
            this.btnCrearDal = new System.Windows.Forms.Button();
            this.btnCargar = new System.Windows.Forms.Button();
            this.dgvLista = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLista)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerarBOL
            // 
            this.btnGenerarBOL.Location = new System.Drawing.Point(25, 108);
            this.btnGenerarBOL.Name = "btnGenerarBOL";
            this.btnGenerarBOL.Size = new System.Drawing.Size(135, 23);
            this.btnGenerarBOL.TabIndex = 11;
            this.btnGenerarBOL.Text = "Generar clases BOL";
            this.btnGenerarBOL.UseVisualStyleBackColor = true;
            this.btnGenerarBOL.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnCrearDal
            // 
            this.btnCrearDal.Location = new System.Drawing.Point(25, 70);
            this.btnCrearDal.Name = "btnCrearDal";
            this.btnCrearDal.Size = new System.Drawing.Size(135, 23);
            this.btnCrearDal.TabIndex = 10;
            this.btnCrearDal.Text = "Generar clases DAL";
            this.btnCrearDal.UseVisualStyleBackColor = true;
            this.btnCrearDal.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCargar
            // 
            this.btnCargar.Location = new System.Drawing.Point(25, 31);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(135, 21);
            this.btnCargar.TabIndex = 6;
            this.btnCargar.Text = "Cargar tablas";
            this.btnCargar.UseVisualStyleBackColor = true;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // dgvLista
            // 
            this.dgvLista.AllowUserToAddRows = false;
            this.dgvLista.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLista.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvLista.Location = new System.Drawing.Point(566, 31);
            this.dgvLista.Name = "dgvLista";
            this.dgvLista.Size = new System.Drawing.Size(247, 360);
            this.dgvLista.TabIndex = 12;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Tabla";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Escoger";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // GeneradoCodigo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 403);
            this.Controls.Add(this.dgvLista);
            this.Controls.Add(this.btnGenerarBOL);
            this.Controls.Add(this.btnCrearDal);
            this.Controls.Add(this.btnCargar);
            this.Name = "GeneradoCodigo";
            this.Text = "GeneradoCodigo";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLista)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGenerarBOL;
        private System.Windows.Forms.Button btnCrearDal;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.DataGridView dgvLista;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
    }
}