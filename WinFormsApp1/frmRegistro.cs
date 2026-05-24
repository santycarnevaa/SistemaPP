using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using CapaLogica;
namespace CapaVista
{
    public partial class frmRegistro : Form
    {
        CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
        List<persona> list = new List<persona>();
        public frmRegistro()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            int idRol = 0;
            string rolSeleccionado = cmbRol.SelectedItem?.ToString();
            switch(rolSeleccionado)
            {
                case "Administrador":
                    idRol = 2;
                    break;
                case "Usuario":
                    idRol = 1 ;
                    break;
                default:
                    MessageBox.Show("Seleccione un rol válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ltbClientes.SelectedIndex != -1)
            {
                list.RemoveAt(ltbClientes.SelectedIndex);
                datos();
                btnModificar.Visible = false;
                btnAceptar.Visible = true;
                Limpiar();
            }

        }
        private void datos()
        {
            ltbClientes.Items.Clear();
            foreach (persona pers in list)
            {
                string fila = $"{pers.name} {pers.apellido} {pers.dni}"
                ;
                ltbClientes.Items.Add(fila);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Close();
        }

        private void ltbClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ltbClientes.SelectedIndex >= 0)
            {
                btnModificar.Visible = true;
                btnAceptar.Visible = false;
                txtNombre.Text = list[ltbClientes.SelectedIndex].name;
                txtApellido.Text = list[ltbClientes.SelectedIndex].apellido;
                txtDni.Text = list[ltbClientes.SelectedIndex].dni;
                txtUsuario.Text = list[ltbClientes.SelectedIndex].usuario;
                txtTelefono.Text = list[ltbClientes.SelectedIndex].telefono;
                txtCorreo.Text = list[ltbClientes.SelectedIndex].correo;
                datFecha.Text = list[ltbClientes.SelectedIndex].fecha;
                txtCalle.Text = list[ltbClientes.SelectedIndex].calle;
                txtNumCalle.Text = list[ltbClientes.SelectedIndex].numero;
                txtCP.Text = list[ltbClientes.SelectedIndex].codPos;
                txtDepto.Text = list[ltbClientes.SelectedIndex].depto;
                txtPiso.Text = list[ltbClientes.SelectedIndex].piso;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            if (txtNombre.Text == "" || txtApellido.Text == "" || txtDni.Text == "" || txtCorreo.Text == "" || txtTelefono.Text == "" || txtUsuario.Text == "")
            {
                MessageBox.Show("Hay un campo vacio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
        }

        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}









