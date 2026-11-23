using System.Windows.Forms;

namespace CapaVista
{
    // para avisos normales, que no son ni de exito ni de error (por ej
    // "ingresa tu usuario para recuperar la cuenta")
    public class MensajeInformacion : MensajeSistema
    {
        protected override string Titulo => "Información";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Information;

        public MensajeInformacion(string texto) : base(texto) { }
    }
}
