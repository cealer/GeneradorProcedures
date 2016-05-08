using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procedures
{
   public class Tabla
    {
        public string Nombre { get; private set; }
        public string tipoDato { get; set; }
        public string length { get; set; }
        public string TipoNull { get; set; }

        public Tabla()
        {

        }

        public Tabla(string nom, string tip, string len, string nu)
        {
            this.Nombre = nom;
            this.tipoDato = tip;
            this.length = len;
            this.TipoNull = nu;
        }

        public override string ToString() => Nombre + " " + tipoDato + " " + length + " " + TipoNull;


        public List<Tabla> ListaAtributos()
        {
            List<Tabla> lista = new List<Tabla>();
            return lista;
        }
    }
}
