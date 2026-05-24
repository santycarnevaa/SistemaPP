namespace Entidades
{
    public class configuracionSistema
    {
        public int idConfiguracion { get; set; }
        public int minCaracteres { get; set; }
        public int cantPreguntas { get; set; }

        public bool requiereMayusculas { get; set; }
        public bool requiereMinusculas { get; set; }
        public bool requiereNumeros { get; set; }
        public bool requiereEspeciales { get; set; }

        public bool requiere2FA { get; set; }
        public bool noRepetirPasswords { get; set; }
        public bool validarDatosPersonales { get; set; }
    }
}