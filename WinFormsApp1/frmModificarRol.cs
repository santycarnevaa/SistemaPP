using System;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmModificarRol : Form
    {
        private int idRol;
        private CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        public frmModificarRol(int idRol, string nombreActual)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
            this.idRol = idRol;
            txtNombre.Text = nombreActual;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = servicioUsuarios.ModificarRol(idRol, txtNombre.Text);

            if (mensaje != "")
            {
                ErrorCampo.Mostrar(txtNombre, mensaje);
                return;
            }

            new MensajeExito("Rol modificado correctamente.").Mostrar();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
