using System.Security.Cryptography;
using System.Text;

namespace Utilidades
{
    public static class CifradoAes
    {
        private static readonly string CarpetaClave =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SistemaPP");

        private static readonly string RutaClave = Path.Combine(CarpetaClave, "mail.key");

        private static byte[] ObtenerClave()
        {
            if (!Directory.Exists(CarpetaClave))
                Directory.CreateDirectory(CarpetaClave);

            if (!File.Exists(RutaClave))
            {
                using Aes aes = Aes.Create();
                aes.KeySize = 256;
                aes.GenerateKey();
                File.WriteAllBytes(RutaClave, aes.Key);
            }

            return File.ReadAllBytes(RutaClave);
        }

        public static (byte[] passwordCifrada, byte[] iv) Cifrar(string textoPlano)
        {
            using Aes aes = Aes.Create();
            aes.Key = ObtenerClave();
            aes.GenerateIV();

            using ICryptoTransform encriptador = aes.CreateEncryptor();
            byte[] bytesPlano = Encoding.UTF8.GetBytes(textoPlano);
            byte[] bytesCifrados = encriptador.TransformFinalBlock(bytesPlano, 0, bytesPlano.Length);

            return (bytesCifrados, aes.IV);
        }

        public static string Descifrar(byte[] passwordCifrada, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = ObtenerClave();
            aes.IV = iv;

            using ICryptoTransform desencriptador = aes.CreateDecryptor();
            byte[] bytesPlano = desencriptador.TransformFinalBlock(passwordCifrada, 0, passwordCifrada.Length);

            return Encoding.UTF8.GetString(bytesPlano);
        }
    }
}
