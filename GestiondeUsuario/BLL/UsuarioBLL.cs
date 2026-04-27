using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Servicios;

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

        public bool Login(string nombreUsuario, string contraseña)
        {
            string passEnc = EncriptadorBLL.Encriptar(contraseña);
            UsuarioDAL dal = new UsuarioDAL();
            BitacoraDAL bitacora = new BitacoraDAL();
            Usuario usuario = dal.ObtenerPorNombreUsuario(nombreUsuario);

            if (usuario == null)
            {
                bitacora.Guardar(nombreUsuario, "Login fallido - usuario no existe");
                return false;
            }

            if (usuario.Bloqueado)
            {
                bitacora.Guardar(nombreUsuario, "Intento en cuenta bloqueada");
                throw new Exception("Usuario bloqueado");
            }

            if (usuario.Contraseña == passEnc)
            {
                usuario.IntentosFallidos = 0;
                dal.ActualizarIntentos(usuario);
                SessionManager.Instancia.IniciarSesion(usuario);
                bitacora.Guardar(nombreUsuario, "Login exitoso");

                // Si es primer ingreso devolvemos true pero marcamos la flag
                return true;
            }
            else
            {
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 3)
                    usuario.Bloqueado = true;

                dal.ActualizarIntentos(usuario);

                bitacora.Guardar(nombreUsuario, "Login fallido");

                return false;
            }
        }

        public bool CambiarContraseña(int dni, string contraseñaActual, string nuevaPass)
        {
            UsuarioDAL dal = new UsuarioDAL();
            BitacoraDAL bitacora = new BitacoraDAL();

            Usuario usuario = dal.ObtenerPorDNI(dni);
            if (usuario == null)
                return false;

            if (usuario.Contraseña != EncriptadorBLL.Encriptar(contraseñaActual))
                return false;

            usuario.Contraseña = EncriptadorBLL.Encriptar(nuevaPass);
            usuario.PrimerIngreso = false;
            bool ok = dal.ActualizarContraseña(usuario);

            if (ok)
                bitacora.Guardar(usuario.NombreUsuario, "Cambio de contraseña exitoso");

            return ok;
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
        public List<Usuario> ObtenerTodos()
        {
            UsuarioDAL dal = new UsuarioDAL();
            return dal.ObtenerTodos();
        }
        public List<Usuario> ObtenerActivos()
        {
            UsuarioDAL dal = new UsuarioDAL();
            return dal.ObtenerActivos();
        }
        public bool Modificar(Usuario u)
        {
            UsuarioDAL dal = new UsuarioDAL();
            return dal.Modificar(u);
        }
        public bool Deshabilitar(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            BitacoraDAL bitacora = new BitacoraDAL();
            bool ok = dal.Deshabilitar(id);
            if (ok)
                bitacora.Guardar("Admin", "Usuario deshabilitado - Id: " + id);
            return ok;
        }
        public bool Habilitar(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            BitacoraDAL bitacora = new BitacoraDAL();
            bool ok = dal.Habilitar(id);
            if (ok)
                bitacora.Guardar("Admin", "Usuario habilitado - Id: " + id);
            return ok;
        }
        public bool Desbloquear(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            BitacoraDAL bitacora = new BitacoraDAL();
            bool ok = dal.Desbloquear(id);
            if (ok)
                bitacora.Guardar("Admin", "Usuario desbloqueado - Id: " + id);
            return ok;
        }
    }
}
