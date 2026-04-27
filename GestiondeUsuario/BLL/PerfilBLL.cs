using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PerfilBLL
    {
        private static PerfilBLL _instancia;
        private Dictionary<string, Perfil> _perfiles = new Dictionary<string, Perfil>();

        private PerfilBLL()
        {
            InicializarPerfiles();
        }

        public static PerfilBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new PerfilBLL();
                return _instancia;
            }
        }

        private void InicializarPerfiles()
        {
            // Perfil General
            var perfilGeneral = new Perfil("General");
            perfilGeneral.Agregar(new Permiso("CambiarContraseña"));
            perfilGeneral.Agregar(new Permiso("VerPrincipal"));

            // Perfil Admin contiene todo lo de General + más permisos
            var perfilAdmin = new Perfil("Admin");
            perfilAdmin.Agregar(new Permiso("CambiarContraseña"));
            perfilAdmin.Agregar(new Permiso("VerPrincipal"));
            perfilAdmin.Agregar(new Permiso("GestionUsuarios"));
            perfilAdmin.Agregar(new Permiso("VerBitacora"));
            perfilAdmin.Agregar(perfilGeneral); // Composite: admin incluye general

            _perfiles["General"] = perfilGeneral;
            _perfiles["Admin"] = perfilAdmin;
        }

        public bool TienePermiso(string rol, string permiso)
        {
            if (!_perfiles.ContainsKey(rol))
                return false;
            return _perfiles[rol].TieneAcceso(permiso);
        }
    }
}
