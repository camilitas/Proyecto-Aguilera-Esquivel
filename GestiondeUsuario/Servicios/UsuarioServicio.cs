using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BE;

namespace Servicios
{
    public class UsuarioServicio
    {
        private static UsuarioServicio _instancia;

        private UsuarioServicio() { }

        public static UsuarioServicio Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new UsuarioServicio();
                return _instancia;
            }
        }

        public bool Login(string email, string contraseña)
        {
            return UsuarioBLL.Instancia.Login(email, contraseña);
        }

        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            return UsuarioBLL.Instancia.CrearUsuario(nuevoUsuario);
        }

        public bool RecuperarContraseña(string email, string contraseñaActual, string nuevaPass)
        {
            return UsuarioBLL.Instancia.RecuperarContraseña(email, contraseñaActual, nuevaPass);
        }

        // SessionManager expuesto desde Servicios
        public void CerrarSesion()
        {
            SessionManagerBLL.Instancia.CerrarSesion();
        }

        public Usuario ObtenerUsuarioActivo()
        {
            return SessionManagerBLL.Instancia.ObtenerUsuarioActivo();
        }

        public bool HaySesionActiva()
        {
            return SessionManagerBLL.Instancia.HaySesionActiva();
        }
    }
}
