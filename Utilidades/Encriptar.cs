using System.Security.Cryptography;
using System.Text;

namespace Utilidades
{
    public class encriptar
    {
        public string HashMetodo(string contra)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(contra);
            byte[] hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
        public string hashUsuarioContra(string usuario, string contra)
        {
            encriptar enc = new encriptar();
            return HashMetodo(usuario + contra);
        }
    }
}
