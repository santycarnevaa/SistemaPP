using System;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CapaVista
{
    internal static class Program
    {
        // punto de entrada de la aplicacion
        [STAThread]
        static void Main()
        {
            // configuracion de la app (dpi, fuente por defecto, etc),
            // ver https://aka.ms/applicationconfiguration
            ApplicationConfiguration.Initialize();
            Application.Run(new frmLogin());
        }
    }
}
