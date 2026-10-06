using System;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CapaVista
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // MODO PRUEBA: abre directo el registro para crear usuarios.
            // Para la presentacion, comentar la linea de frmRegistro y descomentar la de frmlogin.
            Application.Run(new frmRegistro());
            //Application.Run(new frmlogin());
        }
    }
}