using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CapaLogica;
using Entidades;

namespace CapaVista
{
    // pantalla de recuperar contraseña: le pide al usuario la cantidad de preguntas
    // que diga la politica (de las que ya respondio antes) y si acierta todas
    // lo deja pasar a cambiar la contraseña
    public partial class frmPreguntas : Form
    {
        private string usuario;
        private OrigenVerificacion origen;

        private CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();
        private CL_ServicioPreguntasSeguridad servicioPreguntas = new CL_ServicioPreguntasSeguridad();
        private CL_servicioContras servicioContras = new CL_servicioContras();

        private List<PreguntaSeguridad> preguntasUsuario = new List<PreguntaSeguridad>();

        public frmPreguntas(string usuario, OrigenVerificacion origen = OrigenVerificacion.RecuperarContrasena)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
            this.usuario = usuario;
            this.origen = origen;
        }

        private void frmPreguntas_Load(object sender, EventArgs e)
        {
            CargarPreguntasSegunConfiguracion();
            ingresar();
        }

        // trae las preguntas que ya respondio este usuario, se queda con la cantidad
        // que diga la politica (1, 3 o 5) y muestra esa cantidad de label y textbox
        // (hasta 5, ya armadas en el diseñador)
        private void CargarPreguntasSegunConfiguracion()
        {
            var config = servicioConfiguracion.ObtenerConfiguracion();

            if (config == null)
            {
                new MensajeError("No se pudo obtener la configuración del sistema.").Mostrar();
                BeginInvoke(new Action(Close));
                return;
            }

            int cantidadPreguntas = config.cantPreguntas;

            preguntasUsuario = servicioPreguntas.obtenerPreguntasUsuario(usuario);

            if (preguntasUsuario.Count < cantidadPreguntas)
            {
                // si no respondio ninguna pregunta no hay nada para validar: se avisa y se cierra
                // la pantalla en vez de dejarla vacia y sin salida
                new MensajeError("El usuario no tiene suficientes preguntas de seguridad registradas.").Mostrar();
                BeginInvoke(new Action(Close));
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

        // esconde y limpia las 5 filas de pregunta y respuesta antes de mostrar
        // solo las que hagan falta
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

        // junta lo que se escribio en las filas que se ven y lo manda a validar con los
        // hashes guardados. si esta todo bien sigue a cambiar la contraseña, si no
        // limpia los campos y avisa
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (preguntasUsuario.Count == 0)
            {
                new MensajeError("No hay preguntas cargadas para validar.").Mostrar();
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
                if (origen == OrigenVerificacion.RecuperarContrasena)
                {
                    // t10: al recuperar ya no se deja elegir la contraseña aca,
                    // se genera una temporal y se manda por mail
                    EnviarPasswordTemporalYVolver();
                }
                else
                {
                    frmCambiarContraseña frm = new frmCambiarContraseña(usuario);
                    this.Hide();
                    frm.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                txtPreg1.Text = "";
                txtPreg2.Text = "";
                txtPreg3.Text = "";
                txtPreg4.Text = "";
                txtPreg5.Text = "";

                // el mensaje va abajo de la ultima pregunta que se esta mostrando
                TextBox ultima = new[] { txtPreg5, txtPreg4, txtPreg3, txtPreg2, txtPreg1 }.First(t => t.Visible);
                ErrorCampo.Mostrar(ultima, "Una o más respuestas son incorrectas.");
            }
        }

        // habilita el boton ingresar solo cuando todas las preguntas que se ven
        // tienen una respuesta escrita
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
            frmLogin frm = new frmLogin();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void EnviarPasswordTemporalYVolver()
        {
            bool enviado = servicioContras.GenerarYEnviarPasswordTemporal(usuario);

            if (enviado)
                new MensajeExito("Se generó una nueva contraseña temporal y se envió a tu correo. Usala para iniciar sesión.").Mostrar();
            else
                new MensajeError("No se pudo generar la contraseña temporal. Intentá nuevamente más tarde.").Mostrar();

            this.Close();
        }
    }
}
