using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procedures
{
    public class Consultas
    {
        //Opciones server = new Opciones();

        //Mover estas variables a los metodos;
        List<string> ListaTablas = new List<string>();

        public List<Tabla> ObtenerCampos(string tabla)
        {
            List<Tabla> Campos = new List<Tabla>();

            string query = string.Format("Select  i.COLUMN_NAME,CONVERT(text,i.DATA_TYPE),CONVERT(text,i.IS_NULLABLE),CONVERT(nvarchar,i.CHARACTER_MAXIMUM_LENGTH) from information_schema.columns i WHERE TABLE_NAME='{0}'", tabla);

            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", Opciones.servidor, Opciones.DataBase);

            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);
            cn.Open();
            var lista = cmd.ExecuteReader();
            while (lista.Read())
            {

                Campos.Add(new Tabla(lista.GetString(0), lista.GetString(1), lista.GetValue(3).ToString(), lista.GetValue(2).ToString()));
            }

            cn.Close();
            return Campos;
        }

        public List<string> ObtenerTablas()
        {
            ListaTablas = new List<string>();
            string query = "SELECT SO.NAME from sysobjects So where So.type='U'";
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", Opciones.servidor, Opciones.DataBase);
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

        public DataTable SelectAll(string Table)
        {
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", Opciones.servidor, Opciones.DataBase);

            SqlConnection cn = new SqlConnection(cadenaConexion);

            StringBuilder condiciones = new StringBuilder();

            condiciones.Append("select * from " + Table);

            ////Campos para saber si es necesario un inner join
            foreach (var item in getGenerarRelacion(Table))
            {
                condiciones.Append($" INNER JOIN {item.PK_Tabla} ON {item.PK_Tabla}.{item.PK_Columna} = {Table}.{item.FK_Columna}");
            }

            string consulta = condiciones.ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(consulta, cn);
            DataSet ds = new DataSet();
            adapter.Fill(ds, Table);
            return ds.Tables[0];
        }

        public static string getTablaForeign(string Table)
        {
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", Opciones.servidor, Opciones.DataBase);
            //Agregue FILTRO WHERE TABLA
            string query = $"SELECT FK_Table = FK.TABLE_NAME,FK_Column = CU.COLUMN_NAME,PK_Table = PK.TABLE_NAME,PK_Column = PT.COLUMN_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME INNER JOIN ( SELECT i1.TABLE_NAME, i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' ) PT ON PT.TABLE_NAME = PK.TABLE_NAME AND FK.TABLE_NAME='{Table}'";
            string res = "";
            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);

            StringBuilder condiciones = new StringBuilder();
            cn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                res = reader.GetValue(2).ToString();
            }

            cn.Close();
            return res;
        }

        public List<RelacionTabla> getGenerarRelacion(string Table)
        {
            List<RelacionTabla> lista = new List<RelacionTabla>();
            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", Opciones.servidor, Opciones.DataBase);
            //Agregue FILTRO WHERE TABLA
            string query = $"SELECT FK_Table = FK.TABLE_NAME,FK_Column = CU.COLUMN_NAME,PK_Table = PK.TABLE_NAME,PK_Column = PT.COLUMN_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME INNER JOIN ( SELECT i1.TABLE_NAME, i2.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' ) PT ON PT.TABLE_NAME = PK.TABLE_NAME AND FK.TABLE_NAME='{Table}'";

            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);

            StringBuilder condiciones = new StringBuilder();
            cn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                lista.Add(new RelacionTabla(reader.GetString(0), reader.GetString(1), reader.GetValue(2).ToString(), reader.GetValue(3).ToString()));
            }

            cn.Close();
            return lista;
        }

        private string getTable<T>(T entities)
        {
            return entities.GetType().Name;
        }

        public static List<Parametros> getParametros(string tabla, string operacion)
        {
            List<Parametros> lista = new List<Parametros>();

            string cadenaConexion = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True", Opciones.servidor, Opciones.DataBase);

            string query = $@"select PARAMETER_NAME from information_schema.parameters where specific_name='SPU_{operacion}_{tabla}'";

            SqlConnection cn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand(query, cn);

            cn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Parametros aux = new Parametros();
                aux.nombre = reader.GetString(0);
                lista.Add(aux);
            }

            cn.Close();

            return lista;
        }

    }
}