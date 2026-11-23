using System.Windows.Forms;

namespace CapaVista
{
    // para avisos que no son un error pero el usuario tiene que ver
    // (por ej que hay que rotar una credencial)
    public class MensajeAdvertencia : MensajeSistema
    {
        protected override string Titulo => "Atención";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Warning;

        public MensajeAdvertencia(string texto) : base(texto) { }
    }
}
