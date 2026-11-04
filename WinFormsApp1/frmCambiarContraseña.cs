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
            EstiloSistema.Aplicar(this);
            Text = "Cambiar contraseña";
            this.usuario = usuario;
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            ErrorCampo.Limpiar(this);

            string nuevaContra = txtNuevaContra.Text;
            string confirmarContra = txtConfirmarContra.Text;

            if (string.IsNullOrWhiteSpace(nuevaContra))
            {
                ErrorCampo.Mostrar(txtNuevaContra, "Ingresá una nueva contraseña.");
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmarContra))
            {
                ErrorCampo.Mostrar(txtConfirmarContra, "Confirmá la nueva contraseña.");
                return;
            }

            if (nuevaContra != confirmarContra)
            {
                ErrorCampo.Mostrar(txtConfirmarContra, "Las contraseñas no coinciden.");
                return;
            }

            try
            {
                List<string> requisitosIncumplidos = servicioContras.RequisitosIncumplidos(nuevaContra, usuario);

                if (requisitosIncumplidos.Count > 0)
                {
                    ErrorCampo.Mostrar(txtNuevaContra, "No cumple: " + string.Join(", ", requisitosIncumplidos));
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
                    ErrorCampo.Mostrar(txtConfirmarContra, "No se pudo cambiar. Puede que ya se haya usado antes.");
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
            Navegacion.AbrirPantallaPrincipal(this, usuario);
        }

        private void frmCambiarContraseña_Load(object sender, EventArgs e)
        {
            lblRequisitos.Text = servicioContras.DescribirRequisitos();
        }
    }
}
