using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using CapaLogica;
namespace CapaVista

{
    public partial class frmlogin : Form
    {
        CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();
        public string usuario;
        public frmlogin()
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
                MessageBox.Show(
                    "Por favor, ingrese su nombre de usuario para recuperar su cuenta.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            string usuario = txtUsuario.Text.Trim();

            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
            CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();

            bool existeUsuario = servicioUsuarios.ExisteUsuario(usuario);

            if (!existeUsuario)
            {
                MessageBox.Show(
                    "El usuario ingresado no existe.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (servicioConfiguracion.requiere2FA())
            {
                frmCodigo frmCod = new frmCodigo(usuario);
                this.Hide();
                frmCod.ShowDialog();
                this.Show();
            }
            else
            {
                frmrecuperar frmPreg = new frmrecuperar(usuario);
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
                case resultadoLogin.UsuarioVacio:
                    MessageBox.Show("El campo de usuario no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case resultadoLogin.PasswordVacia:
                    MessageBox.Show("El campo de contraseña no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case resultadoLogin.UsuarioNoExiste:
                    MessageBox.Show("El usuario no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case resultadoLogin.UsuarioInactivo:
                    MessageBox.Show("El usuario está inactivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case resultadoLogin.PasswordIncorrecta:
                    MessageBox.Show("La contraseña es incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case resultadoLogin.PrimerLogin:
                    frmPregUsuario frmPreg = new frmPregUsuario(txtUsuario.Text.Trim());
                    this.Hide();
                    frmPreg.ShowDialog();
                    this.Show();
                    break;
                case resultadoLogin.Ok:
                    AbrirPantallaPrincipal(txtUsuario.Text.Trim());
                    this.Hide();
                    break;
            }
            //if (usuario != txtUsuario.Text || contraseña != txtContraseña.Text)
            // {
            //     MessageBox.Show("Algun campo esta incorrecto", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //     return;
            // }
            //if (ingreso == true)
            // {
            //     frmPregUsuario frm = new frmPregUsuario();
            //     this.Hide();
            //     frm.ShowDialog();
            //     this.Show();
            // }
            //else
            // {
            //     frmUsuario frm = new frmUsuario();
            //     this.Hide();
            //     frm.ShowDialog();
            //     this.Show();
            // }
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
                MessageBox.Show("No se pudo identificar el rol del usuario.");
            }
        }

        private void frmlogin_Load(object sender, EventArgs e)
        {

        }
    }
}
