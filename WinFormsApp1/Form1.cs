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
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        //private HttpClient client = new HttpClient();

        // Función para cargar Provincias al inicio
        //private async Task CargarProvincias()
        //{
        //    try
        //    {
        //        var res = await client.GetFromJsonAsync<RespuestaApi>("https://apis.datos.gob.ar/georef/api/provincias?campos=id,nombre");
        //        cmbProvincia.DataSource = res.Provincias.OrderBy(x => x.Nombre).ToList();
        //        cmbProvincia.DisplayMember = "Nombre";
        //        cmbProvincia.ValueMember = "Id";
        //        cmbProvincia.SelectedIndex = -1; // Comienza vacío
        //    }
        //    catch (Exception ex) { MessageBox.Show("Error al cargar provincias: " + ex.Message); }
        //}

        //// Evento para cuando eligen una Provincia
        //private async void cmbProvincia_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbProvincia.SelectedValue != null && cmbProvincia.Focused)
        //    {
        //        string idProv = cmbProvincia.SelectedValue.ToString();
        //        string url = $"https://apis.datos.gob.ar/georef/api/departamentos?provincia={idProv}&campos=id,nombre&max=500";

        //        var res = await client.GetFromJsonAsync<RespuestaApi>(url);
        //        cmbPartido.DataSource = res.Departamentos.OrderBy(x => x.Nombre).ToList();
        //        cmbPartido.DisplayMember = "Nombre";
        //        cmbPartido.ValueMember = "Id";
        //        cmbLocalidad.DataSource = null;
        //    }
        //}

        //// Evento para cuando eligen un Partido
        //private async void cmbPartido_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbPartido.SelectedValue != null && cmbPartido.Focused)
        //    {
        //        string idPart = cmbPartido.SelectedValue.ToString();
        //        string url = $"https://apis.datos.gob.ar/georef/api/localidades?departamento={idPart}&campos=id,nombre&max=1000";

        //        var res = await client.GetFromJsonAsync<RespuestaApi>(url);
        //        cmbLocalidad.DataSource = res.Localidades.OrderBy(x => x.Nombre).ToList();
        //        cmbLocalidad.DisplayMember = "Nombre";
        //        cmbLocalidad.ValueMember = "Id";
        //    }
        //}


        List<persona> list = new List<persona>();
        public Form1()
        {
            InitializeComponent();
        }
        // list.RemoveAt(1);
        //list[0].name = "ffffffffffffffffffff";


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //ltbClientes.DataSource = list;
            if (txtNombre.Text == "" || txtApellido.Text == "" || txtDni.Text == "" || txtCorreo.Text == "" || txtTelefono.Text == "" || txtUsuario.Text == "")
            {
                MessageBox.Show("Hay un campo vacio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!VerifCorreo())
            {
                return;
            }
            list.Add(
                new persona()
                {
                    id = list.Count + 1,
                    name = txtNombre.Text,
                    apellido = txtApellido.Text,
                    correo = txtCorreo.Text,
                    dni = txtDni.Text,
                    telefono = txtTelefono.Text,
                    usuario = txtUsuario.Text,
                    calle = txtCalle.Text,
                    numero = txtNumCalle.Text,
                    codPos = txtCP.Text,
                    depto = txtDepto.Text,
                    piso = txtPiso.Text,
                    fecha = datFecha.Text,
                });
            datos();
            Limpiar();

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

        //private async void Form1_Load(object sender, EventArgs e)
        //{
        //    // await CargarProvincias();
        //}

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
            if (!VerifCorreo())
            {
                return;
            }
            btnAceptar.Visible = true;
            btnModificar.Visible = false;
            list[ltbClientes.SelectedIndex].name = txtNombre.Text;
            list[ltbClientes.SelectedIndex].apellido = txtApellido.Text;
            list[ltbClientes.SelectedIndex].dni = txtDni.Text;
            list[ltbClientes.SelectedIndex].usuario = txtUsuario.Text;
            list[ltbClientes.SelectedIndex].telefono = txtTelefono.Text;
            list[ltbClientes.SelectedIndex].correo = txtCorreo.Text;
            list[ltbClientes.SelectedIndex].fecha = datFecha.Text;
            list[ltbClientes.SelectedIndex].calle = txtCalle.Text;
            list[ltbClientes.SelectedIndex].numero = txtNumCalle.Text;
            list[ltbClientes.SelectedIndex].codPos = txtCP.Text;
            list[ltbClientes.SelectedIndex].depto = txtDepto.Text;
            list[ltbClientes.SelectedIndex].piso = txtPiso.Text;
            Limpiar();
            datos();

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

        private bool VerifCorreo()
        {
            if (string.IsNullOrWhiteSpace(txtCorreo.Text)) return false;
            string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtCorreo.Text, patronEmail))
            {
                MessageBox.Show("El formato del correo electrónico no es válido (ejemplo: usuario@gmail.com).", "Correo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
                return false;
            }
            return true;
        }

    }
}









