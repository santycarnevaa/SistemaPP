using CapaLogica;
using System;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class frmRegistro : Form
    {
        private CL_servicioGeoref servicioGeoref = new CL_servicioGeoref();
        private CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        public frmRegistro()
        {
            InitializeComponent();

            // Evita que se dupliquen eventos si ya estaban conectados desde el diseñador
            this.Load -= frmRegistro_Load;
            this.Load += frmRegistro_Load;

            cmbProvincia.SelectedIndexChanged -= cmbProvincia_SelectedIndexChanged;
            cmbProvincia.SelectedIndexChanged += cmbProvincia_SelectedIndexChanged;

            cmbPartido.SelectedIndexChanged -= cmbPartido_SelectedIndexChanged;
            cmbPartido.SelectedIndexChanged += cmbPartido_SelectedIndexChanged;
        }

        private void frmRegistro_Load(object sender, EventArgs e)
        {
            cargarRoles();
            cargarProvincias();

            btnModificar.Visible = false;
            btnAceptar.Visible = true;
        }

        private void cargarRoles()
        {
            cmbRol.Items.Clear();

            cmbRol.Items.Add("Usuario");
            cmbRol.Items.Add("Administrador");

            cmbRol.SelectedIndex = -1;
        }

        private void cargarProvincias()
        {
            try
            {
                var provincias = servicioGeoref.ObtenerProvincias();

                cmbProvincia.DataSource = null;
                cmbProvincia.DisplayMember = "Nombre";
                cmbProvincia.ValueMember = "IdProvincia";
                cmbProvincia.DataSource = provincias;
                cmbProvincia.SelectedIndex = -1;

                cmbPartido.DataSource = null;
                cmbLocalidad.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar provincias: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void cmbProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProvincia.SelectedValue == null)
                    return;

                string idProvincia = cmbProvincia.SelectedValue.ToString();

                if (string.IsNullOrWhiteSpace(idProvincia))
                    return;

                var partidos = servicioGeoref.ObtenerPartidosPorProvincia(idProvincia);

                cmbPartido.DataSource = null;
                cmbPartido.DisplayMember = "Nombre";
                cmbPartido.ValueMember = "IdPartido";
                cmbPartido.DataSource = partidos;
                cmbPartido.SelectedIndex = -1;

                cmbLocalidad.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar partidos: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void cmbPartido_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPartido.SelectedValue == null)
                    return;

                string idPartido = cmbPartido.SelectedValue.ToString();

                if (string.IsNullOrWhiteSpace(idPartido))
                    return;

                var localidades = servicioGeoref.ObtenerLocalidadesPorPartido(idPartido);

                cmbLocalidad.DataSource = null;
                cmbLocalidad.DisplayMember = "Nombre";
                cmbLocalidad.ValueMember = "IdLocalidad";
                cmbLocalidad.DataSource = localidades;
                cmbLocalidad.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar localidades: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int idRol = obtenerIdRol();

            if (idRol == 0)
            {
                MessageBox.Show(
                    "Seleccione un rol válido.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            try
            {
                resultadoRegistroUsuario resultado = servicioUsuarios.RegistrarUsuario(
                    txtCalle.Text,
                    txtNumCalle.Text,
                    txtCP.Text,
                    txtDepto.Text,
                    txtPiso.Text,
                    cmbProvincia.Text,
                    cmbPartido.Text,
                    cmbLocalidad.Text,

                    txtNombre.Text,
                    txtApellido.Text,
                    txtDni.Text,
                    txtTelefono.Text,
                    datFecha.Value,

                    txtUsuario.Text,
                    txtCorreo.Text,
                    idRol
                );

                MessageBox.Show(servicioUsuarios.ObtenerMensajeRegistro(resultado));

                if (resultado == resultadoRegistroUsuario.Ok)
                {
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error real: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private int obtenerIdRol()
        {
            string rolSeleccionado = cmbRol.SelectedItem?.ToString();

            switch (rolSeleccionado)
            {
                case "Usuario":
                    return 1;

                case "Administrador":
                    return 2;

                default:
                    return 0;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmAdmin frm = new frmAdmin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Por ahora queda vacío.
            // Si después usás modificación de usuario, la lógica iría acá.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Limpiar();

            btnModificar.Visible = false;
            btnAceptar.Visible = true;
        }

        private void Limpiar()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtUsuario.Text = "";
            txtDni.Text = "";

            txtCalle.Text = "";
            txtNumCalle.Text = "";
            txtCP.Text = "";
            txtDepto.Text = "";
            txtPiso.Text = "";

            cmbProvincia.SelectedIndex = -1;
            cmbPartido.DataSource = null;
            cmbLocalidad.DataSource = null;
            cmbRol.SelectedIndex = -1;

            datFecha.Value = DateTime.Today;
        }

        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            TeclaNum(e);
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            TeclaNum(e);
        }

        private void txtNumCalle_KeyPress(object sender, KeyPressEventArgs e)
        {
            TeclaNum(e);
        }

        private void txtCP_KeyPress(object sender, KeyPressEventArgs e)
        {
            TeclaNum(e);
        }

        private void txtPiso_KeyPress(object sender, KeyPressEventArgs e)
        {
            TeclaNum(e);
        }

        private void TeclaNum(KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        //    try
        //    {
        //        var resultado = servicioUsuarios.RegistrarUsuario(
        //    txtCalle.Text,
        //    txtNumCalle.Text,
        //    txtCP.Text,
        //    txtDepto.Text,
        //    txtPiso.Text,
        //    cmbProvincia.Text,
        //    cmbPartido.Text,
        //    cmbLocalidad.Text,

        //    txtNombre.Text,
        //    txtApellido.Text,
        //    txtDni.Text,
        //    txtTelefono.Text,
        //    datFecha.Value,

        //    txtUsuario.Text,
        //    txtCorreo.Text,
        //    idRol
        //    );

        //        MessageBox.Show(servicioUsuarios.ObtenerMensajeRegistro(resultado));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error real: " + ex.Message);
        //    }
    }
}