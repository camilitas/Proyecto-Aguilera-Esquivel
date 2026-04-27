using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestiondeUsuario
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Suscribir la bitácora al gestor de eventos (Observer)
            GestorEventosBLL.Instancia.Suscribir(new BitacoraDAL());

            Application.Run(new Form1());
        }
    }
}
