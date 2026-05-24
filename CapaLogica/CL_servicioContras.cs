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
    internal class CL_servicioContras
    {
        private CD_contraDatos contraDatos = new CD_contraDatos();
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private CD_configuracionDatos configDatos = new CD_configuracionDatos();
        private encriptar encriptar = new encriptar();
        public bool cumplePoliticasContra(string contra, string usuario)
        {
            configuracionSistema config = configDatos.obtenerConfiguracion();

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

            if (config.requiereEspeciales && !contra.Any(c => !char.IsLetterOrDigit(c)))
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
            string usuario = usuarioDatos.obtenerUsuarioPorId(idUsuario);

            if (string.IsNullOrEmpty(usuario))
                return false;
            string hash = encriptar.hashUsuarioContra(usuario, contraTemporal);

            return contraDatos.insertarPassword(
                idUsuario,
                hash,
                esTemporal: true
            );
        }
        public bool cambiarContra(string usuario, string nuevaContra)
        {
            int idUsuario = usuarioDatos.obtenerIdUsuario(usuario);

            if (idUsuario == -1)
                return false;

            if (!cumplePoliticasContra(nuevaContra, usuario))
                return false;

            string hash = encriptar.hashUsuarioContra(usuario, nuevaContra);

            List<string> passwordsAnteriores = contraDatos.obtenerContrasAnteriores(idUsuario);

            if (passwordsAnteriores.Contains(hash))
                return false;

            bool guardado = contraDatos.insertarPassword(
                idUsuario,
                hash,
                esTemporal: false
            );

            if (!guardado)
                return false;

            return usuarioDatos.actualizarPrimerLogin(idUsuario, false);
        }
    }
}
