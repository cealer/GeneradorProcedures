namespace Procedures
{
    partial class Principal
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
            this.btnProcedimientos = new System.Windows.Forms.Button();
            this.btnGenerador = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnProcedimientos
            // 
            this.btnProcedimientos.Location = new System.Drawing.Point(49, 38);
            this.btnProcedimientos.Name = "btnProcedimientos";
            this.btnProcedimientos.Size = new System.Drawing.Size(151, 23);
            this.btnProcedimientos.TabIndex = 0;
            this.btnProcedimientos.Text = "Procedimientos";
            this.btnProcedimientos.UseVisualStyleBackColor = true;
            this.btnProcedimientos.Click += new System.EventHandler(this.btnProcedimientos_Click);
            // 
            // btnGenerador
            // 
            this.btnGenerador.Location = new System.Drawing.Point(310, 38);
            this.btnGenerador.Name = "btnGenerador";
            this.btnGenerador.Size = new System.Drawing.Size(146, 23);
            this.btnGenerador.TabIndex = 0;
            this.btnGenerador.Text = "Generador de clases";
            this.btnGenerador.UseVisualStyleBackColor = true;
            this.btnGenerador.Click += new System.EventHandler(this.btnGenerador_Click);
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 268);
            this.Controls.Add(this.btnGenerador);
            this.Controls.Add(this.btnProcedimientos);
            this.Name = "Principal";
            this.Text = "Principal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProcedimientos;
        private System.Windows.Forms.Button btnGenerador;
    }
}