using System.Windows.Forms;

namespace CapaVista
{
    // para avisar que algo (guardar, registrar, actualizar) salio bien
    public class MensajeExito : MensajeSistema
    {
        protected override string Titulo => "Éxito";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Information;

        public MensajeExito(string texto) : base(texto) { }
    }
}
