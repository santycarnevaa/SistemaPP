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

            TextBox[] cajas = { txtPreg1, txtPreg2, txtPreg3, txtPreg4, txtPreg5 };
            foreach (TextBox caja in cajas)
                caja.KeyDown += Respuesta_KeyDown;
        }

        // Enter pasa a la siguiente respuesta y en la ultima intenta ingresar
        private void Respuesta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;

            TextBox[] cajas = { txtPreg1, txtPreg2, txtPreg3, txtPreg4, txtPreg5 };
            int actual = Array.IndexOf(cajas, sender);

            for (int i = actual + 1; i < cajas.Length; i++)
            {
                if (cajas[i].Visible)
                {
                    cajas[i].Focus();
                    return;
                }
            }

            if (btnIngresar.Enabled)
                btnIngresar.PerformClick();
        }

        private void frmrecuperar_Load(object sender, EventArgs e)
        {
            CargarPreguntasSegunConfiguracion();
            ingresar();
        }

        private void CargarPreguntasSegunConfiguracion()
        {
            var config = servicioConfiguracion.ObtenerConfiguracion();

            int cantidadPreguntas = config != null ? config.cantPreguntas : 3;

            try
            {
                preguntasUsuario = servicioPreguntas.obtenerPreguntasUsuario(usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron obtener las preguntas: " + ex.Message);
                preguntasUsuario = new List<PreguntaSeguridad>();
            }

            if (preguntasUsuario.Count == 0)
            {
                MessageBox.Show(
                    "Este usuario todavía no configuró sus preguntas de seguridad.\nDebe iniciar sesión con la contraseña que recibió por correo.",
                    "Recuperar cuenta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                BeginInvoke(new Action(Close));
                return;
            }

            // si el usuario respondio menos preguntas que las configuradas, se usan las que tiene
            if (cantidadPreguntas > preguntasUsuario.Count)
                cantidadPreguntas = preguntasUsuario.Count;

            if (cantidadPreguntas > 5)
                cantidadPreguntas = 5;

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
                this.DialogResult = frm.ShowDialog();
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
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}