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
        string caracteresEspeciales = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?";
        public bool cumplePoliticasContra(string contra, string usuario)
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

            if (string.IsNullOrWhiteSpace(contra))
                return false;

            if (contra.Length < config.minCaracteres)
                return false;

            if (config.requiereMayusculas && !contra.Any(char.IsUpper))
                return false;

            if (config.requiereMinusculas && !contra.Any(char.IsLower))
                return false;

            if (config.requiereNumeros && !contra.Any(char.IsDigit))
                return false;

            if (config.requiereEspeciales && !contra.Any(c => caracteresEspeciales.Contains(c)))
                return false;

            if (config.validarDatosPersonales)
            {
                if (contra.ToLower().Contains(usuario.ToLower()))
                    return false;
            }

            return true;
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
