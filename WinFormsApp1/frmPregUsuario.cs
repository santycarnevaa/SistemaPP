using CapaLogica;
using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica;

namespace CapaVista
{
    public partial class frmPregUsuario : Form
    {
        private CL_ServicioPreguntasSeguridad servicioPreguntas = new CL_ServicioPreguntasSeguridad();
        private List<PreguntaSeguridad> preguntasSeleccionadas = new List<PreguntaSeguridad>();
        private CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
        private string usuario;
        private bool esPrimerLogin;
        public frmPregUsuario(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.Load += frmPregUsuario_Load;

            TextBox[] cajas = { txtRespuesta1, txtRespuesta2, txtRespuesta3, txtRespuesta4, txtRespuesta5 };
            foreach (TextBox caja in cajas)
                caja.KeyDown += Respuesta_KeyDown;
        }

        // Las cajas son multilinea: sin esto, Enter agrega un renglon y el texto "desaparece".
        // Enter pasa a la siguiente respuesta y en la ultima envia.
        private void Respuesta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;

            TextBox[] cajas = { txtRespuesta1, txtRespuesta2, txtRespuesta3, txtRespuesta4, txtRespuesta5 };
            int actual = Array.IndexOf(cajas, sender);

            for (int i = actual + 1; i < cajas.Length; i++)
            {
                if (cajas[i].Visible)
                {
                    cajas[i].Focus();
                    return;
                }
            }

            btnEnviar.PerformClick();
        }
        private void frmPregUsuario_Load(object sender, EventArgs e)
        {
            esPrimerLogin = servicioUsuarios.esPrimerLogin(usuario);
            CargarPreguntasAleatorias();
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            int idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            if (idUsuario == -1)
            {
                MessageBox.Show("No se pudo identificar el usuario.");
                return;
            }

            if (preguntasSeleccionadas.Count == 0)
            {
                MessageBox.Show("No hay preguntas de seguridad cargadas.");
                return;
            }

            TextBox[] cajas = { txtRespuesta1, txtRespuesta2, txtRespuesta3, txtRespuesta4, txtRespuesta5 };

            for (int i = 0; i < preguntasSeleccionadas.Count && i < cajas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(cajas[i].Text))
                {
                    MessageBox.Show("Debe responder todas las preguntas.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cajas[i].Focus();
                    return;
                }
            }

            Dictionary<int, string> respuestas = new Dictionary<int, string>();

            if (preguntasSeleccionadas.Count >= 1)
                respuestas.Add(preguntasSeleccionadas[0].IdPregunta, txtRespuesta1.Text);

            if (preguntasSeleccionadas.Count >= 2)
                respuestas.Add(preguntasSeleccionadas[1].IdPregunta, txtRespuesta2.Text);

            if (preguntasSeleccionadas.Count >= 3)
                respuestas.Add(preguntasSeleccionadas[2].IdPregunta, txtRespuesta3.Text);

            if (preguntasSeleccionadas.Count >= 4)
                respuestas.Add(preguntasSeleccionadas[3].IdPregunta, txtRespuesta4.Text);

            if (preguntasSeleccionadas.Count >= 5)
                respuestas.Add(preguntasSeleccionadas[4].IdPregunta, txtRespuesta5.Text);

            bool ok = servicioPreguntas.registrarRespuestasUsuario(idUsuario, respuestas);

            if (!ok)
            {
                MessageBox.Show("No se pudieron guardar las respuestas.");
                return;
            }

            MessageBox.Show("Respuestas guardadas correctamente.");

            if (esPrimerLogin)
            {
                MessageBox.Show("Ahora elegí tu nueva contraseña.", "Primer ingreso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                frmContraseña frm = new frmContraseña(usuario);
                this.Hide();

                if (frm.ShowDialog() == DialogResult.OK)
                    this.DialogResult = DialogResult.OK;
                else
                    this.DialogResult = DialogResult.Cancel;

                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void CargarPreguntasAleatorias()
        {
            preguntasSeleccionadas = servicioPreguntas.ObtenerPreguntasAleatoriasSegunConfiguracion();

            if (preguntasSeleccionadas.Count == 0)
            {
                MessageBox.Show("No hay preguntas de seguridad cargadas.");
                BeginInvoke(new Action(Close));
                return;
            }

            OcultarPreguntas();

            if (preguntasSeleccionadas.Count >= 1)
            {
                lblPregunta1.Visible = true;
                txtRespuesta1.Visible = true;
                lblPregunta1.Text = preguntasSeleccionadas[0].Pregunta;
            }

            if (preguntasSeleccionadas.Count >= 2)
            {
                lblPregunta2.Visible = true;
                txtRespuesta2.Visible = true;
                lblPregunta2.Text = preguntasSeleccionadas[1].Pregunta;
            }

            if (preguntasSeleccionadas.Count >= 3)
            {
                lblPregunta3.Visible = true;
                txtRespuesta3.Visible = true;
                lblPregunta3.Text = preguntasSeleccionadas[2].Pregunta;
            }

            if (preguntasSeleccionadas.Count >= 4)
            {
                lblPregunta4.Visible = true;
                txtRespuesta4.Visible = true;
                lblPregunta4.Text = preguntasSeleccionadas[3].Pregunta;
            }

            if (preguntasSeleccionadas.Count >= 5)
            {
                lblPregunta5.Visible = true;
                txtRespuesta5.Visible = true;
                lblPregunta5.Text = preguntasSeleccionadas[4].Pregunta;
            }
        }
        private void OcultarPreguntas()
        {
            lblPregunta1.Visible = false;
            lblPregunta2.Visible = false;
            lblPregunta3.Visible = false;
            lblPregunta4.Visible = false;
            lblPregunta5.Visible = false;

            txtRespuesta1.Visible = false;
            txtRespuesta2.Visible = false;
            txtRespuesta3.Visible = false;
            txtRespuesta4.Visible = false;
            txtRespuesta5.Visible = false;

            txtRespuesta1.Text = "";
            txtRespuesta2.Text = "";
            txtRespuesta3.Text = "";
            txtRespuesta4.Text = "";
            txtRespuesta5.Text = "";
        }
        private void AbrirPantallaPrincipal(string usuario)
        {
            CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

            int idRol = servicioUsuarios.ObtenerRolUsuario(usuario);

            if (idRol == 2)
            {
                frmAdmin frm = new frmAdmin(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else if (idRol == 1)
            {
                frmUsuario frm = new frmUsuario(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo identificar el rol del usuario.");
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
