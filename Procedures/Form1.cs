using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

namespace Procedures
{
    public partial class Form1 : Form
    {
        string actual = "";

        List<string> ListaTablas = new List<string>();
        List<string> Atributos = new List<string>();

        string server = @"CEALER-PC\CEALER";
        string BD = "ABUELO";

        public Form1()
        {
            InitializeComponent();
        }

        void ObtenerTablas(string servidor, string baseDatos)
        {

            string query = "SELECT SO.NAME from sysobjects So where So.type='U'";
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", servidor, baseDatos);
            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);
            cn.Open();
            var lista = cmd.ExecuteReader();

            while (lista.Read())
            {
                ListaTablas.Add(lista.GetString(0));
            }
            cn.Close();

            foreach (var item in ListaTablas)
            {
                if (!item.ToString().Equals(actual.ToString()))
                {
                    cboTablas.Items.Add(item);
                }
                actual = item;
            }
        }

        void ObtenerAtributos(string servidor, string baseDatos, string tabla)
        {
            string query = string.Format("Select  i.COLUMN_NAME,CONVERT(text,i.DATA_TYPE),CONVERT(text,i.IS_NULLABLE),CONVERT(nvarchar,i.CHARACTER_MAXIMUM_LENGTH) from information_schema.columns i WHERE TABLE_NAME='{0}'", tabla);
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", servidor, baseDatos);
            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);
            cn.Open();
            var lista = cmd.ExecuteReader();

            while (lista.Read())
            {
                Atributos.Add(lista.GetString(0));
            }
            cn.Close();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            ObtenerAtributos(server, BD, cboTablas.SelectedItem.ToString());
            ProcedureInsert(server, BD, cboTablas.SelectedItem.ToString());
            GuardarArchivo();
            AbrirArchivo();
        }

        void GuardarArchivo()
        {
            string lines = txtS.Text;
            System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath +string.Format("GUARDAR{0}.sql",BD));
            file.WriteLine(lines);
            file.Close();
        }

        void AbrirArchivo() {
            System.Diagnostics.Process.Start(Application.StartupPath + string.Format("GUARDAR{0}.sql", BD));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ObtenerTablas(server, BD);

        }

        public void ProcedureInsert(string servidor, string baseDatos, string tabla)
        {
            txtS.AppendText("USE " + BD + " \r\n");
            txtS.AppendText("GO " + " \r\n");
            txtS.AppendText("CREATE PROCEDURE SPU_INSERTAR_" + tabla + "\r\n");
            string query = string.Format("Select  i.COLUMN_NAME,CONVERT(text,i.DATA_TYPE),CONVERT(text,i.IS_NULLABLE),CONVERT(nvarchar,i.CHARACTER_MAXIMUM_LENGTH) from information_schema.columns i WHERE TABLE_NAME='{0}'", tabla);
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", servidor, baseDatos);
            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);
            cn.Open();
            var lista = cmd.ExecuteReader();
            string text = "";
            int max = Atributos.Count;

            while (lista.Read())
            {
                max--;
                if (lista.GetString(1).Contains("char"))
                {
                    text = ("@" + lista.GetString(0) + " " + lista.GetString(1) + "(" + lista.GetValue(3) + ")");
                }
                else
                {
                    text = ("@" + lista.GetString(0) + " " + lista.GetString(1) + lista.GetValue(3));
                }
                if (max == 0)
                {
                    //Si es NULL
                    if (lista.GetValue(2).ToString() == "NO")
                    {
                        text += " \r\n";
                    }
                    else
                    {
                        text += "= NULL," + "\r\n";
                    }
                }
                else
                {
                    //Si es NULL
                    if (lista.GetValue(2).ToString() == "NO")
                    {
                        text += ", \r\n";
                    }
                    else
                    {
                        text += "= NULL," + "\r\n";
                    }
                }
                txtS.AppendText(text);

            }
            cn.Close();
            txtS.AppendText("AS" + "\r\n");
            txtS.AppendText(Insert() + "\r\n");
            txtS.AppendText("GO" + "\r\n");
        }

        public string Insert()
        {
            string consulta = "";
            StringBuilder dbAtri = new StringBuilder();
            int max = Atributos.Count;

            foreach (var item in Atributos)
            {
                max--;
                //if (!item.Name.Contains("Id" + Table))
                //{

                if (max == 0)
                {
                    dbAtri.Append("@" + item.ToString());
                    dbAtri.Append(")");
                }
                else
                {
                    dbAtri.Append("@" + item.ToString() + ",");
                }
                //}
            }
            consulta = "INSERT INTO " + cboTablas.SelectedItem.ToString() + " VALUES (" + dbAtri;
            return consulta;
        }
    }
}
