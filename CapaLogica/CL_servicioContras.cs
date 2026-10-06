using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;
using CapaDatos;
using Entidades;

namespace CapaLogica
{
    public class CL_servicioContras
    {
        private CD_contraDatos contraDatos = new CD_contraDatos();
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private CD_configuracionDatos configDatos = new CD_configuracionDatos();
        private encriptar encriptar = new encriptar();
        string caracteresEspeciales = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?¡¿";

        private configuracionSistema obtenerConfiguracion()
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

        // Devuelve "" si la contraseña cumple todas las politicas,
        // o el motivo por el que no las cumple.
        public string validarContra(string contra, string usuario)
        {
            configuracionSistema config = obtenerConfiguracion();

            if (string.IsNullOrWhiteSpace(contra))
                return "Ingrese una contraseña.";

            if (contra.Length < config.minCaracteres)
                return $"La contraseña debe tener al menos {config.minCaracteres} caracteres.";

            if (config.requiereMayusculas && !contra.Any(char.IsUpper))
                return "La contraseña debe tener al menos una letra mayúscula.";

            if (config.requiereMinusculas && !contra.Any(char.IsLower))
                return "La contraseña debe tener al menos una letra minúscula.";

            if (config.requiereNumeros && !contra.Any(char.IsDigit))
                return "La contraseña debe tener al menos un número.";

            if (config.requiereEspeciales && !contra.Any(c => caracteresEspeciales.Contains(c)))
                return "La contraseña debe tener al menos un caracter especial (por ejemplo: ! @ # $ % & *).";

            string usuarioExacto = usuarioDatos.ObtenerNombreUsuarioExacto(usuario);

            if (string.IsNullOrEmpty(usuarioExacto))
                return "No se pudo identificar el usuario.";

            if (config.validarDatosPersonales)
            {
                string contraMin = contra.ToLower();

                if (contraMin.Contains(usuarioExacto.ToLower()))
                    return "La contraseña no puede contener el nombre de usuario.";

                foreach (string dato in usuarioDatos.ObtenerDatosPersonales(usuarioExacto))
                {
                    if (dato.Trim().Length >= 3 && contraMin.Contains(dato.Trim().ToLower()))
                        return "La contraseña no puede contener datos personales (nombre, apellido o DNI).";
                }
            }

            if (config.noRepetirPasswords)
            {
                int idUsuario = usuarioDatos.BuscarUsuarioPorNombreUser(usuarioExacto);
                string hash = encriptar.hashUsuarioContra(usuarioExacto, contra);

                if (contraDatos.obtenerContrasAnteriores(idUsuario).Contains(hash))
                    return "La contraseña ya fue utilizada anteriormente. Elija una distinta.";
            }

            return "";
        }

        public bool cumplePoliticasContra(string contra, string usuario)
        {
            return validarContra(contra, usuario) == "";
        }

        public bool registrarContraTemporal(int idUsuario, string contraTemporal)
        {
            string usuario = usuarioDatos.BuscarUsuarioPorId(idUsuario);

            if (string.IsNullOrEmpty(usuario))
                return false;
            string hash = encriptar.hashUsuarioContra(usuario, contraTemporal);

            return contraDatos.insertarPassword(
                idUsuario,
                hash
            );
        }

        public bool cambiarContra(string usuario, string nuevaContra)
        {
            usuario = usuarioDatos.ObtenerNombreUsuarioExacto(usuario);

            if (string.IsNullOrEmpty(usuario))
                return false;

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
