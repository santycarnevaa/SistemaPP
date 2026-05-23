namespace WinFormsApp1
{
    public partial class frmlogin : Form
    {
        bool ingreso = false;
        public static string usuario = "asd";
        public  static string contraseña = "asd";
        public frmlogin()
        {
            InitializeComponent();
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblRecuperar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmrecuperar frm = new frmrecuperar();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
           if (usuario != txtUsuario.Text || contraseña != txtContraseña.Text)
            {
                MessageBox.Show("Algun campo esta incorrecto", "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           if (ingreso == true)
            {
                frmPregUsuario frm = new frmPregUsuario();
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
           else
            {
                frmUsuario frm = new frmUsuario();
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (txtContraseña.PasswordChar == '•')
            {
                txtContraseña.PasswordChar = '\0';
            }
            else if (txtContraseña.PasswordChar == '\0')
            {
                txtContraseña.PasswordChar = '•';
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

     
    }
}
