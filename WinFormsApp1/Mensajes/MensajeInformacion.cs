using System.Windows.Forms;

namespace CapaVista
{
    // Para avisos neutros, sin connotación de éxito ni de error (por ejemplo,
    // "ingresá tu usuario para recuperar la cuenta").
    public class MensajeInformacion : MensajeSistema
    {
        protected override string Titulo => "Información";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Information;

        public MensajeInformacion(string texto) : base(texto) { }
    }
}
