namespace CapaVista
{
    partial class frmlogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmlogin));
            btnIniciar = new Button();
            txtUsuario = new TextBox();
            txtContraseña = new TextBox();
            lblRecuperar = new LinkLabel();
            label2 = new Label();
            pctOjo = new PictureBox();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pctOjo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btnIniciar
            // 
            btnIniciar.BackColor = Color.Green;
            btnIniciar.BackgroundImageLayout = ImageLayout.Center;
            btnIniciar.FlatStyle = FlatStyle.Flat;
            btnIniciar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnIniciar.ForeColor = Color.White;
            btnIniciar.Location = new Point(125, 165);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(149, 24);
            btnIniciar.TabIndex = 0;
            btnIniciar.Text = "INICIAR";
            btnIniciar.UseVisualStyleBackColor = false;
            btnIniciar.Click += btnIniciar_Click;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(8, 10, 13);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUsuario.ForeColor = SystemColors.Menu;
            txtUsuario.Location = new Point(94, 52);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(210, 21);
            txtUsuario.TabIndex = 1;
            txtUsuario.TextChanged += txtUsuario_TextChanged;
            // 
            // txtContraseña
            // 
            txtContraseña.BackColor = Color.FromArgb(8, 10, 13);
            txtContraseña.BorderStyle = BorderStyle.FixedSingle;
            txtContraseña.Cursor = Cursors.IBeam;
            txtContraseña.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtContraseña.ForeColor = SystemColors.Menu;
            txtContraseña.Location = new Point(94, 99);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.PasswordChar = '•';
            txtContraseña.Size = new Size(210, 21);
            txtContraseña.TabIndex = 2;
            txtContraseña.TextChanged += textBox2_TextChanged;
            // 
            // lblRecuperar
            // 
            lblRecuperar.ActiveLinkColor = Color.FromArgb(77, 15, 138);
            lblRecuperar.AutoSize = true;
            lblRecuperar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecuperar.LinkColor = Color.FromArgb(73, 98, 235);
            lblRecuperar.Location = new Point(150, 124);
            lblRecuperar.Name = "lblRecuperar";
            lblRecuperar.Size = new Size(173, 15);
            lblRecuperar.TabIndex = 3;
            lblRecuperar.TabStop = true;
            lblRecuperar.Text = "¿Olvidaste tu contraseña?";
            lblRecuperar.LinkClicked += lblRecuperar_LinkClicked;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(94, 35);
            label2.Name = "label2";
            label2.Size = new Size(61, 16);
            label2.TabIndex = 5;
            label2.Text = "Usuario";
            // 
            // pctOjo
            // 
            pctOjo.Image = (Image)resources.GetObject("pctOjo.Image");
            pctOjo.ImageLocation = "";
            pctOjo.Location = new Point(307, 99);
            pctOjo.Name = "pctOjo";
            pctOjo.Size = new Size(33, 23);
            pctOjo.SizeMode = PictureBoxSizeMode.Zoom;
            pctOjo.TabIndex = 6;
            pctOjo.TabStop = false;
            pctOjo.Click += pictureBox1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(94, 82);
            label1.Name = "label1";
            label1.Size = new Size(86, 16);
            label1.TabIndex = 7;
            label1.Text = "Contraseña";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(398, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 9;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // frmlogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(433, 230);
            Controls.Add(pictureBox2);
            Controls.Add(label1);
            Controls.Add(pctOjo);
            Controls.Add(label2);
            Controls.Add(lblRecuperar);
            Controls.Add(txtContraseña);
            Controls.Add(txtUsuario);
            Controls.Add(btnIniciar);
            ForeColor = SystemColors.ButtonShadow;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmlogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "login";
            Load += frmlogin_Load;
            ((System.ComponentModel.ISupportInitialize)pctOjo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnIniciar;
        private TextBox txtUsuario;
        private TextBox txtContraseña;
        private LinkLabel lblRecuperar;
        private Label label2;
        private PictureBox pctOjo;
        private Label label1;
        private PictureBox pictureBox2;
    }
}
