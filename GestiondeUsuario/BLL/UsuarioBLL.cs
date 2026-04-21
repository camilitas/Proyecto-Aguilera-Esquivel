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
            string contraseñaEncriptada = EncriptadorBLL.Encriptar(contraseña); //encripta la contraseña ingresada por el usuario utilizando el método Encriptar de la clase EncriptadorBLL. Esto asegura que la contraseña se compare de forma segura con la versión encriptada almacenada en la base de datos.
            UsuarioDAL dal = new UsuarioDAL();
            Usuario usuario = dal.ObtenerPorEmailYContraseña(email, contraseñaEncriptada);
            return usuario != null;
        }

        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            if (string.IsNullOrEmpty(nuevoUsuario.Nombre))
                return false;
            if (string.IsNullOrEmpty(nuevoUsuario.Email))
                return false;
            if (string.IsNullOrEmpty(nuevoUsuario.Contraseña))
                return false;

            nuevoUsuario.Contraseña = EncriptadorBLL.Encriptar(nuevoUsuario.Contraseña); //encripta la contraseña del nuevo usuario utilizando el método Encriptar de la clase EncriptadorBLL. Esto asegura que la contraseña se almacene de forma segura en la base de datos.

            UsuarioDAL dal = new UsuarioDAL();
            return dal.Insertar(nuevoUsuario);
        }
    }
}
