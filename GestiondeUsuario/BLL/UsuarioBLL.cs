using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class UsuarioBLL
    {
        private static UsuarioBLL _instancia; //patron singleton para asegurar que solo haya una instancia de UsuarioBLL en toda la aplicación

        public static UsuarioBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new UsuarioBLL();
                return _instancia;
            }
        }

        public bool Login(string email, string contraseña)
        {
            UsuarioDAL dal = new UsuarioDAL();
            Usuario usuario = dal.ObtenerPorEmailYContraseña(email, contraseña);
            return usuario != null;
        }

        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            if (string.IsNullOrEmpty(nuevoUsuario.Nombre))
                return false;
            if (string.IsNullOrEmpty(nuevoUsuario.Email))
                return false;

            UsuarioDAL dal = new UsuarioDAL();
            return dal.Insertar(nuevoUsuario);
        }
    }
}
