namespace Entidades
{
    public class UsuarioGrilla
    {
        public int IdUsuario { get; set; }
        public int IdPersona { get; set; }
        public int IdDireccion { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }

        public string Usuario { get; set; }
        public string Correo { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }

        public bool Activo { get; set; }
        public string Estado { get; set; }

        public string Calle { get; set; }
        public string Numero { get; set; }
        public string CodigoPostal { get; set; }
        public string Depto { get; set; }
        public string Piso { get; set; }
        public string Provincia { get; set; }
        public string Partido { get; set; }
        public string Localidad { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}
