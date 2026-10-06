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

            // permite iniciar sesion apretando Enter
            this.AcceptButton = btnIniciar;
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

            try
            {
                CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
                CL_ServicioLogin servicioLogin = new CL_ServicioLogin();

                if (!servicioUsuarios.ExisteUsuario(usuario))
                {
                    MessageBox.Show("El usuario ingresado no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!servicioLogin.UsuarioActivo(usuario))
                {
                    MessageBox.Show("El usuario está inactivo. Comuníquese con el administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Form frmSiguiente;

                if (servicioConfiguracion.requiere2FA())
                    frmSiguiente = new frmCodigo(usuario);
                else
                    frmSiguiente = new frmrecuperar(usuario);

                this.Hide();
                frmSiguiente.ShowDialog();
                txtContraseña.Clear();
                this.Show();
            }
            catch (Exception ex)
            {
                this.Show();
                MessageBox.Show("No se pudo conectar con la base de datos.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();

            try
            {
                CL_ServicioLogin servicioLogin = new CL_ServicioLogin();
                resultadoLogin resultado = servicioLogin.Login(usuario, txtContraseña.Text);

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
                        txtContraseña.Clear();
                        txtContraseña.Focus();
                        break;
                    case resultadoLogin.PrimerLogin:
                        MessageBox.Show("Es tu primer ingreso. Primero vas a elegir tus preguntas de seguridad y después tu nueva contraseña.", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        frmPregUsuario frmPreg = new frmPregUsuario(usuario);
                        this.Hide();
                        DialogResult r = frmPreg.ShowDialog();
                        txtContraseña.Clear();

                        if (r == DialogResult.OK)
                            AbrirPantallaPrincipal(usuario);
                        else
                            this.Show();
                        break;
                    case resultadoLogin.Ok:
                        AbrirPantallaPrincipal(usuario);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Show();
                MessageBox.Show("No se pudo conectar con la base de datos.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Abre la pantalla segun el rol y, al cerrar sesion, vuelve al login
        private void AbrirPantallaPrincipal(string usuario)
        {
            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

            int idRol = servicioUsuarios.ObtenerRolUsuario(usuario);

            Form frm;

            if (idRol == 2)
                frm = new frmAdmin(usuario);
            else if (idRol == 1)
                frm = new frmUsuario(usuario);
            else
            {
                this.Show();
                MessageBox.Show("No se pudo identificar el rol del usuario.");
                return;
            }

            this.Hide();
            frm.ShowDialog();

            txtContraseña.Clear();
            txtContraseña.PasswordChar = '•';
            this.Show();
            txtUsuario.Focus();
        }

        private void frmlogin_Load(object sender, EventArgs e)
        {

        }
    }
}
