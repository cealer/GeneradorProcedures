using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procedures
{
    public class RelacionTabla
    {
        public string FK_Tabla { get; set; }
        public string FK_Columna { get; set; }
        public string PK_Tabla { get; set; }
        public string PK_Columna { get; set; }

        public RelacionTabla(string fk_tabla, string fk_colum, string pk_tabla, string pk_colum)
        {
            FK_Tabla = fk_colum;
            FK_Columna = fk_colum;
            PK_Tabla = pk_tabla;
            PK_Columna = pk_colum;
        }
    }
}