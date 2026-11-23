using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    // abre el panel que le toca a cada usuario segun su rol
    // antes esto estaba repetido en el login, el codigo 2fa, el cambio de contraseña y las preguntas
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
                // cualquier otro rol (usuario, ventas, etc) entra al panel de usuario
                // y frmusuario decide que botones muestra segun el rol
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
