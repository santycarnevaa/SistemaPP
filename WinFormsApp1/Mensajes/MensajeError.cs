using System.Windows.Forms;

namespace CapaVista
{
    // Para errores y validaciones que impiden continuar (campo vacío, dato inválido,
    // excepción real, etc.).
    public class MensajeError : MensajeSistema
    {
        protected override string Titulo => "Error";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Warning;

        public MensajeError(string texto) : base(texto) { }
    }
}
