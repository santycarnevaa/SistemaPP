using CapaLogica;
using Entidades;
using System;
using System.Drawing;
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

            configurargrilla(DgvPrueba);

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
            actualizargrilla();

            bloqueoControles(groupBox1, true);
        }

        private void cargarRoles()
        {
            cmbRol.Items.Clear();

            cmbRol.Items.Add("Usuario");
            cmbRol.Items.Add("Administrador");

            cmbRol.SelectedIndex = -1;
        }

        private int obtenerIdRol()
        {
            string rolSeleccionado = cmbRol.Text;

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

        // Si tu botón Agregar todavía está conectado al evento btnAceptar_Click,
        // dejá este método. Si está conectado a btnAgregar_Click, también funciona.
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            btnAgregar_Click(sender, e);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (btnAgregar.Text == "Agregar")
            {
                btnAgregar.Text = "Guardar";
                btnModificar.Visible = false;
                btnEliminar.Text = "Cancelar";

                limpiarcontroles(groupBox1);
                bloqueoControles(groupBox1, false);

                cmbRol.SelectedIndex = -1;
                cmbProvincia.SelectedIndex = -1;
                cmbPartido.DataSource = null;
                cmbLocalidad.DataSource = null;

                txtNombre.Focus();
                return;
            }

            if (!ValidarCamposRegistro())
                return;

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
                    btnAgregar.Text = "Agregar";
                    btnModificar.Visible = true;
                    btnEliminar.Text = "Dar de baja";

                    bloqueoControles(groupBox1, true);
                    limpiarcontroles(groupBox1);
                    actualizargrilla();

                    DgvPrueba.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error real al registrar usuario: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            btnModificar_Click(sender, e);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (DgvPrueba.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario.");
                return;
            }

            if (btnModificar.Text == "Modificar")
            {
                btnModificar.Text = "Guardar cambios";
                btnAgregar.Visible = false;
                btnEliminar.Text = "Cancelar";

                bloqueoControles(groupBox1, false);

                txtUsuario.Enabled = false;

                txtNombre.Focus();
                return;
            }

            if (!ValidarCamposModificacion())
                return;

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
                int idUsuario = Convert.ToInt32(DgvPrueba.SelectedRows[0].Cells["IdUsuario"].Value);

                bool ok = servicioUsuarios.ActualizarDatosUsuario(
                    idUsuario,

                    txtNombre.Text,
                    txtApellido.Text,
                    txtDni.Text,
                    txtTelefono.Text,
                    datFecha.Value,

                    txtCalle.Text,
                    txtNumCalle.Text,
                    txtCP.Text,
                    txtDepto.Text,
                    txtPiso.Text,
                    cmbProvincia.Text,
                    cmbPartido.Text,
                    cmbLocalidad.Text,

                    txtCorreo.Text,
                    idRol
                );

                if (ok)
                {
                    MessageBox.Show("Usuario modificado correctamente.");

                    btnModificar.Text = "Modificar";
                    btnAgregar.Visible = true;
                    btnEliminar.Text = "Dar de baja";

                    bloqueoControles(groupBox1, true);
                    limpiarcontroles(groupBox1);

                    actualizargrilla();

                    DgvPrueba.ClearSelection();
                    DgvPrueba.Focus();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error real al modificar usuario: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (DgvPrueba.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario.");
                return;
            }

            int idUsuario =
                Convert.ToInt32(DgvPrueba.SelectedRows[0].Cells["IdUsuario"].Value);

            bool activo =
                Convert.ToBoolean(DgvPrueba.SelectedRows[0].Cells["Activo"].Value);

            string accion = activo ? "dar de baja" : "activar";

            DialogResult r = MessageBox.Show(
                $"¿Desea {accion} este usuario?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (r != DialogResult.Yes)
                return;

            bool ok = servicioUsuarios.CambiarEstadoUsuario(
                idUsuario,
                !activo
            );

            if (ok)
            {
                MessageBox.Show(
                    activo
                        ? "Usuario dado de baja correctamente."
                        : "Usuario activado correctamente."
                );

                actualizargrilla();

                DgvPrueba.ClearSelection();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el estado.");
            }
        }

        private bool ValidarCamposRegistro()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtDni.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtCalle.Text) ||
                string.IsNullOrWhiteSpace(txtNumCalle.Text) ||
                string.IsNullOrWhiteSpace(txtCP.Text) ||
                string.IsNullOrWhiteSpace(cmbProvincia.Text) ||
                string.IsNullOrWhiteSpace(cmbPartido.Text) ||
                string.IsNullOrWhiteSpace(cmbLocalidad.Text) ||
                string.IsNullOrWhiteSpace(cmbRol.Text))
            {
                MessageBox.Show(
                    "Debe completar todos los campos obligatorios.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            if (!int.TryParse(txtDni.Text, out _))
            {
                MessageBox.Show(
                    "El DNI debe ser numérico.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            if (!int.TryParse(txtTelefono.Text, out _))
            {
                MessageBox.Show(
                    "El teléfono debe ser numérico.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            return true;
        }

        private bool ValidarCamposModificacion()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(cmbRol.Text))
            {
                MessageBox.Show(
                    "Debe completar nombre, apellido, correo y rol.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            return true;
        }

        private void configurargrilla(DataGridView dgv)
        {
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Century Gothic", 11, FontStyle.Bold);

            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.GridColor = Color.Black;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGray;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DarkGray;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void actualizargrilla()
        {
            try
            {
                DgvPrueba.DataSource = null;
                DgvPrueba.DataSource = servicioUsuarios.ObtenerUsuariosParaGrilla();

                DgvPrueba.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvPrueba.ReadOnly = true;

                OcultarColumna("IdPersona");
                OcultarColumna("IdDireccion");
                OcultarColumna("IdRol");

                OcultarColumna("Activo");

                OcultarColumna("FechaNacimiento");
                OcultarColumna("CodigoPostal");
                OcultarColumna("Depto");
                OcultarColumna("Piso");
                OcultarColumna("Provincia");
                OcultarColumna("Partido");
                OcultarColumna("Localidad");

                CambiarTitulo("IdUsuario", "ID");
                CambiarTitulo("DNI", "DNI");
                CambiarTitulo("Telefono", "Teléfono");
                CambiarTitulo("Numero", "Número");
                CambiarTitulo("Estado", "Estado");

                if (DgvPrueba.Columns["IdUsuario"] != null)
                    DgvPrueba.Columns["IdUsuario"].DisplayIndex = 0;

                if (DgvPrueba.Columns["Nombre"] != null)
                    DgvPrueba.Columns["Nombre"].DisplayIndex = 1;

                if (DgvPrueba.Columns["DNI"] != null)
                    DgvPrueba.Columns["DNI"].DisplayIndex = 2;

                if (DgvPrueba.Columns["Usuario"] != null)
                    DgvPrueba.Columns["Usuario"].DisplayIndex = 3;

                if (DgvPrueba.Columns["Correo"] != null)
                    DgvPrueba.Columns["Correo"].DisplayIndex = 4;

                if (DgvPrueba.Columns["Estado"] != null)
                    DgvPrueba.Columns["Estado"].DisplayIndex = 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar usuarios en la grilla: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void OcultarColumna(string nombre)
        {
            if (DgvPrueba.Columns[nombre] != null)
                DgvPrueba.Columns[nombre].Visible = false;
        }

        private void CambiarTitulo(string columna, string titulo)
        {
            if (DgvPrueba.Columns[columna] != null)
                DgvPrueba.Columns[columna].HeaderText = titulo;
        }

        private void DgvPrueba_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (DgvPrueba.Rows[e.RowIndex].DataBoundItem == null)
                return;

            DataGridViewRow fila = DgvPrueba.Rows[e.RowIndex];

            groupBox1.Text = "usuario numero: " + fila.Cells["IdUsuario"].Value?.ToString();

            txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
            txtApellido.Text = fila.Cells["Apellido"].Value?.ToString();
            txtDni.Text = fila.Cells["DNI"].Value?.ToString();
            txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString();

            txtUsuario.Text = fila.Cells["Usuario"].Value?.ToString();
            txtCorreo.Text = fila.Cells["Correo"].Value?.ToString();

            txtCalle.Text = fila.Cells["Calle"].Value?.ToString();
            txtNumCalle.Text = fila.Cells["Numero"].Value?.ToString();
            txtCP.Text = fila.Cells["CodigoPostal"].Value?.ToString();
            txtDepto.Text = fila.Cells["Depto"].Value?.ToString();
            txtPiso.Text = fila.Cells["Piso"].Value?.ToString();

            cmbRol.Text = fila.Cells["Rol"].Value?.ToString();

            cmbProvincia.Text = fila.Cells["Provincia"].Value?.ToString();
            cmbPartido.Text = fila.Cells["Partido"].Value?.ToString();
            cmbLocalidad.Text = fila.Cells["Localidad"].Value?.ToString();

            if (fila.Cells["FechaNacimiento"].Value != null &&
                fila.Cells["FechaNacimiento"].Value != DBNull.Value)
            {
                datFecha.Value = Convert.ToDateTime(fila.Cells["FechaNacimiento"].Value);
            }

            if (fila.Cells["Activo"].Value != null &&
                fila.Cells["Activo"].Value != DBNull.Value)
            {
                bool activo = Convert.ToBoolean(fila.Cells["Activo"].Value);

                if (activo)
                    btnEliminar.Text = "Dar de baja";
                else
                    btnEliminar.Text = "Activar";
            }
        }

        private void limpiarcontroles(Control control)
        {
            foreach (Control item in control.Controls)
            {
                if (item is TextBox txt)
                    txt.Text = "";

                if (item is ComboBox cmb)
                    cmb.SelectedIndex = -1;

                if (item is GroupBox || item is Panel)
                    limpiarcontroles(item);
            }

            groupBox1.Text = "usuario numero:";
        }

        private void bloqueoControles(Control control, bool bloqueado = true)
        {
            foreach (Control item in control.Controls)
            {
                if (item is TextBox txt)
                    txt.Enabled = !bloqueado;

                if (item is ComboBox cmb)
                    cmb.Enabled = !bloqueado;

                if (item is DateTimePicker dtp)
                    dtp.Enabled = !bloqueado;

                if (item is GroupBox || item is Panel)
                    bloqueoControles(item, bloqueado);
            }
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            frm.Show();
            this.Close();
        }

        private void DgvPrueba_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}