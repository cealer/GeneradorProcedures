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
    public partial class GeneradoCodigo : Form
    {
        Cuerpo_Clase cuerpo = new Cuerpo_Clase();

        public GeneradoCodigo()
        {
            InitializeComponent();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {

            Consultas aux = new Consultas();
            var lista = aux.ObtenerTablas();

            foreach (var item in lista)
            {
                dgvLista.Rows.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cuerpo.Tipo = "AD";
            cuerpo.namespace_Capa = "ACCESO_DATOS";
            cuerpo.Generar_DAL_BD();
            MessageBox.Show("Proceso terminado");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cuerpo.Tipo = "BOL";
            cuerpo.namespace_Capa = "BOL";
            cuerpo.Generar_BOL_BD();
            MessageBox.Show("Proceso terminado");
        }
    }
}