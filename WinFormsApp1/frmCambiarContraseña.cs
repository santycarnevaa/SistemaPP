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
    public partial class frmContraseña : Form
    {
        private CL_servicioContras servicioContras = new CL_servicioContras();
        private string usuario;
        public frmContraseña(string usuario)
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
                MessageBox.Show("Ingrese una nueva contraseña.");
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmarContra))
            {
                MessageBox.Show("Confirme la nueva contraseña.");
                return;
            }

            if (nuevaContra != confirmarContra)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return;
            }

            try
            {
                string error = servicioContras.validarContra(nuevaContra, usuario);

                if (error != "")
                {
                    MessageBox.Show(error, "Contraseña no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool cambioOk = servicioContras.cambiarContra(usuario, nuevaContra);

                if (cambioOk)
                {
                    MessageBox.Show("Contraseña cambiada correctamente.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo cambiar la contraseña. Intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar la contraseña: " + ex.Message);
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
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmContraseña_Load(object sender, EventArgs e)
        {

        }
    }
}
