using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class EncriptadorBLL
    {
        public static string Encriptar(string texto) //metodo estatico que recibe un texto (la contraseña) y devuelve un string (el hash encriptado). Es estatico para poder llamarlo sin crear un objeto.
        {
            using (SHA256 sha256 = SHA256.Create()) //Crea una instancia del algoritmo SHA256. El using asegura que se liberen los recursos correctamente.
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);  //convierte la contraseña en un array de bytes
                byte[] hash = sha256.ComputeHash(bytes); //calcula el hash de la contraseña utilizando el algoritmo SHA256. El resultado es un array de bytes que representa el hash encriptado.

                StringBuilder resultado = new StringBuilder();
                foreach (byte b in hash)
                    resultado.Append(b.ToString("x2"));  //convierte cada byte del hash en una representación hexadecimal de dos dígitos y la agrega al StringBuilder. El formato "x2" asegura que cada byte se represente con dos caracteres hexadecimales, incluso si el valor es menor a 16 (0x10).

                return resultado.ToString(); //devuelve el hash encriptado como una cadena de texto. El resultado es una cadena hexadecimal que representa el hash de la contraseña. (64 caracteres)
            }
        }

        public static bool ContraseñaSegura(string contraseña)
        {
            if (contraseña.Length < 8) // Verifica que la contraseña tenga al menos 8 caracteres
                return false;
            if (!contraseña.Any(char.IsUpper)) // Verifica que la contraseña contenga al menos una letra mayúscula
                return false;
            if (!contraseña.Any(char.IsDigit)) // Verifica que la contraseña contenga al menos un número
                return false;
            return true;
        }
    }
}
