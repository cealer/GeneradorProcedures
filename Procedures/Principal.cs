using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Procedures
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void btnProcedimientos_Click(object sender, EventArgs e)
        {
            Form1 aux = new Form1();
            aux.ShowDialog();
        }

        private void btnGenerador_Click(object sender, EventArgs e)
        {
            GeneradoCodigo aux = new GeneradoCodigo();
            aux.ShowDialog();
        }
    }
}
