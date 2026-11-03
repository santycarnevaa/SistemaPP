using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    // Abre el panel que le corresponde a cada usuario según su rol.
    // Antes este código estaba repetido en el login, el código 2FA, el cambio de contraseña y las preguntas.
    public static class Navegacion
    {
        public static void AbrirPantallaPrincipal(Form actual, string usuario)
        {
            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

            int idRol = servicioUsuarios.ObtenerRolUsuario(usuario);

            if (idRol == 2)
            {
                frmAdmin frm = new frmAdmin(usuario);
                actual.Hide();
                frm.ShowDialog();
                actual.Close();
            }
            else if (idRol > 0)
            {
                // cualquier otro rol (Usuario, ventas, etc.) entra al panel de usuario,
                // y frmUsuario decide qué botones muestra según el rol
                frmUsuario frm = new frmUsuario(usuario);
                actual.Hide();
                frm.ShowDialog();
                actual.Close();
            }
            else
            {
                new MensajeError("No se pudo identificar el rol del usuario.").Mostrar();
            }
        }
    }
}
