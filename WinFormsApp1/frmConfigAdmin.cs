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
using Utilidades;

namespace CapaVista
{
    public partial class frmConfigAdmin : Form
    {
        private string usuario;
        private CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();
        private CL_servicioGeoref servicioGeoref = new CL_servicioGeoref();
        private CL_servicioConfiguracionCorreo servicioConfigCorreo = new CL_servicioConfiguracionCorreo();
        private CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        public frmConfigAdmin(string usuario)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
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

        // GUARDAR solo sirve para las solapas Registro y Seguridad, en las otras no se muestra
        private void tabConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGuardar.Visible = tabConfig.SelectedIndex <= 1;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbMinCaracteres.SelectedIndex < 0)
            {
                ErrorCampo.Mostrar(cmbMinCaracteres, "Seleccioná el mínimo de caracteres.");
                return;
            }

            configuracionSistema config = new configuracionSistema
            {
                minCaracteres = int.Parse(cmbMinCaracteres.Text),
                requiereMayusculas = ckbMayusculas.Checked,
                requiereMinusculas = ckbMinusculas.Checked,
                requiereNumeros = ckbNumeros.Checked,
                requiereEspeciales = ckbCaracteresEsp.Checked,
                requiere2FA = ckb2FA.Checked,
                noRepetirPasswords = !ckbRepetirContra.Checked,
                validarDatosPersonales = !ckbDatosPersonales.Checked,
                cantPreguntas = rb1.Checked ? 1 : rb5.Checked ? 5 : 3
            };

            try
            {
                bool resultado = servicioConfiguracion.actualizarConfiguracion(config);

                if (!resultado)
                {
                    new MensajeError("Error al guardar la configuración. Intente nuevamente.").Mostrar();
                    return;
                }

                if (!GuardarDiasRotacion())
                {
                    new MensajeError("Se guardó la configuración, pero no se pudieron guardar los días de rotación.").Mostrar();
                    return;
                }

                new MensajeExito("Configuración guardada exitosamente.").Mostrar();
            }
            catch (Exception ex)
            {
                // antes una falla de la base cerraba el programa con una excepción sin explicar nada
                Logger.RegistrarError("frmConfigAdmin.btnGuardar_Click", ex);
                new MensajeError("No se pudo guardar la configuración. Revisá la conexión con la base de datos e intentá nuevamente.").Mostrar();
            }
        }

        // Los días de rotación se guardan junto con la cuenta de correo del sistema
        private bool GuardarDiasRotacion()
        {
            ConfiguracionCorreo configCorreo = servicioConfigCorreo.ObtenerConfiguracion();

            if (configCorreo == null)
                return true;

            return servicioConfigCorreo.ActualizarSoloDias(configCorreo.CorreoRemitente, (int)numDiasRotacion.Value);
        }

        private void frmConfigAdmin_Load(object sender, EventArgs e)
        {
            ConfigurarGrillaRoles();
            CargarRoles();
            CargarConfiguracionCorreo();

            configuracionSistema config = servicioConfiguracion.ObtenerConfiguracion();

            if (config == null)
            {
                new MensajeInformacion("No hay configuración cargada. Se usarán valores por defecto.").Mostrar();

                SeleccionarMinimoCaracteres(8);
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

            SeleccionarMinimoCaracteres(config.minCaracteres);

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
        }

        // Si el valor guardado no es 5, 8 o 15 (por ejemplo, 6), se agrega a la lista para no perderlo
        private void SeleccionarMinimoCaracteres(int valor)
        {
            string texto = valor.ToString();

            if (!cmbMinCaracteres.Items.Contains(texto))
                cmbMinCaracteres.Items.Add(texto);

            cmbMinCaracteres.SelectedItem = texto;
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

            numDiasRotacion.Value = Math.Min(365, Math.Max(1, configCorreo.DiasRotacion));
            lblUltimaRotacion.Text = "Última rotación: " + configCorreo.FechaUltimaRotacion.ToString("dd/MM/yyyy");
        }

        private void btnActualizarCuenta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPasswordNueva.Text))
            {
                ErrorCampo.Mostrar(txtPasswordNueva, "Ingresá la nueva contraseña de aplicación.");
                return;
            }

            ConfiguracionCorreo configCorreo = servicioConfigCorreo.ObtenerConfiguracion();

            if (configCorreo == null)
            {
                new MensajeError("No hay un correo configurado para enviar los mails. Revisá el archivo mailsettings.json.").Mostrar();
                return;
            }

            try
            {
                // Google muestra la contraseña con espacios, pero se usa sin ellos
                bool ok = servicioConfigCorreo.RotarCredenciales(
                    configCorreo.CorreoRemitente,
                    txtPasswordNueva.Text.Replace(" ", ""),
                    (int)numDiasRotacion.Value
                );

                if (ok)
                {
                    new MensajeExito("Contraseña del correo actualizada correctamente.").Mostrar();
                    txtPasswordNueva.Text = "";
                    CargarConfiguracionCorreo();
                }
                else
                {
                    new MensajeError("No se pudo actualizar la contraseña del correo.").Mostrar();
                }
            }
            catch (Exception ex)
            {
                new MensajeError("Error real al actualizar la contraseña: " + ex.Message).Mostrar();
            }
        }

        private void ConfigurarGrillaRoles()
        {
            EstiloGrilla.Aplicar(dgvRoles);
        }

        private void CargarRoles()
        {
            dgvRoles.DataSource = null;
            dgvRoles.DataSource = servicioUsuarios.ListarRoles();

            if (dgvRoles.Columns["IdRol"] != null)
            {
                dgvRoles.Columns["IdRol"].HeaderText = "ID";
                dgvRoles.Columns["IdRol"].FillWeight = 20;
            }

            if (dgvRoles.Columns["NombreRol"] != null)
                dgvRoles.Columns["NombreRol"].HeaderText = "Rol";

            ActualizarBotonesRoles();
        }

        private rol RolSeleccionado()
        {
            return dgvRoles.CurrentRow?.DataBoundItem as rol;
        }

        // Modificar y dar de baja solo se pueden usar con un rol seleccionado
        private void ActualizarBotonesRoles()
        {
            bool haySeleccion = dgvRoles.SelectedRows.Count > 0;

            btnModificarRol.Enabled = haySeleccion;
            btnBajaRol.Enabled = haySeleccion;
        }

        private void dgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotonesRoles();
        }

        private void btnAgregarRol_Click(object sender, EventArgs e)
        {
            string mensaje = servicioUsuarios.AgregarRol(txtNuevoRol.Text);

            if (mensaje != "")
            {
                ErrorCampo.Mostrar(txtNuevoRol, mensaje);
                return;
            }

            new MensajeExito("Rol agregado correctamente.").Mostrar();
            txtNuevoRol.Text = "";
            CargarRoles();
        }

        private void btnModificarRol_Click(object sender, EventArgs e)
        {
            rol rolSeleccionado = RolSeleccionado();

            if (rolSeleccionado == null)
            {
                new MensajeError("Seleccione un rol.").Mostrar();
                return;
            }

            using (frmModificarRol frm = new frmModificarRol(rolSeleccionado.IdRol, rolSeleccionado.NombreRol))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    CargarRoles();
            }
        }

        private void btnBajaRol_Click(object sender, EventArgs e)
        {
            rol rolSeleccionado = RolSeleccionado();

            if (rolSeleccionado == null)
            {
                new MensajeError("Seleccione un rol.").Mostrar();
                return;
            }

            if (!new MensajeConfirmacion("¿Desea dar de baja el rol \"" + rolSeleccionado.NombreRol + "\"?").Confirmado())
                return;

            string mensaje = servicioUsuarios.EliminarRol(rolSeleccionado.IdRol);

            if (mensaje != "")
            {
                new MensajeError(mensaje).Mostrar();
                return;
            }

            new MensajeExito("Rol dado de baja correctamente.").Mostrar();
            CargarRoles();
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
