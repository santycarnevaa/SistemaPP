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
using Entidades;

namespace CapaVista
{
    public partial class frmPregUsuario : Form
    {
        private CL_ServicioPreguntasSeguridad servicioPreguntas = new CL_ServicioPreguntasSeguridad();
        private List<PreguntaSeguridad> preguntasSeleccionadas = new List<PreguntaSeguridad>();
        private List<TextBox> controlesRespuesta = new List<TextBox>();
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
            //Mover mensajes a capa logica
            int idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            if (idUsuario == -1)
            {
                MessageBox.Show("No se pudo identificar el usuario.");
                return;
            }

            for (int i = 0; i < controlesRespuesta.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(controlesRespuesta[i].Text))
                {
                    MessageBox.Show("Respondé todas las preguntas antes de continuar.");
                    return;
                }
            }

            Dictionary<int, string> respuestas = new Dictionary<int, string>();

            for (int i = 0; i < preguntasSeleccionadas.Count; i++)
                respuestas.Add(preguntasSeleccionadas[i].IdPregunta, controlesRespuesta[i].Text);

            bool ok = servicioPreguntas.registrarRespuestasUsuario(idUsuario, respuestas);

            if (!ok)
            {
                MessageBox.Show("No se pudieron guardar las respuestas.");
                return;
            }

            MessageBox.Show("Respuestas guardadas correctamente.");

            if (esPrimerLogin)
            {
                frmCambiarContraseña frm = new frmCambiarContraseña(usuario);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                AbrirPantallaPrincipal(usuario);
                this.Hide();
                this.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            AbrirPantallaPrincipal(usuario);
            this.Hide();
            this.Close();
        }

        private void CargarPreguntasAleatorias()
        {
            preguntasSeleccionadas = servicioPreguntas.ObtenerTodasLasPreguntasDisponibles();

            if (preguntasSeleccionadas.Count == 0)
            {
                MessageBox.Show("No hay preguntas de seguridad cargadas.");
                return;
            }

            pnlPreguntas.Controls.Clear();
            controlesRespuesta.Clear();

            const int altoFila = 96;
            int anchoDisponible = pnlPreguntas.ClientSize.Width - 20;

            for (int i = 0; i < preguntasSeleccionadas.Count; i++)
            {
                Label lblPregunta = new Label
                {
                    AutoSize = true,
                    MaximumSize = new Size(anchoDisponible, 0),
                    Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(0, i * altoFila),
                    Text = preguntasSeleccionadas[i].Pregunta
                };

                TextBox txtRespuesta = new TextBox
                {
                    BackColor = Color.FromArgb(8, 10, 13),
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                    ForeColor = Color.White,
                    Multiline = true,
                    Location = new Point(0, i * altoFila + 34),
                    Size = new Size(anchoDisponible, 32)
                };

                pnlPreguntas.Controls.Add(lblPregunta);
                pnlPreguntas.Controls.Add(txtRespuesta);
                controlesRespuesta.Add(txtRespuesta);
            }
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
