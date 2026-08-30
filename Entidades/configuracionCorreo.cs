namespace Entidades
{
    public class ConfiguracionCorreo
    {
        public int IdConfiguracionCorreo { get; set; }
        public string CorreoRemitente { get; set; }
        public byte[] PasswordCifrada { get; set; }
        public byte[] IV { get; set; }
        public DateTime FechaUltimaRotacion { get; set; }
        public int DiasRotacion { get; set; }
    }
}
