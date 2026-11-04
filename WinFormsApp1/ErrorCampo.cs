using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaVista
{
    // Muestra los errores de validacion como texto abajo del campo, en vez de un cartel.
    // El texto se borra solo cuando el usuario vuelve a escribir en ese campo.
    public static class ErrorCampo
    {
        private const string Prefijo = "error_";

        public static void Mostrar(Control campo, string mensaje)
        {
            Control padre = campo.Parent;
            string nombre = Prefijo + campo.Name;

            Label etiqueta = padre.Controls.OfType<Label>().FirstOrDefault(l => l.Name == nombre);

            if (etiqueta == null)
            {
                etiqueta = new Label
                {
                    Name = nombre,
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    ForeColor = Color.FromArgb(255, 107, 107),
                    Font = new Font("Segoe UI", 8F)
                };

                padre.Controls.Add(etiqueta);
                campo.TextChanged += (s, e) => etiqueta.Visible = false;
            }

            etiqueta.MaximumSize = new Size(padre.ClientSize.Width - campo.Left - 8, 0);
            etiqueta.Location = new Point(campo.Left, campo.Bottom + 1);
            etiqueta.Text = mensaje;
            etiqueta.Visible = true;
            etiqueta.BringToFront();
        }

        // Saca los errores anteriores antes de volver a validar
        public static void Limpiar(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (control is Label && control.Name.StartsWith(Prefijo))
                    control.Visible = false;
                else if (control.HasChildren)
                    Limpiar(control);
            }
        }
    }
}
