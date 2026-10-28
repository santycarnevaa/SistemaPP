using System.Windows.Forms;

namespace CapaVista
{
    // Para avisos que no son un error del sistema pero requieren atención del usuario
    // (por ejemplo, que corresponde rotar una credencial).
    public class MensajeAdvertencia : MensajeSistema
    {
        protected override string Titulo => "Atención";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Warning;

        public MensajeAdvertencia(string texto) : base(texto) { }
    }
}
