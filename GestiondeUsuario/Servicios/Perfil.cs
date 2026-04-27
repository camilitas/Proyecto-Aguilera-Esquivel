using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Perfil : IComponentePermiso
    {
        public string Nombre { get; set; }
        private List<IComponentePermiso> _componentes = new List<IComponentePermiso>();

        public Perfil(string nombre)
        {
            Nombre = nombre;
        }

        public void Agregar(IComponentePermiso componente)
        {
            _componentes.Add(componente);
        }

        public void Quitar(IComponentePermiso componente)
        {
            _componentes.Remove(componente);
        }

        public bool TieneAcceso(string permiso)
        {
            return _componentes.Any(c => c.TieneAcceso(permiso));
        }
    }
}
