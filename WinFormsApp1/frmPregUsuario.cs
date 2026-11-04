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
    // Pantalla de primer login: el usuario responde las 15 preguntas de seguridad
    // completas para poder usarlas después en la recuperación de contraseña.
    public partial class frmPregUsuario : Form
    {
        private CL_ServicioPreguntasSeguridad servicioPreguntas = new CL_ServicioPreguntasSeguridad();
        private List<PreguntaSeguridad> preguntasSeleccionadas = new List<PreguntaSeguridad>();
        private List<TextBox> controlesRespuesta = new List<TextBox>();
        private CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();
        private string usuario;
        private bool esPrimerLogin;
        private bool salidaConfirmada;

        public frmPregUsuario(string usuario)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
            this.usuario = usuario;
            this.Load += frmPregUsuario_Load;
        }

        private void frmPregUsuario_Load(object sender, EventArgs e)
        {
            // Si es el primer login, después de responder las preguntas se manda al usuario
            // a cambiar la contraseña temporal; si no, directo a la pantalla principal.
            esPrimerLogin = servicioUsuarios.esPrimerLogin(usuario);
            CargarPreguntasAleatorias();
        }

        // Valida que estén todas las respuestas completas, las guarda, y sigue el flujo
        // según si esto pasó en el primer login o en otro momento.
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            //Mover mensajes a capa logica
            int idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            if (idUsuario == -1)
            {
                new MensajeError("No se pudo identificar el usuario.").Mostrar();
                return;
            }

            bool completas = true;

            for (int i = 0; i < controlesRespuesta.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(controlesRespuesta[i].Text))
                {
                    ErrorCampo.Mostrar(controlesRespuesta[i], "Respondé esta pregunta.");
                    completas = false;
                }
            }

            if (!completas)
                return;

            Dictionary<int, string> respuestas = new Dictionary<int, string>();

            for (int i = 0; i < preguntasSeleccionadas.Count; i++)
                respuestas.Add(preguntasSeleccionadas[i].IdPregunta, controlesRespuesta[i].Text);

            bool ok = servicioPreguntas.registrarRespuestasUsuario(idUsuario, respuestas);

            if (!ok)
            {
                new MensajeError("No se pudieron guardar las respuestas.").Mostrar();
                return;
            }

            new MensajeExito("Respuestas guardadas correctamente.").Mostrar();
            salidaConfirmada = true;

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
            if (!ConfirmarSalida())
                return;

            AbrirPantallaPrincipal(usuario);
            this.Hide();
            this.Close();
        }

        // Si se cierra con la X, tambien se pregunta antes de perder lo que se escribio
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !salidaConfirmada && !ConfirmarSalida())
                e.Cancel = true;

            base.OnFormClosing(e);
        }

        private bool ConfirmarSalida()
        {
            if (!new MensajeConfirmacion("¿Desea salir? Las respuestas que no guardó se van a perder.").Confirmado())
                return false;

            salidaConfirmada = true;
            return true;
        }

        // A pesar del nombre, ya no elige preguntas al azar: trae las 15 completas y arma
        // un par Label+TextBox por cada una, apiladas dentro de un panel con scroll.
        private void CargarPreguntasAleatorias()
        {
            preguntasSeleccionadas = servicioPreguntas.ObtenerTodasLasPreguntasDisponibles();

            if (preguntasSeleccionadas.Count == 0)
            {
                new MensajeError("No hay preguntas de seguridad cargadas.").Mostrar();
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

            EstiloSistema.AplicarControles(pnlPreguntas);
        }

        // Abre frmAdmin o frmUsuario según el rol, una vez terminado este paso.
        private void AbrirPantallaPrincipal(string usuario)
        {
            Navegacion.AbrirPantallaPrincipal(this, usuario);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
