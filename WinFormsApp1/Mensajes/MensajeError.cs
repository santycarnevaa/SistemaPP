using System.Windows.Forms;

namespace CapaVista
{
    // para errores y validaciones que no dejan seguir (campo vacio, dato invalido,
    // una excepcion, etc)
    public class MensajeError : MensajeSistema
    {
        protected override string Titulo => "Error";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Warning;

        public MensajeError(string texto) : base(texto) { }
    }
}
