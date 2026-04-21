using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario //representa la entidad Usuario de la base de datos 
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Contraseña { get; set; }

        public int DNI { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool Activo { get; set; }

        public int IntentosFallidos { get; set; }
        public bool Bloqueado { get; set; }
    }
}
