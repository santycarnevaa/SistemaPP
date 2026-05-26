using CapaLogica;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class frmrecuperar : Form
    {
        private string usuario;

        private CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();
        private CL_ServicioPreguntasSeguridad servicioPreguntas = new CL_ServicioPreguntasSeguridad();

        private List<PreguntaSeguridad> preguntasUsuario = new List<PreguntaSeguridad>();

        public frmrecuperar(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;

            this.Load += frmrecuperar_Load;
        }

        private void frmrecuperar_Load(object sender, EventArgs e)
        {
            CargarPreguntasSegunConfiguracion();
            ingresar();
        }

        private void CargarPreguntasSegunConfiguracion()
        {
            var config = servicioConfiguracion.ObtenerConfiguracion();

            if (config == null)
            {
                MessageBox.Show("No se pudo obtener la configuración del sistema.");
                return;
            }

            int cantidadPreguntas = config.cantPreguntas;

            preguntasUsuario = servicioPreguntas.obtenerPreguntasUsuario(usuario);

            if (preguntasUsuario.Count < cantidadPreguntas)
            {
                MessageBox.Show("El usuario no tiene suficientes preguntas de seguridad registradas.");
                return;
            }

            preguntasUsuario = preguntasUsuario.Take(cantidadPreguntas).ToList();

            OcultarTodasLasPreguntas();

            if (cantidadPreguntas >= 1)
            {
                lblPreg1.Visible = true;
                txtPreg1.Visible = true;
                lblPreg1.Text = preguntasUsuario[0].Pregunta;
            }

            if (cantidadPreguntas >= 2)
            {
                lblPreg2.Visible = true;
                txtPreg2.Visible = true;
                lblPreg2.Text = preguntasUsuario[1].Pregunta;
            }

            if (cantidadPreguntas >= 3)
            {
                lblPreg3.Visible = true;
                txtPreg3.Visible = true;
                lblPreg3.Text = preguntasUsuario[2].Pregunta;
            }

            if (cantidadPreguntas >= 4)
            {
                lblPreg4.Visible = true;
                txtPreg4.Visible = true;
                lblPreg4.Text = preguntasUsuario[3].Pregunta;
            }

            if (cantidadPreguntas >= 5)
            {
                lblPreg5.Visible = true;
                txtPreg5.Visible = true;
                lblPreg5.Text = preguntasUsuario[4].Pregunta;
            }
        }

        private void OcultarTodasLasPreguntas()
        {
            lblPreg1.Visible = false;
            lblPreg2.Visible = false;
            lblPreg3.Visible = false;
            lblPreg4.Visible = false;
            lblPreg5.Visible = false;

            txtPreg1.Visible = false;
            txtPreg2.Visible = false;
            txtPreg3.Visible = false;
            txtPreg4.Visible = false;
            txtPreg5.Visible = false;

            txtPreg1.Text = "";
            txtPreg2.Text = "";
            txtPreg3.Text = "";
            txtPreg4.Text = "";
            txtPreg5.Text = "";
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (preguntasUsuario.Count == 0)
            {
                MessageBox.Show("No hay preguntas cargadas para validar.");
                return;
            }

            List<string> respuestas = new List<string>();

            if (txtPreg1.Visible)
                respuestas.Add(txtPreg1.Text);

            if (txtPreg2.Visible)
                respuestas.Add(txtPreg2.Text);

            if (txtPreg3.Visible)
                respuestas.Add(txtPreg3.Text);

            if (txtPreg4.Visible)
                respuestas.Add(txtPreg4.Text);

            if (txtPreg5.Visible)
                respuestas.Add(txtPreg5.Text);

            bool respuestasCorrectas = servicioPreguntas.validarRespuestas(usuario, respuestas);

            if (respuestasCorrectas)
            {
                frmContraseña frm = new frmContraseña(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                txtPreg1.Text = "";
                txtPreg2.Text = "";
                txtPreg3.Text = "";
                txtPreg4.Text = "";
                txtPreg5.Text = "";

                MessageBox.Show(
                    "Una o más respuestas son incorrectas.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void ingresar()
        {
            bool habilitar = true;

            if (txtPreg1.Visible && string.IsNullOrWhiteSpace(txtPreg1.Text))
                habilitar = false;

            if (txtPreg2.Visible && string.IsNullOrWhiteSpace(txtPreg2.Text))
                habilitar = false;

            if (txtPreg3.Visible && string.IsNullOrWhiteSpace(txtPreg3.Text))
                habilitar = false;

            if (txtPreg4.Visible && string.IsNullOrWhiteSpace(txtPreg4.Text))
                habilitar = false;

            if (txtPreg5.Visible && string.IsNullOrWhiteSpace(txtPreg5.Text))
                habilitar = false;

            btnIngresar.Enabled = habilitar;
        }

        private void txtPreg1_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void txtPreg2_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void txtPreg3_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void txtPreg4_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void txtPreg5_TextChanged(object sender, EventArgs e)
        {
            ingresar();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmlogin frm = new frmlogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }
    }
}