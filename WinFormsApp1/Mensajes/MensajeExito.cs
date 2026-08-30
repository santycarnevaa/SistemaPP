using System.Windows.Forms;

namespace CapaVista
{
    // Para confirmar que una operación (guardar, registrar, actualizar) salió bien.
    public class MensajeExito : MensajeSistema
    {
        protected override string Titulo => "Éxito";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Information;

        public MensajeExito(string texto) : base(texto) { }
    }
}
