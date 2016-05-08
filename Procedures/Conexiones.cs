using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procedures
{
    public class Conexiones
    {
        List<string> ListaTablas = new List<string>();
        List<Atributos> Atributos = new List<Atributos>();

        public void ObtenerAtributos(string servidor, string baseDatos, string tabla)
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

        public List<string> ObtenerTablas(string servidor, string baseDatos)
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
            return ListaTablas;
        }
    }
}
