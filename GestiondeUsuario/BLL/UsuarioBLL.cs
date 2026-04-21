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
            string passEnc = EncriptadorBLL.Encriptar(contraseña);

            UsuarioDAL dal = new UsuarioDAL();
            BitacoraDAL bitacora = new BitacoraDAL();

            Usuario usuario = dal.ObtenerPorEmail(email);

            if (usuario == null)
            {
                bitacora.Guardar(email, "Login fallido - usuario no existe");
                return false;
            }

            if (usuario.Bloqueado)
            {
                bitacora.Guardar(email, "Intento en cuenta bloqueada");
                throw new Exception("Usuario bloqueado");
            }

            if (usuario.Contraseña == passEnc)
            {
                usuario.IntentosFallidos = 0;
                dal.ActualizarIntentos(usuario);

                SessionManagerBLL.Instancia.IniciarSesion(usuario);

                bitacora.Guardar(email, "Login exitoso");
                return true;
            }
            else
            {
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 3)
                    usuario.Bloqueado = true;

                dal.ActualizarIntentos(usuario);

                bitacora.Guardar(email, "Login fallido");

                return false;
            }
        }

        public bool RecuperarContraseña(string email, string nuevaPass, string contraseñaActual)
        {
            UsuarioDAL dal = new UsuarioDAL();

            Usuario usuario = dal.ObtenerPorEmail(email);

            if (usuario == null)
                return false;

            // Verificamos que la contraseña actual sea correcta
            if (usuario.Contraseña != EncriptadorBLL.Encriptar(contraseñaActual))
                return false;

            usuario.Contraseña = EncriptadorBLL.Encriptar(nuevaPass);

            return dal.ActualizarContraseña(usuario);
        }

        // validaciones: ningun campo puede estar vacio
        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            if (string.IsNullOrEmpty(nuevoUsuario.Nombre))
                return false;
            if (string.IsNullOrEmpty(nuevoUsuario.Email))
                return false;
            if (string.IsNullOrEmpty(nuevoUsuario.Contraseña))
                return false;

            // antes de guardar, encriptamos la contraseña
            // a partir de aca el objeto ya tiene la contraseña encriptada
            nuevoUsuario.Contraseña = EncriptadorBLL.Encriptar(nuevoUsuario.Contraseña); //encripta la contraseña del nuevo usuario utilizando el método Encriptar de la clase EncriptadorBLL. Esto asegura que la contraseña se almacene de forma segura en la base de datos.

            //le pedimos a dal que inserte el nuevo usuario en la bd
            UsuarioDAL dal = new UsuarioDAL();
            return dal.Insertar(nuevoUsuario);
        }
    }
}
