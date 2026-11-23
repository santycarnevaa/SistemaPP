using System.Windows.Forms;

namespace CapaVista
{
    // clase base de todos los mensajes que se le muestran al usuario. cada tipo
    // (error, exito, advertencia, informacion, confirmacion) hereda de aca y
    // pone su titulo y su icono, todos se muestran de la misma forma
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
