using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RolBLL
    {
        private static RolBLL _instancia;
        private RolBLL() { }
        public static RolBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new RolBLL();
                return _instancia;
            }
        }

        public List<Rol> ObtenerTodos()
        {
            RolDAL dal = new RolDAL();
            return dal.ObtenerTodos();
        }

        public bool Insertar(Rol r)
        {
            if (string.IsNullOrEmpty(r.Nombre))
                return false;
            RolDAL dal = new RolDAL();
            return dal.Insertar(r);
        }

        public bool Modificar(Rol r)
        {
            if (string.IsNullOrEmpty(r.Nombre))
                return false;
            RolDAL dal = new RolDAL();
            return dal.Modificar(r);
        }

        public bool Eliminar(int id)
        {
            RolDAL dal = new RolDAL();
            if (dal.EstaEnUso(id))
                throw new Exception("No se puede eliminar un rol que está siendo utilizado por un usuario.");
            // Limpiamos las tablas intermedias primero
            foreach (var p in dal.ObtenerPatentes(id))
                dal.EliminarPatente(id, p.Id);
            foreach (var f in dal.ObtenerFamilias(id))
                dal.EliminarFamilia(id, f.Id);
            return dal.Eliminar(id);
        }

        public bool AgregarPatente(int idRol, int idPatente)
        {
            RolDAL dal = new RolDAL();
            FamiliaDAL familiaDAL = new FamiliaDAL();
            // Verificamos que no esté ya directo en el rol
            var patentes = dal.ObtenerPatentes(idRol);
            if (patentes.Any(p => p.Id == idPatente))
                throw new Exception("Esta patente ya está asignada directamente al rol.");

            // Verificamos que ninguna familia del rol ya tenga esa patente
            var familias = dal.ObtenerFamilias(idRol);
            foreach (var familia in familias)
            {
                var patentesDeFamilia = familiaDAL.ObtenerPatentes(familia.Id);
                if (patentesDeFamilia.Any(p => p.Id == idPatente))
                    throw new Exception($"La patente ya está incluida en la familia '{familia.Nombre}' que tiene este rol.");
            }

            return dal.AgregarPatente(idRol, idPatente);
        }

        public bool AgregarFamilia(int idRol, int idFamilia)
        {
            RolDAL dal = new RolDAL();
            var familias = dal.ObtenerFamilias(idRol);
            if (familias.Any(f => f.Id == idFamilia))
                throw new Exception("Esta familia ya está en el rol.");
            return dal.AgregarFamilia(idRol, idFamilia);
        }

        public bool EliminarPatente(int idRol, int idPatente)
        {
            RolDAL dal = new RolDAL();
            return dal.EliminarPatente(idRol, idPatente);
        }

        public bool EliminarFamilia(int idRol, int idFamilia)
        {
            RolDAL dal = new RolDAL();
            return dal.EliminarFamilia(idRol, idFamilia);
        }

        public List<Patente> ObtenerPatentes(int idRol)
        {
            RolDAL dal = new RolDAL();
            return dal.ObtenerPatentes(idRol);
        }

        public List<Familia> ObtenerFamilias(int idRol)
        {
            RolDAL dal = new RolDAL();
            return dal.ObtenerFamilias(idRol);
        }
    }
}

