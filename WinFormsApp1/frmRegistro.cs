using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CapaLogica;
using Entidades;

namespace CapaVista
{
    public partial class frmRegistro : Form
    {
        private CL_servicioGeoref servicioGeoref = new CL_servicioGeoref();
        private CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        private Color colorVerde = EstiloSistema.Verde;
        private Color colorGris = EstiloSistema.Gris;
        private Color colorRojo = EstiloSistema.Rojo;
        private Color colorAzul = Color.RoyalBlue;

        private bool hayCambios = false;
        private bool cargandoDatos = false;
        private string modo = "normal";

        private List<rol> rolesDisponibles = new List<rol>();
        private string usuario;

        public frmRegistro(string usuario)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
            ComboBuscable.Activar(cmbProvincia);
            ComboBuscable.Activar(cmbPartido);
            ComboBuscable.Activar(cmbLocalidad);
            this.usuario = usuario;

            btnbuscar.Click += btnbuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            filtroscb.SelectedIndexChanged += filtroscb_SelectedIndexChanged;
            txtboxbuscar.KeyDown += txtboxbuscar_KeyDown;

            EstiloGrilla.Aplicar(DgvPrueba);

            this.Load -= frmRegistro_Load;
            this.Load += frmRegistro_Load;

            cmbProvincia.SelectedIndexChanged -= cmbProvincia_SelectedIndexChanged;
            cmbProvincia.SelectedIndexChanged += cmbProvincia_SelectedIndexChanged;

            cmbPartido.SelectedIndexChanged -= cmbPartido_SelectedIndexChanged;
            cmbPartido.SelectedIndexChanged += cmbPartido_SelectedIndexChanged;
        }

        private async void frmRegistro_Load(object sender, EventArgs e)
        {
            cargarRoles();
            await cargarProvincias();
            actualizargrilla();

            datFecha.MaxDate = DateTime.Today.AddYears(-16);
            datFecha.MinDate = DateTime.Today.AddYears(-100);

            ConectarEventosCambios();
            VolverEstadoNormal();
            CargarFiltros();

        }

        private void CargarFiltros()
        {
            filtroscb.Items.Clear();

            filtroscb.Items.Add("Todos");
            filtroscb.Items.Add("Usuarios Activos");
            filtroscb.Items.Add("Usuarios Inactivos");
            filtroscb.Items.Add("Rol Usuario");
            filtroscb.Items.Add("Rol Administrador");

            filtroscb.SelectedIndex = 0;
        }

        private void PintarBoton(Button boton, Color color, bool habilitado)
        {
            boton.Enabled = habilitado;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.UseVisualStyleBackColor = false;
            boton.BackColor = color;
            boton.ForeColor = Color.White;
        }

        private void ConectarEventosCambios()
        {
            txtNombre.TextChanged += Campo_Cambio;
            txtApellido.TextChanged += Campo_Cambio;
            txtDni.TextChanged += Campo_Cambio;
            txtTelefono.TextChanged += Campo_Cambio;
            txtUsuario.TextChanged += Campo_Cambio;
            txtCorreo.TextChanged += Campo_Cambio;
            txtCalle.TextChanged += Campo_Cambio;
            txtNumCalle.TextChanged += Campo_Cambio;
            txtCP.TextChanged += Campo_Cambio;
            txtDepto.TextChanged += Campo_Cambio;
            txtPiso.TextChanged += Campo_Cambio;

            cmbRol.SelectedIndexChanged += Campo_Cambio;
            cmbProvincia.SelectedIndexChanged += Campo_Cambio;
            cmbPartido.SelectedIndexChanged += Campo_Cambio;
            cmbLocalidad.SelectedIndexChanged += Campo_Cambio;

            datFecha.ValueChanged += Campo_Cambio;
        }

        private void Campo_Cambio(object sender, EventArgs e)
        {
            if (cargandoDatos)
                return;

            if (modo == "agregar" || modo == "modificar")
            {
                hayCambios = true;
                ActualizarBotonGuardar();
            }
        }

        private void ActualizarBotonGuardar()
        {
            bool puedeGuardar = hayCambios;

            if (modo == "agregar")
                PintarBoton(btnAgregar, puedeGuardar ? colorVerde : colorGris, puedeGuardar);

            if (modo == "modificar")
                PintarBoton(btnModificar, puedeGuardar ? colorVerde : colorGris, puedeGuardar);
        }

        private void VolverEstadoNormal()
        {
            modo = "normal";
            hayCambios = false;

            btnAgregar.Text = "Agregar";
            btnModificar.Text = "Modificar";
            btnEliminar.Text = "Dar de baja";

            btnAgregar.Visible = true;
            btnModificar.Visible = true;
            btnEliminar.Visible = true;

            PintarBoton(btnAgregar, colorVerde, true);
            PintarBoton(btnModificar, colorVerde, true);
            PintarBoton(btnEliminar, colorRojo, true);

            limpiarcontroles(groupBox1);
            bloqueoControles(groupBox1, true);

            txtUsuario.Enabled = true;

            if (DgvPrueba.Rows.Count > 0)
                DgvPrueba.ClearSelection();

            DgvPrueba.Focus();
        }

        private void ModoAgregar()
        {
            modo = "agregar";
            hayCambios = false;

            btnAgregar.Text = "Guardar";
            btnModificar.Visible = false;
            btnEliminar.Text = "Cancelar";

            PintarBoton(btnAgregar, colorGris, false);
            PintarBoton(btnEliminar, colorAzul, true);

            limpiarcontroles(groupBox1);
            bloqueoControles(groupBox1, false);

            txtUsuario.Enabled = true;

            cargandoDatos = true;
            cmbRol.SelectedIndex = -1;
            cmbProvincia.SelectedIndex = -1;
            cmbPartido.DataSource = null;
            cmbLocalidad.DataSource = null;
            cargandoDatos = false;

            txtNombre.Focus();
        }

        private void ModoModificar()
        {
            if (DgvPrueba.SelectedRows.Count == 0)
            {
                new MensajeError("Seleccione un usuario para modificar.").Mostrar();
                return;
            }

            modo = "modificar";
            hayCambios = false;

            btnModificar.Text = "Guardar cambios";
            btnAgregar.Visible = false;
            btnEliminar.Text = "Cancelar";

            PintarBoton(btnModificar, colorGris, false);
            PintarBoton(btnEliminar, colorAzul, true);

            bloqueoControles(groupBox1, false);
            txtUsuario.Enabled = false;

            txtNombre.Focus();
        }

        private void cargarRoles()
        {
            rolesDisponibles = servicioUsuarios.ListarRoles();

            cmbRol.Items.Clear();
            foreach (rol rol in rolesDisponibles)
                cmbRol.Items.Add(rol.NombreRol);

            cmbRol.SelectedIndex = -1;
        }

        private int obtenerIdRol()
        {
            rol rolSeleccionado = rolesDisponibles.FirstOrDefault(r => r.NombreRol == cmbRol.Text);
            return rolSeleccionado != null ? rolSeleccionado.IdRol : 0;
        }

        private async Task cargarProvincias()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // En una instalación limpia las tablas de georreferencia están
                // vacías; cargarGeorefEnBase() ya se cuida de no volver a pedirle
                // datos a la API si ya están cargados, así que es seguro llamarla
                // siempre acá.
                bool georefOk = await servicioGeoref.cargarGeorefEnBase();

                if (!georefOk)
                {
                    new MensajeError("No se pudieron cargar los datos geográficos desde la API. Verificá la conexión a internet e intentá de nuevo.").Mostrar();
                }

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
                new MensajeError("Error al cargar provincias: " + ex.Message).Mostrar();
            }
            finally
            {
                this.Cursor = Cursors.Default;
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
                new MensajeError("Error al cargar partidos: " + ex.Message).Mostrar();
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
                new MensajeError("Error al cargar localidades: " + ex.Message).Mostrar();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            btnAgregar_Click(sender, e);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (modo == "normal")
            {
                ModoAgregar();
                return;
            }

            if (modo != "agregar")
                return;

            if (!hayCambios)
                return;

            if (!ValidarCamposRegistro())
                return;

            int idRol = obtenerIdRol();

            if (idRol == 0)
            {
                ErrorCampo.Mostrar(cmbRol, "Seleccioná un rol válido.");
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
                    Convert.ToString(cmbLocalidad.SelectedValue),
                    txtNombre.Text,
                    txtApellido.Text,
                    txtDni.Text,
                    txtTelefono.Text,
                    datFecha.Value,
                    txtUsuario.Text,
                    txtCorreo.Text,
                    idRol
                );

                if (resultado == resultadoRegistroUsuario.Ok)
                {
                    // si llegó acá, el correo con la contraseña temporal ya se envió
                    new MensajeExito("Usuario registrado correctamente.\nEl correo se ha enviado correctamente a " + txtCorreo.Text.Trim() + ".").Mostrar();
                    actualizargrilla();
                    VolverEstadoNormal();
                }
                else
                {
                    MostrarErrorRegistro(resultado);
                }
            }
            catch (Exception ex)
            {
                new MensajeError("Error real al registrar usuario: " + ex.Message).Mostrar();
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string texto = txtboxbuscar.Text.Trim();
            string filtro = filtroscb.Text;

            DgvPrueba.DataSource = servicioUsuarios.BuscarUsuariosParaGrilla(texto, filtro);

            FormatearGrilla();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtboxbuscar.Clear();
            filtroscb.SelectedIndex = 0;
            actualizargrilla();
        }

        private void txtboxbuscar_KeyDown(object sender, KeyEventArgs e) // es para que se pueda buscar con la tecla enter
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnbuscar.PerformClick();

                e.SuppressKeyPress = true;
            }
        }

        private void filtroscb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filtroscb.SelectedIndex >= 0)
            {
                btnbuscar.PerformClick();
            }
        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            btnModificar_Click(sender, e);
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtboxbuscar.Clear();

            filtroscb.SelectedIndex = 0;

            actualizargrilla();

            txtboxbuscar.Focus();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (modo == "normal")
            {
                ModoModificar();
                return;
            }

            if (modo != "modificar")
                return;

            if (!hayCambios)
                return;

            if (!ValidarCamposModificacion())
                return;

            int idRol = obtenerIdRol();

            if (idRol == 0)
            {
                ErrorCampo.Mostrar(cmbRol, "Seleccioná un rol válido.");
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
                    Convert.ToString(cmbLocalidad.SelectedValue),
                    txtCorreo.Text,
                    idRol
                );

                if (ok)
                {
                    new MensajeExito("Usuario modificado correctamente.").Mostrar();
                    actualizargrilla();
                    VolverEstadoNormal();
                }
                else
                {
                    new MensajeError("No se pudo modificar el usuario.").Mostrar();
                }
            }
            catch (Exception ex)
            {
                new MensajeError("Error real al modificar usuario: " + ex.Message).Mostrar();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (modo == "agregar" || modo == "modificar")
            {
                VolverEstadoNormal();
                return;
            }

            if (DgvPrueba.SelectedRows.Count == 0)
            {
                new MensajeError("Seleccione un usuario.").Mostrar();
                return;
            }

            int idUsuario = Convert.ToInt32(DgvPrueba.SelectedRows[0].Cells["IdUsuario"].Value);
            bool activo = Convert.ToBoolean(DgvPrueba.SelectedRows[0].Cells["Activo"].Value);

            string accion = activo ? "dar de baja" : "activar";

            if (!new MensajeConfirmacion($"¿Desea {accion} este usuario?").Confirmado())
                return;

            bool ok = servicioUsuarios.CambiarEstadoUsuario(idUsuario, !activo);

            if (ok)
            {
                new MensajeExito(activo ? "Usuario dado de baja correctamente." : "Usuario activado correctamente.").Mostrar();
                actualizargrilla();
                VolverEstadoNormal();
            }
            else
            {
                new MensajeError("No se pudo actualizar el estado.").Mostrar();
            }
        }

        private bool ValidarCamposRegistro()
        {
            ErrorCampo.Limpiar(this);

            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                ErrorCampo.Mostrar(txtUsuario, "Obligatorio.");
                ok = false;
            }

            return ValidarCamposComunes() && ok;
        }

        private bool ValidarCamposModificacion()
        {
            ErrorCampo.Limpiar(this);
            return ValidarCamposComunes();
        }

        // Validaciones comunes a alta y modificación de usuario (T15): antes
        // estaban duplicadas, y la modificación ni siquiera validaba DNI ni
        // teléfono. Usuario (solo se edita en el alta) se valida aparte.
        // Marca todos los campos que estén mal, cada uno con su texto abajo.
        private bool ValidarCamposComunes()
        {
            bool ok = true;

            foreach (TextBox caja in new[] { txtNombre, txtApellido, txtDni, txtTelefono, txtCorreo, txtCalle, txtNumCalle, txtCP })
            {
                if (string.IsNullOrWhiteSpace(caja.Text))
                {
                    ErrorCampo.Mostrar(caja, "Obligatorio.");
                    ok = false;
                }
            }

            foreach (ComboBox combo in new[] { cmbProvincia, cmbPartido, cmbLocalidad, cmbRol })
            {
                if (combo.SelectedIndex < 0)
                {
                    ErrorCampo.Mostrar(combo, "Elegí una opción.");
                    ok = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtDni.Text) && !Regex.IsMatch(txtDni.Text.Trim(), @"^\d{7,8}$"))
            {
                ErrorCampo.Mostrar(txtDni, "Debe tener 7 u 8 números.");
                ok = false;
            }

            if (!string.IsNullOrWhiteSpace(txtTelefono.Text) && !Regex.IsMatch(txtTelefono.Text.Trim(), @"^\d{8,15}$"))
            {
                ErrorCampo.Mostrar(txtTelefono, "Entre 8 y 15 números.");
                ok = false;
            }

            return ValidarEdad() && ok;
        }

        // Los errores que devuelve la lógica y tienen un campo van abajo de ese campo
        private void MostrarErrorRegistro(resultadoRegistroUsuario resultado)
        {
            string mensaje = servicioUsuarios.ObtenerMensajeRegistro(resultado);

            switch (resultado)
            {
                case resultadoRegistroUsuario.UsuarioYaExiste:
                    ErrorCampo.Mostrar(txtUsuario, mensaje);
                    break;
                case resultadoRegistroUsuario.CorreoYaExiste:
                case resultadoRegistroUsuario.CorreoInvalido:
                    ErrorCampo.Mostrar(txtCorreo, mensaje);
                    break;
                case resultadoRegistroUsuario.DniYaExiste:
                    ErrorCampo.Mostrar(txtDni, mensaje);
                    break;
                default:
                    new MensajeInformacion(mensaje).Mostrar();
                    break;
            }
        }

        private bool ValidarEdad()
        {
            DateTime fechaNacimiento = datFecha.Value.Date;
            DateTime hoy = DateTime.Today;

            int edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento > hoy.AddYears(-edad))
                edad--;

            if (edad < 16)
            {
                ErrorCampo.Mostrar(datFecha, "Debe tener al menos 16 años.");
                return false;
            }

            return true;
        }

        private void actualizargrilla()
        {
            try
            {
                DgvPrueba.DataSource = null;
                DgvPrueba.DataSource = servicioUsuarios.ObtenerUsuariosParaGrilla();

                FormatearGrilla();
            }
            catch (Exception ex)
            {
                new MensajeError("Error al cargar usuarios en la grilla: " + ex.Message).Mostrar();
            }
        }

        // Estilo, columnas ocultas y orden de la grilla, para usarlo tanto al listar como al buscar
        private void FormatearGrilla()
        {
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
            CambiarTitulo("Telefono", "Teléfono");
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

            AjusteVentana.AjustarAGrilla(this, DgvPrueba);
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
            if (modo == "agregar" || modo == "modificar")
                return;

            if (e.RowIndex < 0)
                return;

            if (DgvPrueba.Rows[e.RowIndex].DataBoundItem == null)
                return;

            cargandoDatos = true;

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

            bool activo = Convert.ToBoolean(fila.Cells["Activo"].Value);

            btnEliminar.Text = activo ? "Dar de baja" : "Activar";
            PintarBoton(btnEliminar, activo ? colorRojo : colorVerde, true);

            cargandoDatos = false;
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

            groupBox1.Text = "Nuevo Usuario:";
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
                e.Handled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmAdmin frm = new frmAdmin(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void lblFecha_Click(object sender, EventArgs e)
        {
        }

        private void lblbuscar_Click(object sender, EventArgs e)
        {

        }
    }
}
