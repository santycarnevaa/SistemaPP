using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using CapaLogica;
namespace CapaVista

{
    public partial class frmLogin : Form
    {
        CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();
        public string usuario;
        public frmLogin()
        {
            InitializeComponent();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblRecuperar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                new MensajeInformacion("Por favor, ingrese su nombre de usuario para recuperar su cuenta.").Mostrar();
                return;
            }

            string usuario = txtUsuario.Text.Trim();

            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
            CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();

            bool existeUsuario = servicioUsuarios.ExisteUsuario(usuario);

            if (!existeUsuario)
            {
                // No decimos "no existe": igual que en el login, evitamos confirmar
                // o negar si un nombre de usuario está registrado en el sistema.
                new MensajeError("No se pudo iniciar la recuperación de cuenta. Verificá el usuario ingresado.").Mostrar();
                return;
            }

            if (servicioConfiguracion.requiere2FA())
            {
                frmCodigo frmCod = new frmCodigo(usuario, OrigenVerificacion.RecuperarContrasena);
                this.Hide();
                frmCod.ShowDialog();
                this.Show();
            }
            else
            {
                frmPreguntas frmPreg = new frmPreguntas(usuario, OrigenVerificacion.RecuperarContrasena);
                this.Hide();
                frmPreg.ShowDialog();
                this.Show();
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            CL_ServicioLogin servicioLogin = new CL_ServicioLogin();
            resultadoLogin resultado = servicioLogin.Login(txtUsuario.Text, txtContraseña.Text);

            switch (resultado)
            {
                case resultadoLogin.PrimerLogin:
                    frmPregUsuario frmPreg = new frmPregUsuario(txtUsuario.Text.Trim());
                    this.Hide();
                    frmPreg.ShowDialog();
                    this.Show();
                    break;
                case resultadoLogin.Ok:
                    string usuarioOk = txtUsuario.Text.Trim();

                    if (servicioConfiguracion.requiere2FA())
                    {
                        frmCodigo frmCod = new frmCodigo(usuarioOk, OrigenVerificacion.ConfirmacionLogin);
                        this.Hide();
                        frmCod.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        AbrirPantallaPrincipal(usuarioOk);
                        this.Hide();
                    }
                    break;
                default:
                    // Usuario/contraseña vacíos, usuario inexistente, inactivo o contraseña
                    // incorrecta: el mensaje exacto lo decide CL_ServicioLogin, que unifica
                    // los tres últimos casos para no revelar cuáles usuarios existen.
                    new MensajeError(servicioLogin.ObtenerMensajeResultado(resultado)).Mostrar();
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (txtContraseña.PasswordChar == '•')
            {
                txtContraseña.PasswordChar = '\0';
            }
            else if (txtContraseña.PasswordChar == '\0')
            {
                txtContraseña.PasswordChar = '•';
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }
        private void AbrirPantallaPrincipal(string usuario)
        {
            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

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

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
