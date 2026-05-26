using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using CapaLogica;
namespace CapaVista

{
    public partial class frmlogin : Form
    {
        public static string user = "";
        public frmlogin()
        {
            InitializeComponent();
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblRecuperar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            user = txtUsuario.Text.Trim();

            if (frmConfigAdmin.requiere2FA == 1)
            {
                frmCodigo frmCod = new frmCodigo();
                this.Hide();
                frmCod.ShowDialog();
                this.Show();
            }
            else
            {
                frmrecuperar frmPreg = new frmrecuperar();
                this.Hide();
                frmPreg.ShowDialog();
                this.Show();
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            user = txtUsuario.Text;
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
                        frmUsuario frm = new frmUsuario();
                        this.Hide();
                        frm.ShowDialog();
                        this.Show();
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

     
    }
}
