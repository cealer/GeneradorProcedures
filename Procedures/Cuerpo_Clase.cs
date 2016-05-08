using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Procedures
{
    public class Cuerpo_Clase
    {
        public string namespace_Capa { get; set; }
        public string Tipo { get; set; }
        const string Comillas = "\"";


        public void CrearCuerpo(string tablaBD)
        {
            StringBuilder Clase = new StringBuilder();

            //Determinar dónde guardará
            string archivo;
            string tipo = "";

            //Creando el path del archivo
            if (Tipo.StartsWith("AD"))
            {
                tipo = Opciones.DirectorioDAL;
            }

            else if (Tipo.StartsWith("BOL"))
            {
                tipo = Opciones.DirectorioBOL;
            }

            archivo = tipo + string.Format(@"\{0}{1}.cs", tablaBD, Tipo);

            string[] namespaces = { "System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks" };

            //Agregando namespaces predeterminados

            foreach (var item in namespaces)
            {
                Clase.AppendLine(string.Format("using {0}; ", item));
            }

            if (Tipo.StartsWith("AD"))
            {
                //Agregando namespace de ENTIDADES
                Clase.AppendLine("using Entidades;");
                Clase.AppendLine("using ACCESO_DATOS;");
                Clase.AppendLine("using System.Data;");
                Clase.AppendLine("using System.Data.SqlClient;");
                Clase.AppendLine("");
            }

            else if (Tipo.StartsWith("BOL"))
            {
                //Agregando namespace de DAL
                Clase.AppendLine("using ACCESO_DATOS;");
                Clase.AppendLine("using Entidades;");
            }

            //Escribiendo el código base de la clase
            Clase.AppendLine("");
            Clase.AppendLine($"namespace {namespace_Capa}");
            Clase.AppendLine("{");

            //Determinar si la clase es BOL o DAL
            if (Tipo.StartsWith("AD"))
            {
                Clase.AppendLine($"    public class {tablaBD}{Tipo}");
                CuerpoDALImplementado(Clase, tablaBD);
            }

            else if (Tipo.StartsWith("BOL"))
            {
                Clase.AppendLine($"    public class {tablaBD}{Tipo}");
                CuerpoBOL(Clase, tablaBD);
            }

            //Creando Clase en el directorio indicado
            //Creando archivo .cs
            File.Create(archivo).Close();

            //Escribiendo el cuerpo del archivo .cs
            using (StreamWriter file = new StreamWriter(archivo))
            {
                file.Write(Clase.ToString());
            }

        }

        public void Generar_DAL_BD()
        {
            Consultas aux = new Consultas();
            var lista = aux.ObtenerTablas();
            Tipo = "AD";

            foreach (var item in lista)
            {
                CrearCuerpo(item);
            }
        }

        public void Generar_BOL_BD()
        {
            Consultas aux = new Consultas();
            var lista = aux.ObtenerTablas();
            Tipo = "BOL";

            foreach (var item in lista)
            {
                CrearCuerpo(item);
            }
        }

        //Creacion de cuerpo DAL implementando metodos de entityframework
        public void CuerpoDALImplementado(StringBuilder Clase, string tablaBD)
        {

            Consultas aux = new Consultas();

            Clase.AppendLine("    {");
            Clase.AppendLine($"        public static bool Agregar({tablaBD} ent)");
            Clase.AppendLine("        {");
            Clase.AppendLine("        var r = 0;");
            Clase.AppendLine("            using (var cn = Conexion.ConexionDBSqlServer())");
            Clase.AppendLine("            {");
            Clase.AppendLine($"            SqlCommand cmd = new SqlCommand({Comillas}SPU_INSERTAR_{tablaBD}{Comillas},cn);");
            Clase.AppendLine("            cmd.CommandType = CommandType.StoredProcedure;");

            var lista = Consultas.getParametros(tablaBD, "INSERTAR");

            foreach (var item in lista)
            {
                Clase.AppendLine($"            cmd.Parameters.AddWithValue({Comillas}{item.nombre}{Comillas},ent.{Regex.Replace(item.nombre, "@", "")});");
            }

            Clase.AppendLine("        cn.Open();");
            Clase.AppendLine("        r = cmd.ExecuteNonQuery();");
            Clase.AppendLine("        cn.Close();");
            Clase.AppendLine("        }");
            Clase.AppendLine("        return r > 0;");
            Clase.AppendLine("      }");
            Clase.AppendLine("");
            Clase.AppendLine($"        public static bool Modificar({tablaBD} ent)");
            Clase.AppendLine("        {");
            Clase.AppendLine("        var r = 0;");
            Clase.AppendLine("            using (var cn = Conexion.ConexionDBSqlServer())");
            Clase.AppendLine("            {");
            Clase.AppendLine($"            SqlCommand cmd = new SqlCommand({Comillas}SPU_Modificar_{tablaBD}{Comillas},cn);");
            Clase.AppendLine("            cmd.CommandType = CommandType.StoredProcedure;");

            var listaUpdate = Consultas.getParametros(tablaBD, "MODIFICAR");

            foreach (var item in listaUpdate)
            {
                Clase.AppendLine($"            cmd.Parameters.AddWithValue({Comillas}{item.nombre}{Comillas},ent.{Regex.Replace(item.nombre, "@", "")});");
            }

            Clase.AppendLine("        cn.Open();");
            Clase.AppendLine("        r = cmd.ExecuteNonQuery();");
            Clase.AppendLine("        cn.Close();");
            Clase.AppendLine("        }");
            Clase.AppendLine("        return r > 0;");
            Clase.AppendLine("        }");
            Clase.AppendLine("");

            Clase.AppendLine($"        public static bool Existe(string id)");
            Clase.AppendLine("        {");
            Clase.AppendLine("        var r = 0;");
            Clase.AppendLine("        using (var cn = Conexion.ConexionDBSqlServer())");
            Clase.AppendLine("         {");
            Clase.AppendLine($"         var sql = {Comillas}select count(Id) from {tablaBD} where Id = @Id{Comillas}; ");
            Clase.AppendLine("          var cmd = new SqlCommand(sql, cn);");
            Clase.AppendLine($"          cmd.Parameters.AddWithValue({Comillas}@Id{Comillas}, id);");
            Clase.AppendLine("          cn.Open();");
            Clase.AppendLine("          r = (int)cmd.ExecuteScalar();");
            Clase.AppendLine("          cn.Close();");
            Clase.AppendLine("         }");
            Clase.AppendLine("         return r > 0;");
            Clase.AppendLine("         }");
            Clase.AppendLine("");

            Clase.AppendLine($"        public static bool Eliminar(string id)");
            Clase.AppendLine("        {");
            Clase.AppendLine("        var r = 0;");
            Clase.AppendLine("        using (var cn = Conexion.ConexionDBSqlServer()){");
            Clase.AppendLine($"        var sql = {Comillas}UPDATE {tablaBD} SET Estado='0' WHERE Id =@Id{Comillas};");
            Clase.AppendLine("        var cmd = new SqlCommand(sql, cn);");
            Clase.AppendLine($"        cmd.Parameters.AddWithValue({Comillas}@Id{Comillas}, id);");
            Clase.AppendLine("        cn.Open();");
            Clase.AppendLine("        r = cmd.ExecuteNonQuery();");
            Clase.AppendLine("        cn.Close();");
            Clase.AppendLine("        }");
            Clase.AppendLine("        return r > 0;");
            Clase.AppendLine("        }");
            Clase.AppendLine("");

            Clase.AppendLine("");
            Clase.AppendLine($"        public static {tablaBD} CrearEntidad(IDataReader lector)");
            Clase.AppendLine("        {");
            Clase.AppendLine($"        var aux = new {tablaBD}();");

            var Campos = aux.ObtenerCampos(tablaBD);

            for (int i = 0; i < Campos.Count; i++)
            {
                if (Campos[i].tipoDato.Contains("char") || Campos[i].tipoDato.Contains("nvarchar") || Campos[i].tipoDato.Contains("varchar"))
                {
                    Clase.AppendLine($"        aux.{Campos[i].Nombre} = lector.GetString({i});");
                }
                //Agregar Mas tipos de datos
            }
            Clase.AppendLine($"        return aux;");
            Clase.AppendLine("        }");

            Clase.AppendLine("");
            Clase.AppendLine($"         public static List<{tablaBD}> Buscar({tablaBD} ent)");
            Clase.AppendLine("        {");
            Clase.AppendLine($"        var lista = new List<{tablaBD}>();");
            Clase.AppendLine($"        using (var cn = Conexion.ConexionDBSqlServer())");
            Clase.AppendLine("        {");
            Clase.AppendLine($"                    SqlCommand cmd = new SqlCommand({Comillas}SPU_BUSCAR_{tablaBD}{Comillas}, cn); ");
            Clase.AppendLine("            cmd.CommandType = CommandType.StoredProcedure;");
            Clase.AppendLine("        cn.Open();");
            var listaSelect = Consultas.getParametros(tablaBD, "BUSCAR");


            foreach (var item in listaSelect)
            {

                Clase.AppendLine($"            cmd.Parameters.AddWithValue({Comillas}{item.nombre}{Comillas},ent.{Regex.Replace(Regex.Replace(item.nombre, $"@{tablaBD}", ""), "@", "")});");
            }
            Clase.AppendLine("        var r = cmd.ExecuteReader();");
            Clase.AppendLine("        while (r.Read())");
            Clase.AppendLine("        {");
            Clase.AppendLine("        lista.Add(CrearEntidad(r));");
            Clase.AppendLine("        }");
            Clase.AppendLine("       cn.Close();");
            Clase.AppendLine("        }");
            Clase.AppendLine("        return lista;");
            Clase.AppendLine("        }");


            Clase.AppendLine("");
            Clase.AppendLine($"        public static List<{tablaBD}> ObtenerUltimos()");
            Clase.AppendLine("        {");
            Clase.AppendLine($"        var lista = new List<{tablaBD}>();");
            Clase.AppendLine($"        using (var cn = Conexion.ConexionDBSqlServer())");
            Clase.AppendLine("        {");
            Clase.AppendLine($"                    SqlCommand cmd = new SqlCommand({Comillas}SPU_TOP10_{tablaBD}{Comillas}, cn); ");
            Clase.AppendLine("        cn.Open();");
            Clase.AppendLine("        var r = cmd.ExecuteReader();");
            Clase.AppendLine("        while (r.Read())");
            Clase.AppendLine("        {");
            Clase.AppendLine("        lista.Add(CrearEntidad(r));");
            Clase.AppendLine("        }");
            Clase.AppendLine("       cn.Close();");
            Clase.AppendLine("        }");
            Clase.AppendLine("        return lista;");
            Clase.AppendLine("        }");

            Clase.AppendLine("    }");
            Clase.AppendLine("}");
        }

        //Creacion de cuerpo BOL
        public void CuerpoBOL(StringBuilder Clase, string tablaBD)
        {
            Clase.AppendLine("    {");
            Clase.AppendLine("");
            Clase.AppendLine($"        public static bool Registro({tablaBD} ent)");
            Clase.AppendLine("        {");
            Clase.AppendLine("        bool r;");
            Clase.AppendLine($"                if ({tablaBD}AD.Existe(ent.Id))");
            Clase.AppendLine("                {");
            Clase.AppendLine("        r = true;");
            Clase.AppendLine($"                {tablaBD}AD.Modificar(ent);");
            Clase.AppendLine("                 }");
            Clase.AppendLine("                else");
            Clase.AppendLine("                {");
            Clase.AppendLine("        r = false;");
            Clase.AppendLine($"               {tablaBD}AD.Agregar(ent);");
            Clase.AppendLine("                 }");
            Clase.AppendLine("        return r;");
            Clase.AppendLine("        }");

            Clase.AppendLine($"        public static void Eliminar(string id)");
            Clase.AppendLine("        {");
            Clase.AppendLine($"                {tablaBD}AD.Eliminar(id);");
            Clase.AppendLine("        }");

            Clase.AppendLine("");
            Clase.AppendLine($"         public static List<{tablaBD}> BuscarUltimos()");
            Clase.AppendLine("        {");
            Clase.AppendLine($"                return {tablaBD}AD.ObtenerUltimos();");
            Clase.AppendLine("        }");

            Clase.AppendLine("");
            Clase.AppendLine($"         public static List<{tablaBD}> Buscar({tablaBD} ent)");
            Clase.AppendLine("        {");
            Clase.AppendLine($"                return {tablaBD}AD.Buscar(ent);");
            Clase.AppendLine("        }");

            Clase.AppendLine("    }");
            Clase.AppendLine("}");
        }
    }
}