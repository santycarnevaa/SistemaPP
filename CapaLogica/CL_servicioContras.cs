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
        private generarContra generarContra = new generarContra();
        private CL_servicioMail servicioMail = new CL_servicioMail();
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

            if (config.validarDatosPersonales && !string.IsNullOrWhiteSpace(usuario))
            {
                string contraMin = contra.ToLower();

                if (contraMin.Contains(usuario.ToLower()))
                    faltantes.Add("No puede contener tu nombre de usuario.");

                var datos = usuarioDatos.ObtenerDatosPersonalesPorUsuario(usuario);

                if (!string.IsNullOrWhiteSpace(datos.nombre) && datos.nombre.Length >= 3 && contraMin.Contains(datos.nombre.ToLower()))
                    faltantes.Add("No puede contener tu nombre.");

                if (!string.IsNullOrWhiteSpace(datos.apellido) && datos.apellido.Length >= 3 && contraMin.Contains(datos.apellido.ToLower()))
                    faltantes.Add("No puede contener tu apellido.");

                if (!string.IsNullOrWhiteSpace(datos.dni) && contra.Contains(datos.dni))
                    faltantes.Add("No puede contener tu DNI.");

                if (datos.fechaNacimiento.HasValue && contra.Contains(datos.fechaNacimiento.Value.Year.ToString()))
                    faltantes.Add("No puede contener tu año de nacimiento.");
            }

            if (config.noRepetirPasswords && !string.IsNullOrWhiteSpace(usuario))
            {
                int idUsuario = usuarioDatos.BuscarUsuarioPorNombreUser(usuario);

                if (idUsuario > 0)
                {
                    string hashCandidato = encriptar.hashUsuarioContra(usuario, contra);
                    List<string> anteriores = contraDatos.obtenerContrasAnteriores(idUsuario);

                    if (anteriores.Contains(hashCandidato))
                        faltantes.Add("No podés reutilizar una contraseña que ya usaste antes.");
                }
            }

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
                requisitos.Add("no puede contener tus datos personales (usuario, nombre, apellido, DNI o año de nacimiento)");

            if (config.noRepetirPasswords)
                requisitos.Add("no puede ser una contraseña que ya hayas usado antes");

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

            configuracionSistema config = ObtenerConfiguracionConDefault();
            string hash = encriptar.hashUsuarioContra(usuario, nuevaContra);

            return contraDatos.CambiarContra(idUsuario, hash, config.noRepetirPasswords);
        }

        // Recuperación de contraseña (T10): en vez de dejar que el usuario elija
        // una nueva contraseña directamente, se genera una temporal, se guarda
        // hasheada y se le envía por correo. Tiene que volver a iniciar sesión
        // con ella.
        public bool GenerarYEnviarPasswordTemporal(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            int idUsuario = usuarioDatos.BuscarUsuarioPorNombreUser(usuario);

            if (idUsuario <= 0)
                return false;

            string correo = usuarioDatos.ObtenerCorreoPorUsuario(usuario);

            if (string.IsNullOrWhiteSpace(correo))
                return false;

            string passwordTemporal = generarContra.autogenerarContra();
            string hash = encriptar.hashUsuarioContra(usuario, passwordTemporal);

            bool guardado = contraDatos.insertarPassword(idUsuario, hash);

            if (!guardado)
                return false;

            return servicioMail.enviarContraTemporal(correo, passwordTemporal);
        }
    }
}
