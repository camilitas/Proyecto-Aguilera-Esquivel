using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BitacoraBLL
    {
        private static BitacoraBLL _instancia;

        private BitacoraBLL() { }

        public static BitacoraBLL Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new BitacoraBLL();
                return _instancia;
            }
        }

        public List<Bitacora> ObtenerFiltrado(string login, DateTime? fechaIni,
            DateTime? fechaFin, string modulo, string evento, int? criticidad)
        {
            BitacoraDAL dal = new BitacoraDAL();
            return dal.ObtenerFiltrado(login, fechaIni, fechaFin, modulo, evento, criticidad);
        }

        public List<string> ObtenerLogins()
        {
            BitacoraDAL dal = new BitacoraDAL();
            return dal.ObtenerLogins();
        }
    }
}
