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
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace Procedures
{
    public partial class Form1 : Form
    {
        string actual = "";
        string id, campoIncrementar;
        List<string> ListaTablas = new List<string>();
        List<Atributos> Atributos = new List<Atributos>();
        List<string> ListaBD = new List<string>();
        List<string> ListaCampos = new List<string>();

        string filename;
        string server = @"CEALER-PC\CEALER";

        public Form1()
        {
            InitializeComponent();
        }

        void ObtenerTablas(string servidor, string baseDatos)
        {
            ListaTablas = new List<string>();
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
            cboTablas.Items.Clear();
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
                Atributos.Add(new Atributos(lista.GetString(0), lista.GetString(1), lista.GetValue(3).ToString(), lista.GetValue(2).ToString()));
            }
            cn.Close();
        }


        void GuardarArchivo()
        {
            string lines = txtS.Text;
            System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + string.Format("{0}{1}.sql", filename, cboBD.SelectedItem.ToString()));
            file.WriteLine(lines);
            file.Close();
        }

        void AbrirArchivo()
        {
            System.Diagnostics.Process.Start(Application.StartupPath + string.Format("{0}{1}.sql", filename, cboBD.SelectedItem.ToString()));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ObtenerBDs(server);
        }
        void ObtenerBDs(string servidor)
        {
            string query = "SELECT * from sys.databases";
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", servidor, "master");
            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);

            cn.Open();

            var lista = cmd.ExecuteReader();

            while (lista.Read())
            {
                ListaBD.Add(lista.GetString(0));
            }

            cn.Close();

            foreach (var item in ListaBD)
            {
                cboBD.Items.Add(item.ToString());
            }
        }

        List<Atributos> listaAtrib = new List<Atributos>();

        Atributos aux;

        public void ProcedureUpdate(string servidor, string baseDatos, string tabla)
        {
            txtS.AppendText("USE " + cboBD.SelectedItem.ToString() + " \r\n");
            txtS.AppendText("GO " + " \r\n");
            txtS.AppendText("CREATE PROCEDURE SPU_MODIFICAR_" + tabla + "\r\n");
            txtS.AppendText("@" + dgvAtributos.Rows[0].Cells[0].Value.ToString() + " " + dgvAtributos.Rows[0].Cells[1].Value.ToString() + "(" + dgvAtributos.Rows[0].Cells[2].Value.ToString() + ")" + ", \r\n");
            string text = "";

            foreach (DataGridViewRow elemento in dgvAtributos.Rows)
            {
                if ((bool)(elemento.Cells[4].Value) == true)
                {
                    listaAtrib.Add(new Atributos(elemento.Cells[0].Value.ToString(), elemento.Cells[1].Value.ToString(), elemento.Cells[2].Value.ToString(), elemento.Cells[3].Value.ToString()));
                }
            }

            int max = listaAtrib.Count;
            MessageBox.Show(max.ToString());
            foreach (var atri in listaAtrib)
            {
                max--;
                if (atri.tipoDato.Contains("char"))
                {
                    text = ("@" + atri.Nombre + " " + atri.tipoDato + "(" + atri.length + ")");
                }
                else
                {
                    text = ("@" + atri.Nombre + " " + atri.tipoDato + atri.length);
                }

                if (max == 0)
                {
                    //   Si es NULL
                    if (atri.TipoNull == "NO")
                    {
                        text += " \r\n";
                    }
                    else
                    {
                        text += "= NULL" + "\r\n";
                    }
                }

                else
                {
                    // Si es NULL
                    if (atri.TipoNull == "NO")
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

            txtS.AppendText("AS" + "\r\n");
            txtS.AppendText(UPDATE() + "\r\n");
            txtS.AppendText("GO");
        }

        public void ProcedureInsert(string servidor, string baseDatos, string tabla)
        {

            listaAtrib.Clear();
            ListaCampos.Clear();
            txtS.AppendText("USE " + cboBD.SelectedItem.ToString() + " \r\n");
            txtS.AppendText("GO " + " \r\n");
            txtS.AppendText("CREATE PROCEDURE SPU_INSERTAR_" + tabla + "\r\n");
            string text = "";

            #region Declarando variables para el procedimiento
            foreach (DataGridViewRow elemento in dgvAtributos.Rows)
            {
                if ((bool)(elemento.Cells[4].Value) == true)
                {
                    listaAtrib.Add(new Atributos(elemento.Cells[0].Value.ToString(), elemento.Cells[1].Value.ToString(), elemento.Cells[2].Value.ToString(), elemento.Cells[3].Value.ToString()));
                }
            }

            int max = listaAtrib.Count;

            foreach (var atri in listaAtrib)
            {
                max--;

                if (!(atri.Nombre.ToLower().StartsWith("id" + tabla.ToLower())))
                {
                    if (atri.tipoDato.Contains("char"))
                    {
                        text = ("@" + atri.Nombre + " " + atri.tipoDato + "(" + atri.length + ")");
                    }
                    else
                    {
                        text = ("@" + atri.Nombre + " " + atri.tipoDato + atri.length);
                    }

                    if (max == 0)
                    {
                        //   Si es NULL
                        if (atri.TipoNull == "NO")
                        {
                            text += " \r\n";
                        }
                        else
                        {
                            text += "= NULL" + "\r\n";
                        }
                    }

                    else
                    {
                        // Si es NULL
                        if (atri.TipoNull == "NO")
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
                else
                {
                    id = text = ("@" + atri.Nombre + " " + atri.tipoDato + "(" + atri.length + ")");
                    campoIncrementar = atri.Nombre;
                }
            }
            #endregion

            #region Escribiendo el cuerpo del procedimiento
            imprimir("AS");
            imprimir("BEGIN");
            imprimir("	BEGIN TRANSACTION TR_" + tabla);
            imprimir("	Declare @Mensaje nvarchar(30)");
            imprimir("		BEGIN");

            //Escribiendo condiciones

            foreach (var item in listaAtrib)
            {
                if (item.tipoDato.ToLower().Contains("nvarchar"))
                {
                    imprimir(string.Format("			IF LEN(@{0})=0", item.Nombre));
                    imprimir("                    BEGIN");
                    imprimir(string.Format("					SET @Mensaje='El {0} no puede estar en blanco'", item.Nombre.ToLower()));
                    imprimir("				END");
                }
                else if (item.tipoDato.ToLower().Contains("decimal") || item.tipoDato.ToLower().Contains("numeric") || item.tipoDato.ToLower().Contains("money"))
                {
                    imprimir(string.Format("			IF @{0}>0", item.Nombre));
                    imprimir("                    BEGIN");
                    imprimir(string.Format("					SET @Mensaje='El {0} no puede ser menor o igual que 0'", item.Nombre.ToLower()));
                    imprimir("				END");
                }
            }

            imprimir("				Declare " + id);
            imprimir("				SET @" + campoIncrementar + " = " + Incrementar(tbxCOD.Text, tbxNUM.Text, tabla));
            imprimir("				" + Insert());
            imprimir("					IF @@ERROR<>0");
            imprimir("						BEGIN");
            imprimir(string.Format("						SET @Mensaje='Error al registrar al {0}'", tabla));
            imprimir("						GOTO ERROR");
            imprimir("					END");
            imprimir("			OK:");
            imprimir("			COMMIT TRANSACTION TR_" + tabla);
            imprimir(string.Format("			SET @Mensaje='{0} Registrado'", tabla));
            imprimir("			PRINT(@Mensaje)");
            imprimir("			GOTO FIN");
            imprimir("		ERROR:");
            imprimir(string.Format("			ROLLBACK TRANSACTION TR_{0}", tabla));
            imprimir("			RAISERROR(@Mensaje,15,1)");
            imprimir("			GOTO FIN");
            imprimir("		FIN:");
            imprimir("			END");
            imprimir("END");
            imprimir("GO");
            #endregion
        }


        public void imprimir(string cad)
        {
            txtS.AppendText(cad + "\r\n");
        }

        //Llenar lista con los campos de la tabla
        public void cargarCamposBD()
        {
            ListaCampos.Clear();
            foreach (DataGridViewRow elemento in dgvAtributos.Rows)
            {
                if ((bool)(elemento.Cells[4].Value) == true)
                {
                    ListaCampos.Add(elemento.Cells[0].Value.ToString());
                }
            }
        }

        //Crear consulta para autoincremento
        public string Incrementar(string ide, string num, string tabla)
        {
            return string.Format("'{0}'+(SELECT RIGHT('{1}'+LTRIM(STR(COUNT(*)+1)),{2}) FROM {3})", ide, num, num.Length, tabla);
        }

        //Crear consulta Insert
        public string Insert()
        {
            string consulta = "";
            StringBuilder dbAtri = new StringBuilder();
            StringBuilder CamposInsert = new StringBuilder();
            cargarCamposBD();
            int max = ListaCampos.Count;

            foreach (var item in ListaCampos)
            {
                max--;
                if (max == 0)
                {
                    CamposInsert.Append(item);
                    dbAtri.Append("@" + item);
                }
                else
                {
                    CamposInsert.Append(item + ",");
                    dbAtri.Append("@" + item + ",");
                }
            }
            consulta = "INSERT INTO " + cboTablas.SelectedItem.ToString() + "(" + CamposInsert + ") VALUES (" + dbAtri + ")";
            return consulta;
        }

        private void cboBD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObtenerTablas(server, cboBD.SelectedItem.ToString());
        }

        public string UPDATE()
        {
            ListaCampos.Clear();
            string consulta = "";
            StringBuilder dbAtri = new StringBuilder();

            foreach (DataGridViewRow elemento in dgvAtributos.Rows)
            {
                if ((bool)(elemento.Cells[4].Value) == true)
                {
                    ListaCampos.Add(elemento.Cells[0].Value.ToString());
                }
            }

            int max = ListaCampos.Count;

            foreach (var item in ListaCampos)
            {
                max--;
                if (max == 0)
                {
                    dbAtri.Append(item + "=@");
                    dbAtri.Append(item);
                }
                else
                {
                    dbAtri.Append(item + "=@");
                    dbAtri.Append(item + ",");
                }

            }
            consulta = "UPDATE " + cboTablas.SelectedItem.ToString() + " SET " + dbAtri + " WHERE " + dgvAtributos.Rows[0].Cells[0].Value.ToString() + " =@" + dgvAtributos.Rows[0].Cells[0].Value.ToString();
            return consulta;
        }

        private void cboTablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvAtributos.Rows.Clear();
            ObtenerAtributos(server, cboBD.SelectedItem.ToString(), cboTablas.SelectedItem.ToString());
            foreach (var item in Atributos)
            {
                dgvAtributos.Rows.Add(item.Nombre, item.tipoDato, item.length, item.TipoNull, true);
            }
        }

        //Botones
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            txtS.ResetText();
            Atributos = new List<Atributos>();
            ListaTablas = new List<string>();
            ObtenerAtributos(server, cboBD.SelectedItem.ToString(), cboTablas.SelectedItem.ToString());
            ProcedureInsert(server, cboBD.SelectedItem.ToString(), cboTablas.SelectedItem.ToString());
            filename = "INSERTAR";
            GuardarArchivo();
            AbrirArchivo();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtS.ResetText();
            Atributos = new List<Atributos>();
            ListaTablas = new List<string>();
            ProcedureUpdate(server, cboBD.SelectedItem.ToString(), cboTablas.SelectedItem.ToString());
            filename = "Modificar";
            GuardarArchivo();
            AbrirArchivo();
        }

        public void ProcedureSelect(string servidor, string baseDatos, string tabla)
        {
            txtS.AppendText("USE " + cboBD.SelectedItem.ToString() + " \r\n");
            txtS.AppendText("GO " + " \r\n");
            txtS.AppendText("CREATE PROCEDURE SPU_BUSCAR_" + tabla + "\r\n");
            txtS.AppendText("AS" + "\r\n");
            txtS.AppendText("Select ");
            string query = string.Format("Select  i.COLUMN_NAME,CONVERT(text,i.DATA_TYPE),CONVERT(text,i.IS_NULLABLE),CONVERT(nvarchar,i.CHARACTER_MAXIMUM_LENGTH) from information_schema.columns i WHERE TABLE_NAME='{0}'", tabla);
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", servidor, baseDatos);
            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);
            cn.Open();
            var lista = cmd.ExecuteReader();
            string text = "";

            foreach (DataGridViewRow elemento in dgvAtributos.Rows)
            {
                if ((bool)(elemento.Cells[4].Value) == true)
                {
                    listaAtrib.Add(new Atributos(elemento.Cells[0].Value.ToString(), elemento.Cells[1].Value.ToString(), elemento.Cells[2].Value.ToString(), elemento.Cells[3].Value.ToString()));
                }
            }

            int max = listaAtrib.Count;

            foreach (var item in listaAtrib)
            {
                max--;
                if (max == 0)
                {
                    text = item.Nombre;
                }
                else
                {
                    text = item.Nombre + ", ";
                }
                txtS.AppendText(text);
            }
            txtS.AppendText(" From " + tabla + "\r\n");
            txtS.AppendText("GO");
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ProcedureSelect(server, cboBD.SelectedItem.ToString(), cboTablas.SelectedItem.ToString());

        }




        void Insert1CampoNvarchar(string archivo)
        {
            StreamReader file = new StreamReader(@"C:\Users\Cesar\Desktop\Datos\" + archivo + ".txt");

            List<string> lista = new List<string>();

            string str = file.ReadToEnd();

            var r = str.Split();

            foreach (var item in r)
            {
                if (!item.Equals(""))
                {
                    if (item.Contains("_"))
                    {
                        txtS.AppendText("INSERT INTO " + archivo + " VALUES ('" + Regex.Replace(item, "_", " ") + "')" + "\r\n");
                    }
                    else
                    {
                        txtS.AppendText("INSERT INTO " + archivo + " VALUES ('" + item + "')" + "\r\n");
                    }
                }
            }
        }

        void InsertCargos()
        {
            StreamReader file = new StreamReader(@"C:\Users\Cesar\Desktop\Datos\Cargos.txt");

            List<string> lista = new List<string>();

            string str = file.ReadToEnd();

            var r = str.Split();


            foreach (var item in r)
            {
                if (!item.Equals(""))
                {
                    if (item.Contains("_"))
                    {
                        txtS.AppendText("INSERT INTO CARGOS VALUES ('" + Regex.Replace(item, "_", " ") + "')" + "\r\n");
                    }
                    else
                    {
                        txtS.AppendText("INSERT INTO DEPARTAMENTO VALUES ('" + item + "')" + "\r\n");
                    }
                }
            }
        }

        private void btnInsertarDatos_Click(object sender, EventArgs e)
        {
            //Insert1CampoNvarchar("CARGO");
            //Insert1CampoNvarchar("DEPARTAMENTOS");
            //Insert1CampoNvarchar("COLEGIO_PROFESIONAL");
            //Insert1CampoNvarchar("AREA");

        }
    }
}