using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Sesion //clase sesion para almacenar información de la sesión activa del usuario
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public DateTime HoraIngreso { get; set; }
    }
}
