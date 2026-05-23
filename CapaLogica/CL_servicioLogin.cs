using System;
using System.Linq;
using Utilidades;

namespace CapaLogica;

    public class CL_servicioLogin
    {
        private LeerDatos datos = new LeerDatos();
        private Encriptar encriptar = new Encriptar();

        public bool Login(string usuario, string contraseña)
        {
            bool existeUsuario = datos.validarUsuarios(usuario);
            if (!existeUsuario)
                return false;
            string contraHasheada = encriptar.hashUsuarioContra(usuario, contraseña);
            bool existeContra = datos.validarContra(contraHasheada, usuario);

            return existeContra;
        }
        public bool PrimerLogin(string usuario)
        {
            bool primerLogin = datos.primerlogin(true, usuario);
            return primerLogin;
        }
        public bool PasswordNoRepetida(int idUser, string nuevoHash)
        {
            LeerDatos datos = new LeerDatos();

            var passwordsViejas = datos.ObtenerPasswordsAnteriores(idUser);

            return !passwordsViejas.Contains(nuevoHash);
        }
        public bool CambiarPassword(string usuario, string nuevaPassword)
        {
            CD_escribirDatos escribir = new CD_escribirDatos();
            LeerDatos datos = new LeerDatos();
            Encriptar enc = new Encriptar();

            int idUser = datos.ObtenerIdUsuario(usuario);

            if (idUser == -1)
                return false;

            string hash = enc.hashUsuarioContra(usuario, nuevaPassword);

            bool noRepetida = PasswordNoRepetida(idUser, hash);
            if (!noRepetida)
                return false;

            return escribir.ActualizarPassword(idUser, hash);
        }
}
