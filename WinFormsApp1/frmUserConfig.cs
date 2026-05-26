using CapaLogica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class frmUserConfig : Form
    {
        private CL_ServicioConfiguracion servicioConfiguracion = new CL_ServicioConfiguracion();
        private string usuario;
        public frmUserConfig(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmPregUsuario frm = new frmPregUsuario(usuario);
            frm.ShowDialog();
            this.Close();
        }

        private void btnContraseña_Click(object sender, EventArgs e)
        {
            if (servicioConfiguracion.requiere2FA())
            {
                frmCodigo frmCod = new frmCodigo(usuario);
                this.Hide();
                frmCod.ShowDialog();
                this.Close();
            }
            else
            {
                frmrecuperar frmPreg = new frmrecuperar(usuario);
                this.Hide();
                frmPreg.ShowDialog();
                this.Close();
            }
        }
    }
}
