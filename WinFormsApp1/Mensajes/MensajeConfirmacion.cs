using System.Windows.Forms;

namespace CapaVista
{
    // Para preguntas que el usuario tiene que responder Sí/No antes de seguir
    // (por ejemplo, dar de baja un usuario). A diferencia de los demás mensajes,
    // sobreescribe Mostrar() porque necesita botones distintos (Sí/No en vez de OK).
    public class MensajeConfirmacion : MensajeSistema
    {
        protected override string Titulo => "Confirmación";
        protected override MessageBoxIcon Icono => MessageBoxIcon.Question;

        public MensajeConfirmacion(string texto) : base(texto) { }

        public override DialogResult Mostrar()
        {
            return MessageBox.Show(Texto, Titulo, MessageBoxButtons.YesNo, Icono);
        }

        // Atajo para el caso más común: preguntar "¿confirmás?" y usar el resultado como bool.
        public bool Confirmado()
        {
            return Mostrar() == DialogResult.Yes;
        }
    }
}
