using CapaDatos;
using Utilidades;

namespace CapaLogica
{
    public class CL_ServicioLogin
    {
        private CD_usuariosDatos usuarioDatos = new CD_usuariosDatos();
        private CD_contraDatos passwordDatos = new CD_contraDatos();
        private encriptar encriptar = new encriptar();

        public resultadoLogin Login(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return resultadoLogin.UsuarioVacio;

            if (string.IsNullOrWhiteSpace(password))
                return resultadoLogin.PasswordVacia;

            bool existeUsuario = usuarioDatos.ExisteUsuario(usuario);

            if (!existeUsuario)
                return resultadoLogin.UsuarioNoExiste;

            bool usuarioActivo = usuarioDatos.UsuarioActivo(usuario);

            if (!usuarioActivo)
                return resultadoLogin.UsuarioInactivo;

            string passwordHash = encriptar.hashUsuarioContra(usuario, password);

            bool loginCorrecto = passwordDatos.verificarLogin(usuario, passwordHash);

            if (!loginCorrecto)
                return resultadoLogin.PasswordIncorrecta;

            bool primerLogin = usuarioDatos.EsPrimerLogin(usuario);

            if (primerLogin)
                return resultadoLogin.PrimerLogin;

            return resultadoLogin.Ok;
        }

        public bool LoginCorrecto(string usuario, string password)
        {
            resultadoLogin resultado = Login(usuario, password);

            return resultado == resultadoLogin.Ok || resultado == resultadoLogin.PrimerLogin;
        }

        public bool EsPrimerLogin(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            return usuarioDatos.EsPrimerLogin(usuario);
        }

        public bool UsuarioActivo(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            return usuarioDatos.UsuarioActivo(usuario);
        }

        public bool ExisteUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            return usuarioDatos.ExisteUsuario(usuario);
        }

        public int ObtenerIdUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return -1;

            return usuarioDatos.BuscarUsuarioPorNombreUser(usuario);
        }

        public int ObtenerRolUsuario(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return -1;

            return usuarioDatos.ObtenerRolUsuario(usuario);
        }

        public bool EsAdministrador(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return false;

            int idRol = usuarioDatos.ObtenerRolUsuario(usuario);

            return idRol == 2;
        }

        public string ObtenerMensajeResultado(resultadoLogin resultado)
        {
            switch (resultado)
            {
                case resultadoLogin.Ok:
                    return "Bienvenido al sistema.";

                case resultadoLogin.PrimerLogin:
                    return "Es tu primer login. Debés cambiar tu contraseña.";

                case resultadoLogin.UsuarioVacio:
                    return "Ingrese el nombre de usuario.";

                case resultadoLogin.PasswordVacia:
                    return "Ingrese la contraseña.";

                case resultadoLogin.UsuarioNoExiste:
                    return "El usuario no existe.";

                case resultadoLogin.UsuarioInactivo:
                    return "El usuario está inactivo. No puede iniciar sesión.";

                case resultadoLogin.PasswordIncorrecta:
                    return "La contraseña es incorrecta.";

                default:
                    return "Ocurrió un error al iniciar sesión.";
            }
        }
    }
}
