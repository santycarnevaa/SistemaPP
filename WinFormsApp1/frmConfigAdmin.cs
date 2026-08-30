using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private CL_servicioConfiguracionCorreo servicioConfigCorreo = new CL_servicioConfiguracionCorreo();

        public frmConfigAdmin(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void lblPasswordNueva_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://myaccount.google.com/apppasswords") { UseShellExecute = true });
            }
            catch
            {
                new MensajeError("No se pudo abrir el navegador. Entrá manualmente a myaccount.google.com/apppasswords para generar la contraseña de aplicación.").Mostrar();
            }
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
                new MensajeError("Ingrese un número válido para la cantidad mínima de caracteres.").Mostrar();
                return;
            }
            configuracionSistema config = new configuracionSistema
            {
                minCaracteres = minCaracteres,
                requiereMayusculas = ckbMayusculas.Checked,
                requiereMinusculas = ckbMinusculas.Checked,
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
                new MensajeExito("Configuración guardada exitosamente.").Mostrar();
            }
            else
            {
                new MensajeError("Error al guardar la configuración. Intente nuevamente.").Mostrar();
            }
        }

        private void frmConfigAdmin_Load(object sender, EventArgs e)
        {
            configuracionSistema config = servicioConfiguracion.ObtenerConfiguracion();

            if (config == null)
            {
                new MensajeInformacion("No hay configuración cargada. Se usarán valores por defecto.").Mostrar();

                txtCaracteres.Text = "8";
                ckbMayusculas.Checked = false;
                ckbMinusculas.Checked = false;
                ckbNumeros.Checked = true;
                ckbCaracteresEsp.Checked = true;
                ckb2FA.Checked = false;
                ckbRepetirContra.Checked = false;
                ckbDatosPersonales.Checked = false;
                rb3.Checked = true;

                return;
            }

            txtCaracteres.Text = config.minCaracteres.ToString();

            ckbMayusculas.Checked = config.requiereMayusculas;
            ckbMinusculas.Checked = config.requiereMinusculas;
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

            CargarConfiguracionCorreo();
        }

        private void CargarConfiguracionCorreo()
        {
            var configCorreo = servicioConfigCorreo.ObtenerConfiguracion();

            if (configCorreo == null)
            {
                lblUltimaRotacion.Text = "Última rotación: nunca se cargó una credencial.";
                numDiasRotacion.Value = 90;
                return;
            }

            txtCorreoRemitente.Text = configCorreo.CorreoRemitente;
            numDiasRotacion.Value = configCorreo.DiasRotacion;
            lblUltimaRotacion.Text = "Última rotación: " + configCorreo.FechaUltimaRotacion.ToString("dd/MM/yyyy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCorreoRemitente.Text))
            {
                new MensajeError("Ingrese el correo remitente.").Mostrar();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPasswordNueva.Text))
            {
                new MensajeError("Ingrese la nueva contraseña de aplicación.").Mostrar();
                return;
            }

            try
            {
                bool ok = servicioConfigCorreo.RotarCredenciales(
                    txtCorreoRemitente.Text.Trim(),
                    txtPasswordNueva.Text,
                    (int)numDiasRotacion.Value
                );

                if (ok)
                {
                    new MensajeExito("Credenciales de correo actualizadas correctamente.").Mostrar();
                    txtPasswordNueva.Text = "";
                    CargarConfiguracionCorreo();
                }
                else
                {
                    new MensajeError("No se pudieron actualizar las credenciales.").Mostrar();
                }
            }
            catch (Exception ex)
            {
                new MensajeError("Error real al actualizar credenciales: " + ex.Message).Mostrar();
            }
        }

        private async void btnRegistros_Click(object sender, EventArgs e)
        {
            try
            {
                btnRegistros.Enabled = false;
                btnRegistros.Text = "Actualizando...";

                bool resultado = await servicioGeoref.cargarGeorefEnBase();

                if (resultado)
                    new MensajeExito("Datos geográficos actualizados correctamente.").Mostrar();
                else
                    new MensajeError("No se pudieron actualizar los datos geográficos.").Mostrar();
            }
            catch (Exception ex)
            {
                new MensajeError("Error al actualizar GeoRef: " + ex.Message).Mostrar();
            }
            finally
            {
                btnRegistros.Enabled = true;
                btnRegistros.Text = "Actualizar API";
            }
        }
    }
}
