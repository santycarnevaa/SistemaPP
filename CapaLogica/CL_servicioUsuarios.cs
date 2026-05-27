using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace CapaLogica
{
    public class CL_servicioUsuarios
    {
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private CL_servicioContras servicioContras = new CL_servicioContras();
        private CL_servicioMail servicioMail = new CL_servicioMail();
        private generarContra generarContra = new generarContra();
        private encriptar encriptar = new encriptar();
        public resultadoRegistroUsuario RegistrarUsuario(
        string calle,
        string numero,
        string codigoPostal,
        string depto,
        string piso,
        string provincia,
        string partido,
        string localidad,

        string nombre,
        string apellido,
        string dni,
        string telefono,
        DateTime fechaNacimiento,

        string usuario,
        string correo,
        int idRol)
            {
                if (string.IsNullOrWhiteSpace(usuario))
                    return resultadoRegistroUsuario.UsuarioVacio;

                if (string.IsNullOrWhiteSpace(nombre))
                    return resultadoRegistroUsuario.NombreVacio;

                if (string.IsNullOrWhiteSpace(apellido))
                    return resultadoRegistroUsuario.ApellidoVacio;

                if (string.IsNullOrWhiteSpace(dni))
                    return resultadoRegistroUsuario.DniVacio;

                if (string.IsNullOrWhiteSpace(telefono))
                    return resultadoRegistroUsuario.TelefonoVacio;

                if (string.IsNullOrWhiteSpace(correo))
                    return resultadoRegistroUsuario.CorreoVacio;

                if (string.IsNullOrWhiteSpace(calle))
                    return resultadoRegistroUsuario.CalleVacia;

                if (string.IsNullOrWhiteSpace(numero))
                    return resultadoRegistroUsuario.NumeroVacio;

                if (string.IsNullOrWhiteSpace(codigoPostal))
                    return resultadoRegistroUsuario.CodigoPostalVacio;

                if (string.IsNullOrWhiteSpace(provincia))
                    return resultadoRegistroUsuario.ProvinciaVacia;

                if (string.IsNullOrWhiteSpace(partido))
                    return resultadoRegistroUsuario.PartidoVacio;

                if (string.IsNullOrWhiteSpace(localidad))
                    return resultadoRegistroUsuario.LocalidadVacia;

                if (idRol <= 0)
                    return resultadoRegistroUsuario.RolInvalido;

                if (usuarioDatos.ExisteUsuario(usuario))
                    return resultadoRegistroUsuario.UsuarioYaExiste;

                if (usuarioDatos.ExisteCorreo(correo))
                    return resultadoRegistroUsuario.CorreoYaExiste;

                string direccionOriginal = calle + " " + numero + ", " + localidad + ", " + partido + ", " + provincia;
                string direccionNormalizada = direccionOriginal.ToUpper().Trim();

                string passwordTemporal = generarContra.autogenerarContra();
                string passwordHash = encriptar.hashUsuarioContra(usuario, passwordTemporal);

                int idUsuario = usuarioDatos.RegistrarUsuarioCompleto(
                    nombre,
                    apellido,
                    dni,
                    telefono,
                    fechaNacimiento,

                    calle,
                    numero,
                    codigoPostal,
                    depto,
                    piso,
                    provincia,
                    partido,
                    localidad,
                    null,
                    null,
                    direccionOriginal,
                    direccionNormalizada,

                    usuario,
                    correo,
                    passwordHash,
                    idRol,
                    false
                );

            if (idUsuario == -1)
                    return resultadoRegistroUsuario.ErrorBaseDatos;

                //bool passwordOk = servicioContras.registrarContraTemporal(idUsuario, passwordTemporal);

                //if (!passwordOk)
                //    return resultadoRegistroUsuario.ErrorPasswordTemporal;

                bool mailOk = servicioMail.enviarContraTemporal(correo, passwordTemporal);

                if (!mailOk)
                    return resultadoRegistroUsuario.ErrorEnvioMail;

                return resultadoRegistroUsuario.Ok;

        }
        public bool DarDeBajaUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                return false;

            return usuarioDatos.ActualizarEstadoUsuario(idUsuario, false);
        }

        public bool ActivarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                return false;

            return usuarioDatos.ActualizarEstadoUsuario(idUsuario, true);
        }

        public bool ActualizarDatosUsuario(
    int idUsuario,

    string nombre,
    string apellido,
    string dni,
    string telefono,
    DateTime fechaNacimiento,

    string calle,
    string numero,
    string codigoPostal,
    string depto,
    string piso,
    string provincia,
    string partido,
    string localidad,

    string correo,
    int idRol)
        {
            if (idUsuario <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            if (string.IsNullOrWhiteSpace(apellido))
                return false;

            if (string.IsNullOrWhiteSpace(dni))
                return false;

            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            if (string.IsNullOrWhiteSpace(calle))
                return false;

            if (string.IsNullOrWhiteSpace(numero))
                return false;

            if (string.IsNullOrWhiteSpace(codigoPostal))
                return false;

            if (string.IsNullOrWhiteSpace(provincia))
                return false;

            if (string.IsNullOrWhiteSpace(partido))
                return false;

            if (string.IsNullOrWhiteSpace(localidad))
                return false;

            if (string.IsNullOrWhiteSpace(correo))
                return false;

            if (idRol <= 0)
                return false;

            string direccionOriginal = calle + " " + numero + ", " + localidad + ", " + partido + ", " + provincia;
            string direccionNormalizada = direccionOriginal.ToUpper().Trim();

            return usuarioDatos.ActualizarDatosUsuario(
                idUsuario,

                nombre,
                apellido,
                dni,
                telefono,
                fechaNacimiento,

                calle,
                numero,
                codigoPostal,
                depto,
                piso,
                provincia,
                partido,
                localidad,
                null,
                null,
                direccionOriginal,
                direccionNormalizada,

                correo,
                idRol
            );
        }

        public int ObtenerIdUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return -1;

            return usuarioDatos.BuscarUsuarioPorNombreUser(usuario);
        }

        public bool ExisteUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            return usuarioDatos.ExisteUsuario(usuario);
        }

        public bool ExisteCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            return usuarioDatos.ExisteCorreo(correo);
        }
        public string ObtenerMensajeRegistro(resultadoRegistroUsuario resultado)
        {
            switch (resultado)
            {
                case resultadoRegistroUsuario.Ok:
                    return "Usuario registrado correctamente.";

                case resultadoRegistroUsuario.UsuarioVacio:
                    return "Debe ingresar un nombre de usuario.";

                case resultadoRegistroUsuario.NombreVacio:
                    return "Debe ingresar el nombre.";

                case resultadoRegistroUsuario.ApellidoVacio:
                    return "Debe ingresar el apellido.";

                case resultadoRegistroUsuario.DniVacio:
                    return "Debe ingresar el DNI.";

                case resultadoRegistroUsuario.TelefonoVacio:
                    return "Debe ingresar el teléfono.";

                case resultadoRegistroUsuario.CorreoVacio:
                    return "Debe ingresar el correo.";

                case resultadoRegistroUsuario.CalleVacia:
                    return "Debe ingresar la calle.";

                case resultadoRegistroUsuario.NumeroVacio:
                    return "Debe ingresar el número de calle.";

                case resultadoRegistroUsuario.CodigoPostalVacio:
                    return "Debe ingresar el código postal.";

                case resultadoRegistroUsuario.ProvinciaVacia:
                    return "Debe seleccionar una provincia.";

                case resultadoRegistroUsuario.PartidoVacio:
                    return "Debe seleccionar un partido.";

                case resultadoRegistroUsuario.LocalidadVacia:
                    return "Debe seleccionar una localidad.";

                case resultadoRegistroUsuario.RolInvalido:
                    return "Debe seleccionar un rol válido.";

                case resultadoRegistroUsuario.UsuarioYaExiste:
                    return "El nombre de usuario ya existe.";

                case resultadoRegistroUsuario.CorreoYaExiste:
                    return "El correo ya está registrado.";

                case resultadoRegistroUsuario.ErrorBaseDatos:
                    return "No se pudo registrar el usuario en la base de datos.";

                case resultadoRegistroUsuario.ErrorPasswordTemporal:
                    return "El usuario se creó, pero no se pudo registrar la contraseña temporal.";

                case resultadoRegistroUsuario.ErrorEnvioMail:
                    return "El usuario se creó, pero no se pudo enviar el correo con la contraseña temporal.";

                default:
                    return "Ocurrió un error desconocido.";
            }

        }
        public bool esPrimerLogin(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            return usuarioDatos.EsPrimerLogin(usuario);
        }
        public string ObtenerCorreoPorUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return "";

            return usuarioDatos.ObtenerCorreoPorUsuario(usuario);
        }
        public List<UsuarioGrilla> ObtenerUsuariosParaGrilla()
        {
            return usuarioDatos.ObtenerUsuariosGrilla();
        }
        public int ObtenerRolUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return 0;

            return usuarioDatos.ObtenerRolUsuario(usuario);
        }
    }
}
