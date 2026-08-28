using System.Collections.Generic;
using System.Linq;
using CapaDatos;
using Entidades;
using Utilidades;

namespace CapaLogica
{
    public class CL_servicioContras
    {
        private CD_contraDatos contraDatos = new CD_contraDatos();
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private CD_configuracionDatos configDatos = new CD_configuracionDatos();
        private encriptar encriptar = new encriptar();
        string caracteresEspeciales = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?";

        private configuracionSistema ObtenerConfiguracionConDefault()
        {
            configuracionSistema config = configDatos.obtenerConfiguracion();

            if (config == null)
            {
                config = new configuracionSistema
                {
                    minCaracteres = 8,
                    cantPreguntas = 3,
                    requiereMayusculas = true,
                    requiereNumeros = true,
                    requiereEspeciales = true,
                    requiere2FA = false,
                    noRepetirPasswords = true,
                    validarDatosPersonales = true
                };
            }

            return config;
        }

        public List<string> RequisitosIncumplidos(string contra, string usuario)
        {
            configuracionSistema config = ObtenerConfiguracionConDefault();
            List<string> faltantes = new List<string>();

            if (string.IsNullOrWhiteSpace(contra))
            {
                faltantes.Add($"Debe tener al menos {config.minCaracteres} caracteres.");
                return faltantes;
            }

            if (contra.Length < config.minCaracteres)
                faltantes.Add($"Debe tener al menos {config.minCaracteres} caracteres.");

            if (config.requiereMayusculas && !contra.Any(char.IsUpper))
                faltantes.Add("Debe incluir al menos una mayúscula.");

            if (config.requiereMinusculas && !contra.Any(char.IsLower))
                faltantes.Add("Debe incluir al menos una minúscula.");

            if (config.requiereNumeros && !contra.Any(char.IsDigit))
                faltantes.Add("Debe incluir al menos un número.");

            if (config.requiereEspeciales && !contra.Any(c => caracteresEspeciales.Contains(c)))
                faltantes.Add("Debe incluir al menos un carácter especial (!@#$%^&*, etc.).");

            if (config.validarDatosPersonales && !string.IsNullOrWhiteSpace(usuario) && contra.ToLower().Contains(usuario.ToLower()))
                faltantes.Add("No puede contener tu nombre de usuario.");

            return faltantes;
        }

        public string DescribirRequisitos()
        {
            configuracionSistema config = ObtenerConfiguracionConDefault();
            List<string> requisitos = new List<string> { $"Al menos {config.minCaracteres} caracteres" };

            if (config.requiereMayusculas)
                requisitos.Add("una mayúscula");

            if (config.requiereMinusculas)
                requisitos.Add("una minúscula");

            if (config.requiereNumeros)
                requisitos.Add("un número");

            if (config.requiereEspeciales)
                requisitos.Add("un carácter especial");

            if (config.validarDatosPersonales)
                requisitos.Add("no puede contener tu usuario");

            return "Requisitos: " + string.Join(", ", requisitos) + ".";
        }

        public bool cumplePoliticasContra(string contra, string usuario)
        {
            return RequisitosIncumplidos(contra, usuario).Count == 0;
        }

        public bool cambiarContra(string usuario, string nuevaContra)
        {
            int idUsuario = usuarioDatos.BuscarUsuarioPorNombreUser(usuario);

            if (idUsuario == -1)
                return false;

            if (!cumplePoliticasContra(nuevaContra, usuario))
                return false;

            string hash = encriptar.hashUsuarioContra(usuario, nuevaContra);

            return contraDatos.CambiarContra(idUsuario, hash);
        }
    }
}
