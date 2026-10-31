using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmAuditoria : Form
    {
        private const string OpcionTodas = "(Todas)";
        private const string OpcionTodos = "(Todos)";

        private readonly CL_servicioAuditoria servicioAuditoria = new CL_servicioAuditoria();

        public frmAuditoria()
        {
            InitializeComponent();
        }

        private void frmAuditoria_Load(object sender, EventArgs e)
        {
            CargarFiltros();
            dtpHasta.Value = DateTime.Today;
            Filtrar();
        }

        private void CargarFiltros()
        {
            // Las tablas salen de lo que realmente quedó auditado (de cualquier módulo).
            List<string> tablas = new List<string> { OpcionTodas };
            tablas.AddRange(servicioAuditoria.ObtenerTablasAuditadas());
            cmbTabla.DataSource = tablas;

            List<KeyValuePair<int, string>> usuarios = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, OpcionTodos)
            };
            usuarios.AddRange(servicioAuditoria.ObtenerUsuarios());

            cmbUsuario.DataSource = usuarios;
            cmbUsuario.DisplayMember = "Value";
            cmbUsuario.ValueMember = "Key";
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            Filtrar();
        }

        private void Filtrar()
        {
            string tabla = cmbTabla.SelectedItem?.ToString();

            if (tabla == OpcionTodas)
                tabla = null;

            int? idUsuario = null;

            if (cmbUsuario.SelectedValue is int id && id > 0)
                idUsuario = id;

            var registros = servicioAuditoria.ConsultarAuditoria(
                tabla, idUsuario, dtpDesde.Value.Date, dtpHasta.Value.Date.AddDays(1).AddSeconds(-1));

            dgvAuditoria.DataSource = null;
            dgvAuditoria.DataSource = registros;

            if (dgvAuditoria.Columns["FK_Usuario"] != null)
                dgvAuditoria.Columns["FK_Usuario"].Visible = false;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
