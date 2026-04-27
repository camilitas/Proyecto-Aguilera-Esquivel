using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Servicios
    {
        public class Permiso : IComponentePermiso
        {
            public string Nombre { get; set; }

            public Permiso(string nombre)
            {
                Nombre = nombre;
            }

            public bool TieneAcceso(string permiso)
            {
                return Nombre == permiso;
            }
        }
    }

