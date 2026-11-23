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
            EstiloSistema.Aplicar(this);
            AcceptButton = btnIniciar;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblRecuperar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ErrorCampo.Limpiar(this);

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                ErrorCampo.Mostrar(txtUsuario, "Ingresá tu usuario para recuperar la cuenta.");
                return;
            }

            string usuario = txtUsuario.Text.Trim();

            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
            CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();

            bool existeUsuario = servicioUsuarios.ExisteUsuario(usuario);

            if (!existeUsuario)
            {
                // no decimos que no existe: igual que en el login, no confirmamos
                // ni negamos si un usuario esta registrado
                ErrorCampo.Mostrar(txtUsuario, "No se pudo iniciar la recuperación. Verificá el usuario.");
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
            ErrorCampo.Limpiar(this);

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
                    // usuario o contraseña vacios, usuario que no existe, inactivo o contraseña
                    // mal: el mensaje lo decide cl_serviciologin, que junta
                    // los ultimos tres casos para no mostrar que usuarios existen
                    Control campo = resultado == resultadoLogin.UsuarioVacio ? txtUsuario : txtContraseña;
                    ErrorCampo.Mostrar(campo, servicioLogin.ObtenerMensajeResultado(resultado));
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
            Navegacion.AbrirPantallaPrincipal(this, usuario);
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
