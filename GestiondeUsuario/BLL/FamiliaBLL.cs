using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FamiliaBLL
    {
        private static FamiliaBLL _instancia;
        private FamiliaBLL() { }
        public static FamiliaBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new FamiliaBLL();
                return _instancia;
            }
        }

        public List<Familia> ObtenerTodos()
        {
            FamiliaDAL dal = new FamiliaDAL();
            return dal.ObtenerTodos();
        }

        public bool Insertar(Familia f)
        {
            if (string.IsNullOrEmpty(f.Nombre))
                return false;
            FamiliaDAL dal = new FamiliaDAL();
            return dal.Insertar(f);
        }

        public bool Modificar(Familia f)
        {
            if (string.IsNullOrEmpty(f.Nombre))
                return false;
            FamiliaDAL dal = new FamiliaDAL();
            return dal.Modificar(f);
        }

        public bool Eliminar(int id)
        {
            FamiliaDAL dal = new FamiliaDAL();
            if (dal.EstaEnUso(id))
                throw new Exception("No se puede eliminar una familia que está siendo utilizada por un rol.");
            return dal.Eliminar(id);
        }

        public List<Patente> ObtenerPatentes(int idFamilia)
        {
            FamiliaDAL dal = new FamiliaDAL();
            return dal.ObtenerPatentes(idFamilia);
        }

        public List<Familia> ObtenerFamiliasIntegradas(int idFamilia)
        {
            FamiliaDAL dal = new FamiliaDAL();
            return dal.ObtenerFamiliasIntegradas(idFamilia);
        }

        public bool AgregarPatente(int idFamilia, int idPatente)
        {
            FamiliaDAL dal = new FamiliaDAL();

            // Verificamos que no esté ya directo en esta familia
            var patentes = dal.ObtenerPatentes(idFamilia);
            if (patentes.Any(p => p.Id == idPatente))
                throw new Exception("Esta patente ya está en la familia.");

            // Verificamos que ninguna familia integrada ya la tenga
            var familiasIntegradas = dal.ObtenerFamiliasIntegradas(idFamilia);
            foreach (var familiaIntegrada in familiasIntegradas)
            {
                if (TienePatenteRecursivo(familiaIntegrada.Id, idPatente, dal))
                {
                    var patente = PatenteBLL.Instancia.ObtenerTodos()
                        .FirstOrDefault(p => p.Id == idPatente);
                    throw new Exception(
                        $"La patente '{patente?.Nombre}' ya está incluida en la familia integrada '{familiaIntegrada.Nombre}'.");
                }
            }



            return dal.AgregarPatente(idFamilia, idPatente);
        }

        private bool TienePatenteRecursivo(int idFamilia, int idPatente, FamiliaDAL dal)
        {
            var patentes = dal.ObtenerPatentes(idFamilia);
            if (patentes.Any(p => p.Id == idPatente))
                return true;

            var familiasIntegradas = dal.ObtenerFamiliasIntegradas(idFamilia);
            foreach (var f in familiasIntegradas)
            {
                if (TienePatenteRecursivo(f.Id, idPatente, dal))
                    return true;
            }
            return false;
        }

        public bool AgregarFamilia(int idFamilia, int idFamiliaIntegrada)
        {
            if (idFamilia == idFamiliaIntegrada)
                throw new Exception("Una familia no puede contenerse a sí misma.");

            FamiliaDAL dal = new FamiliaDAL();
            var familias = dal.ObtenerFamiliasIntegradas(idFamilia);
            if (familias.Any(f => f.Id == idFamiliaIntegrada))
                throw new Exception("Esta familia ya está integrada.");

            return dal.AgregarFamilia(idFamilia, idFamiliaIntegrada);
        }

        public bool EliminarPatente(int idFamilia, int idPatente)
        {
            FamiliaDAL dal = new FamiliaDAL();
            return dal.EliminarPatente(idFamilia, idPatente);
        }

        public bool EliminarFamiliaIntegrada(int idFamilia, int idFamiliaIntegrada)
        {
            FamiliaDAL dal = new FamiliaDAL();
            return dal.EliminarFamiliaIntegrada(idFamilia, idFamiliaIntegrada);
        }

        public int ObtenerUltimoId()
        {
            FamiliaDAL dal = new FamiliaDAL();
            return dal.ObtenerUltimoId();
        }
    }
}

