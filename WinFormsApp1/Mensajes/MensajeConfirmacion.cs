using System.Windows.Forms;

namespace CapaVista
{
    // para preguntas de si o no que el usuario tiene que responder antes de seguir
    // (por ej dar de baja un usuario). a diferencia de los otros mensajes
    // sobreescribe mostrar() porque necesita otros botones (si/no en vez de ok)
    public class MensajeConfirmacion : MensajeSistema
    {
        protected override string Titulo => "Confirmación";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Question;

        public MensajeConfirmacion(string texto) : base(texto) { }

        public override DialogResult Mostrar()
        {
            return MessageBox.Show(Texto, Titulo, MessageBoxButtons.YesNo, Icono);
        }

        // atajo para lo mas comun: preguntar si confirma y usar la respuesta como bool
        public bool Confirmado()
        {
            return Mostrar() == DialogResult.Yes;
        }
    }
}
