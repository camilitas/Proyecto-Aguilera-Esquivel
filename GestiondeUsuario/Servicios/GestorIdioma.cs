using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace Servicios
{
    public class GestorIdioma
    {
        private static GestorIdioma _instancia;
        private JObject _traducciones;
        private List<IObservadorIdioma> _observadores = new List<IObservadorIdioma>();

        private GestorIdioma() { }

        public static GestorIdioma Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new GestorIdioma();
                return _instancia;
            }
        }

        public void Suscribir(IObservadorIdioma obs)
        {
            _observadores.Add(obs);
        }

        public void Desuscribir(IObservadorIdioma obs)
        {
            _observadores.Remove(obs);
        }

        public void CambiarIdioma(string idioma)
        {
            // Cargamos el JSON del idioma
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Idiomas", idioma + ".json");
            string json = File.ReadAllText(ruta);
            _traducciones = JObject.Parse(json);

            // Guardamos en SessionManager
            SessionManager.Instancia.CambiarIdioma(idioma);

            // Notificamos a todos los observadores
            foreach (var obs in _observadores)
                obs.ActualizarIdioma(_traducciones);
        }

        public string Obtener(string form, string clave)
        {
            if (_traducciones == null) return clave;
            return _traducciones[form]?[clave]?.ToString() ?? clave;
        }
    }
}

