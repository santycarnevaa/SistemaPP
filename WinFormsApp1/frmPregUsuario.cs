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
        }
        private void frmPregUsuario_Load(object sender, EventArgs e)
        {
            esPrimerLogin = servicioUsuarios.esPrimerLogin(usuario);
            CargarPreguntasAleatorias();
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> respuestas = new Dictionary<int, string>();

            respuestas.Add(preguntasSeleccionadas[0].IdPregunta, txtRespuesta1.Text);
            respuestas.Add(preguntasSeleccionadas[1].IdPregunta, txtRespuesta2.Text);
            respuestas.Add(preguntasSeleccionadas[2].IdPregunta, txtRespuesta3.Text);

            int idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);
            bool ok = servicioPreguntas.registrarRespuestasUsuario(idUsuario, respuestas);

            if (ok)
            {
                MessageBox.Show("Respuestas guardadas correctamente.");
                this.Hide();
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudieron guardar las respuestas.");
            }
            if (esPrimerLogin)
            {
                frmContraseña frm = new frmContraseña(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                frmUsuario frm = new frmUsuario(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmUsuario frm = new frmUsuario(usuario);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }
        private void CargarPreguntasAleatorias()
        {
            preguntasSeleccionadas = servicioPreguntas.ObtenerPreguntasAleatoriasSegunConfiguracion();

            if (preguntasSeleccionadas.Count < 3)
            {
                MessageBox.Show("No hay suficientes preguntas de seguridad cargadas.");
                return;
            }

            lblPregunta1.Text = preguntasSeleccionadas[0].Pregunta;
            lblPregunta2.Text = preguntasSeleccionadas[1].Pregunta;
            lblPregunta3.Text = preguntasSeleccionadas[2].Pregunta;
            lblPregunta4.Text = preguntasSeleccionadas[3].Pregunta;
            lblPregunta5.Text = preguntasSeleccionadas[4].Pregunta;
        }
    }
}
