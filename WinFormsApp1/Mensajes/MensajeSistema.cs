using System.Windows.Forms;

namespace CapaVista
{
    // Clase base de todos los mensajes que se le muestran al usuario. Cada tipo de
    // mensaje (error, éxito, advertencia, información, confirmación) hereda de acá y
    // define su propio título e ícono; todos comparten la misma forma de mostrarse.
    public abstract class MensajeSistema
    {
        protected abstract string Titulo { get; }
        protected abstract MessageBoxIcon Icono { get; }

        public string Texto { get; }

        protected MensajeSistema(string texto)
        {
            Texto = texto;
        }

        public virtual DialogResult Mostrar()
        {
            return MessageBox.Show(Texto, Titulo, MessageBoxButtons.OK, Icono);
        }
    }
}
