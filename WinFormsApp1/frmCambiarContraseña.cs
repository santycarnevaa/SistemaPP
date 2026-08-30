using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmCambiarContraseña : Form
    {
        private CL_servicioContras servicioContras = new CL_servicioContras();
        private string usuario;

        public frmCambiarContraseña(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            string nuevaContra = txtNuevaContra.Text;
            string confirmarContra = txtConfirmarContra.Text;

            if (string.IsNullOrWhiteSpace(nuevaContra))
            {
                new MensajeError("Ingrese una nueva contraseña.").Mostrar();
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmarContra))
            {
                new MensajeError("Confirme la nueva contraseña.").Mostrar();
                return;
            }

            if (nuevaContra != confirmarContra)
            {
                new MensajeError("Las contraseñas no coinciden.").Mostrar();
                return;
            }

            try
            {
                List<string> requisitosIncumplidos = servicioContras.RequisitosIncumplidos(nuevaContra, usuario);

                if (requisitosIncumplidos.Count > 0)
                {
                    new MensajeError("La contraseña no cumple los requisitos:\n\n- " + string.Join("\n- ", requisitosIncumplidos)).Mostrar();
                    return;
                }

                bool cambioOk = servicioContras.cambiarContra(usuario, nuevaContra);

                if (cambioOk)
                {
                    new MensajeExito("Contraseña cambiada correctamente.").Mostrar();

                    AbrirPantallaPrincipal(usuario);
                    this.Hide();
                    this.Close();
                }
                else
                {
                    new MensajeError("No se pudo cambiar la contraseña. Puede que ya se haya usado antes.").Mostrar();
                }
            }
            catch (Exception ex)
            {
                new MensajeError("Error real al cambiar contraseña: " + ex.Message).Mostrar();
            }
        }

        private void boton() //habilita el boton cuando los dos txt son iguales
        {
            {
                if (txtNuevaContra.Text == txtConfirmarContra.Text)
                {
                    btnSiguiente.Enabled = true;
                }
                else
                {
                    btnSiguiente.Enabled = false;
                }
            }
        }

        private void txtnueva2_TextChanged(object sender, EventArgs e)
        {
            boton();
        }

        private void txtnueva_TextChanged(object sender, EventArgs e)
        {
            boton();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void pctOjoNueva_Click(object sender, EventArgs e)
        {
            if (txtNuevaContra.PasswordChar == '•')
                txtNuevaContra.PasswordChar = '\0';
            else
                txtNuevaContra.PasswordChar = '•';
        }

        private void pctOjoConfirmar_Click(object sender, EventArgs e)
        {
            if (txtConfirmarContra.PasswordChar == '•')
                txtConfirmarContra.PasswordChar = '\0';
            else
                txtConfirmarContra.PasswordChar = '•';
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

        private void frmCambiarContraseña_Load(object sender, EventArgs e)
        {
            lblRequisitos.Text = servicioContras.DescribirRequisitos();
        }
    }
}
