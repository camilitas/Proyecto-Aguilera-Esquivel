using System;
using System.Collections.Generic;
using DAL;
using Servicios;

namespace BLL
{
    public class UsuarioBLL
    {
        private static UsuarioBLL _instancia;

        private UsuarioBLL() { } // constructor privado — corregido del original

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
            Usuario usuario = dal.ObtenerPorNombreUsuario(nombreUsuario);

            if (usuario == null)
            {
                GestorEventosBLL.Instancia.Notificar(nombreUsuario,
                    "Login fallido - usuario no existe", "Usuarios", 1);
                return false;
            }

            if (usuario.Bloqueado)
            {
                GestorEventosBLL.Instancia.Notificar(nombreUsuario,
                    "Intento en cuenta bloqueada", "Usuarios", 1);
                throw new Exception("Usuario bloqueado");
            }

            if (usuario.Contraseña == passEnc)
            {
                usuario.IntentosFallidos = 0;
                dal.ActualizarIntentos(usuario);
                SessionManager.Instancia.IniciarSesion(usuario);
                GestorEventosBLL.Instancia.Notificar(nombreUsuario,
                    "Login", "Usuarios", 1);
                return true;
            }
            else
            {
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 3)
                    usuario.Bloqueado = true;

                dal.ActualizarIntentos(usuario);
                GestorEventosBLL.Instancia.Notificar(nombreUsuario,
                    "Login fallido", "Usuarios", 1);
                return false;
            }
        }

        public bool CambiarContraseña(int dni, string contraseñaActual, string nuevaPass)
        {
            UsuarioDAL dal = new UsuarioDAL();

            Usuario usuario = dal.ObtenerPorDNI(dni);
            if (usuario == null)
                return false;

            if (usuario.Contraseña != EncriptadorBLL.Encriptar(contraseñaActual))
                return false;

            usuario.Contraseña = EncriptadorBLL.Encriptar(nuevaPass);
            usuario.PrimerIngreso = false;
            bool ok = dal.ActualizarContraseña(usuario);

            if (ok)
                GestorEventosBLL.Instancia.Notificar(usuario.NombreUsuario,
                    "Cambiar Clave", "Usuarios", 2);

            return ok;
        }

        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            if (string.IsNullOrEmpty(nuevoUsuario.Nombre))
                return false;
            if (string.IsNullOrEmpty(nuevoUsuario.Email))
                return false;
            if (string.IsNullOrEmpty(nuevoUsuario.Contraseña))
                return false;

            nuevoUsuario.Contraseña = EncriptadorBLL.Encriptar(nuevoUsuario.Contraseña);

            UsuarioDAL dal = new UsuarioDAL();
            bool ok = dal.Insertar(nuevoUsuario);

            if (ok)
                GestorEventosBLL.Instancia.Notificar(
                    SessionManager.Instancia.ObtenerUsuarioActivo()?.NombreUsuario ?? "Admin",
                    "Crear Usuario", "Usuarios", 2);

            return ok;
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
            bool ok = dal.Modificar(u);

            if (ok)
                GestorEventosBLL.Instancia.Notificar(
                    SessionManager.Instancia.ObtenerUsuarioActivo()?.NombreUsuario ?? "Admin",
                    "Modificar Usuario", "Usuarios", 3);

            return ok;
        }

        public bool Deshabilitar(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            bool ok = dal.Deshabilitar(id);

            if (ok)
                GestorEventosBLL.Instancia.Notificar(
                    SessionManager.Instancia.ObtenerUsuarioActivo()?.NombreUsuario ?? "Admin",
                    "Deshabilitar Usuario", "Usuarios", 2);

            return ok;
        }

        public bool Habilitar(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            bool ok = dal.Habilitar(id);

            if (ok)
                GestorEventosBLL.Instancia.Notificar(
                    SessionManager.Instancia.ObtenerUsuarioActivo()?.NombreUsuario ?? "Admin",
                    "Habilitar Usuario", "Usuarios", 2);

            return ok;
        }

        public bool Desbloquear(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            bool ok = dal.Desbloquear(id);

            if (ok)
                GestorEventosBLL.Instancia.Notificar(
                    SessionManager.Instancia.ObtenerUsuarioActivo()?.NombreUsuario ?? "Admin",
                    "Bloquear Usuario", "Usuarios", 1);

            return ok;
        }

        public void Logout()
        {
            string nombreUsuario = SessionManager.Instancia
                .ObtenerUsuarioActivo()?.NombreUsuario ?? "Desconocido";

            GestorEventosBLL.Instancia.Notificar(nombreUsuario,
                "Logout", "Usuarios", 1);

            SessionManager.Instancia.CerrarSesion();
        }
    }
}