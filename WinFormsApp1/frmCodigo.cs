using System;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmCodigo : Form
    {
        private string usuario;
        private string codigoGenerado;
        private OrigenVerificacion origen;

        private CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
        private CL_servicioMail servicioMail = new CL_servicioMail();

        public frmCodigo(string usuario, OrigenVerificacion origen = OrigenVerificacion.RecuperarContrasena)
        {
            InitializeComponent();

            this.usuario = usuario;
            this.origen = origen;

            txt2.Enabled = false;
            txt3.Enabled = false;
            txt4.Enabled = false;
            txt5.Enabled = false;
            btnSiguiente.Enabled = false;

            this.Load += frmCodigo_Load;
        }

        private void frmCodigo_Load(object sender, EventArgs e)
        {
            GenerarYEnviarCodigo();
        }

        private void GenerarYEnviarCodigo()
        {
            try
            {
                string correo = servicioUsuarios.ObtenerCorreoPorUsuario(usuario);

                if (string.IsNullOrWhiteSpace(correo))
                {
                    new MensajeError("No se pudo obtener el correo del usuario.").Mostrar();
                    this.Close();
                    return;
                }

                Random random = new Random();
                codigoGenerado = random.Next(10000, 99999).ToString();

                bool enviado = servicioMail.EnviarCodigoRecuperacion(correo, codigoGenerado);

                if (enviado)
                {
                    new MensajeExito("Se envió un código de verificación a su correo.").Mostrar();
                }
                else
                {
                    new MensajeError("No se pudo enviar el código de verificación.").Mostrar();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                new MensajeError("Error al generar o enviar el código: " + ex.Message).Mostrar();
                this.Close();
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            string codigoIngresado = txt1.Text + txt2.Text + txt3.Text + txt4.Text + txt5.Text;

            if (codigoIngresado != codigoGenerado)
            {
                new MensajeError("Código equivocado").Mostrar();

                txt1.Text = "";
                txt2.Text = "";
                txt3.Text = "";
                txt4.Text = "";
                txt5.Text = "";

                txt2.Enabled = false;
                txt3.Enabled = false;
                txt4.Enabled = false;
                txt5.Enabled = false;

                btnSiguiente.Enabled = false;

                txt1.Focus();
                return;
            }

            if (origen == OrigenVerificacion.ConfirmacionLogin)
            {
                // Login estándar con 2FA (T09): el código solo confirma la
                // identidad, no hace falta pasar por las preguntas de seguridad.
                AbrirPantallaPrincipal(usuario);
                return;
            }

            frmPreguntas frm = new frmPreguntas(usuario, origen);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void AbrirPantallaPrincipal(string usuario)
        {
            int idRol = servicioUsuarios.ObtenerRolUsuario(usuario);

            if (idRol == 2)
            {
                frmAdmin frm = new frmAdmin(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else if (idRol == 1)
            {
                frmUsuario frm = new frmUsuario(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                new MensajeError("No se pudo identificar el rol del usuario.").Mostrar();
            }
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            Boton();

            if (txt1.Text == "")
            {
                txt1.Focus();
            }
            else
            {
                txt2.Enabled = true;
                txt2.Focus();
            }
        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {
            Boton();

            if (txt2.Text == "")
            {
                txt2.Focus();
            }
            else
            {
                txt3.Enabled = true;
                txt3.Focus();
            }
        }

        private void txt3_TextChanged(object sender, EventArgs e)
        {
            Boton();

            if (txt3.Text == "")
            {
                txt3.Focus();
            }
            else
            {
                txt4.Enabled = true;
                txt4.Focus();
            }
        }

        private void txt4_TextChanged(object sender, EventArgs e)
        {
            Boton();

            if (txt4.Text == "")
            {
                txt4.Focus();
            }
            else
            {
                txt5.Enabled = true;
                txt5.Focus();
            }
        }

        private void txt5_TextChanged(object sender, EventArgs e)
        {
            Boton();

            if (txt5.Text == "")
            {
                txt5.Focus();
            }
        }

        private void txt2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
                txt1.Focus();
        }

        private void txt3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
                txt2.Focus();
        }

        private void txt4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
                txt3.Focus();
        }

        private void txt5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
                txt4.Focus();
        }

        private void Boton()
        {
            if (txt1.Text == "" || txt2.Text == "" || txt3.Text == "" || txt4.Text == "" || txt5.Text == "")
                btnSiguiente.Enabled = false;
            else
                btnSiguiente.Enabled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void frmCodigo_Load_1(object sender, EventArgs e)
        {

        }
    }
}
