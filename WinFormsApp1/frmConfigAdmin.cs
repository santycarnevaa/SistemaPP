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
using Entidades;

namespace CapaVista
{
    public partial class frmConfigAdmin : Form
    {
        private string usuario;
        private CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();
        private CL_servicioGeoref servicioGeoref = new CL_servicioGeoref();
        public frmConfigAdmin(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmAdmin frm = new frmAdmin(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCaracteres.Text, out int minCaracteres) || minCaracteres < 1)
            {
                MessageBox.Show("Ingrese un número válido para la cantidad mínima de caracteres.");
                return;
            }
            configuracionSistema config = new configuracionSistema
            {
                minCaracteres = minCaracteres,
                requiereNumeros = ckbNumeros.Checked,
                requiereEspeciales = ckbCaracteresEsp.Checked,
                requiere2FA = ckb2FA.Checked,
                noRepetirPasswords = !ckbRepetirContra.Checked,
                validarDatosPersonales = !ckbDatosPersonales.Checked,
                cantPreguntas = rb1.Checked ? 1 : rb5.Checked ? 5 : 3
            };
            bool resultado = servicioConfiguracion.actualizarConfiguracion(config);
            if (resultado)
            {
                MessageBox.Show("Configuración guardada exitosamente.");
            }
            else
            {
                MessageBox.Show("Error al guardar la configuración. Intente nuevamente.");
            }
        }

        private void frmConfigAdmin_Load(object sender, EventArgs e)
        {
            configuracionSistema config = servicioConfiguracion.ObtenerConfiguracion();

            if (config == null)
            {
                MessageBox.Show("No hay configuración cargada. Se usarán valores por defecto.");

                txtCaracteres.Text = "8";
                ckbNumeros.Checked = true;
                ckbCaracteresEsp.Checked = true;
                ckb2FA.Checked = false;
                ckbRepetirContra.Checked = false;
                ckbDatosPersonales.Checked = false;
                rb3.Checked = true;

                return;
            }

            txtCaracteres.Text = config.minCaracteres.ToString();

            ckbNumeros.Checked = config.requiereNumeros;
            ckbCaracteresEsp.Checked = config.requiereEspeciales;
            ckb2FA.Checked = config.requiere2FA;

            ckbRepetirContra.Checked = !config.noRepetirPasswords;

            ckbDatosPersonales.Checked = !config.validarDatosPersonales;

            if (config.cantPreguntas == 1)
                rb1.Checked = true;
            else if (config.cantPreguntas == 5)
                rb5.Checked = true;
            else
                rb3.Checked = true;
        }

        private async void btnRegistros_Click(object sender, EventArgs e)
        {
            try
            {
                btnRegistros.Enabled = false;
                btnRegistros.Text = "Actualizando...";

                bool resultado = await servicioGeoref.cargarGeorefEnBase();

                if (resultado)
                    MessageBox.Show("Datos geográficos actualizados correctamente.");
                else
                    MessageBox.Show("No se pudieron actualizar los datos geográficos.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar GeoRef: " + ex.Message);
            }
            finally
            {
                btnRegistros.Enabled = true;
                btnRegistros.Text = "Actualizar API";
            }
        }
    }
}
