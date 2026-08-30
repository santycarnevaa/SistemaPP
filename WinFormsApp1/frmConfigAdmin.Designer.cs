namespace CapaVista
{
    partial class frmConfigAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigAdmin));
            lblApellido = new Label();
            rb1 = new RadioButton();
            grbCantidadPreg = new GroupBox();
            rb5 = new RadioButton();
            rb3 = new RadioButton();
            btnRegistros = new Button();
            txtCaracteres = new TextBox();
            label1 = new Label();
            ckbNumeros = new CheckBox();
            ckbCaracteresEsp = new CheckBox();
            ckbRepetirContra = new CheckBox();
            ckbMayusculas = new CheckBox();
            ckbMinusculas = new CheckBox();
            label2 = new Label();
            ckb2FA = new CheckBox();
            ckbDatosPersonales = new CheckBox();
            pictureBox2 = new PictureBox();
            btnGuardar = new Button();
            button1 = new Button();
            lblCorreoRemitente = new Label();
            txtCorreoRemitente = new TextBox();
            lblPasswordNueva = new Label();
            txtPasswordNueva = new TextBox();
            lblDiasRotacion = new Label();
            numDiasRotacion = new NumericUpDown();
            lblUltimaRotacion = new Label();
            toolTipAyuda = new ToolTip(components);
            grbCantidadPreg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDiasRotacion).BeginInit();
            SuspendLayout();
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            lblApellido.ForeColor = Color.White;
            lblApellido.Location = new Point(7, 9);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(318, 29);
            lblApellido.TabIndex = 45;
            lblApellido.Text = "Seguridad de Contraseña:";
            // 
            // rb1
            // 
            rb1.AutoSize = true;
            rb1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rb1.Location = new Point(24, 25);
            rb1.Name = "rb1";
            rb1.Size = new Size(33, 20);
            rb1.TabIndex = 46;
            rb1.Text = "1";
            rb1.UseVisualStyleBackColor = true;
            // 
            // grbCantidadPreg
            // 
            grbCantidadPreg.BackColor = Color.FromArgb(8, 10, 13);
            grbCantidadPreg.Controls.Add(rb5);
            grbCantidadPreg.Controls.Add(rb3);
            grbCantidadPreg.Controls.Add(rb1);
            grbCantidadPreg.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbCantidadPreg.ForeColor = Color.White;
            grbCantidadPreg.Location = new Point(12, 199);
            grbCantidadPreg.Name = "grbCantidadPreg";
            grbCantidadPreg.Size = new Size(193, 60);
            grbCantidadPreg.TabIndex = 47;
            grbCantidadPreg.TabStop = false;
            grbCantidadPreg.Text = "Cantidad de Preguntas";
            // 
            // rb5
            // 
            rb5.AutoSize = true;
            rb5.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rb5.Location = new Point(116, 25);
            rb5.Name = "rb5";
            rb5.Size = new Size(33, 20);
            rb5.TabIndex = 48;
            rb5.Text = "5";
            rb5.UseVisualStyleBackColor = true;
            // 
            // rb3
            // 
            rb3.AutoSize = true;
            rb3.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rb3.Location = new Point(70, 25);
            rb3.Name = "rb3";
            rb3.Size = new Size(33, 20);
            rb3.TabIndex = 47;
            rb3.Text = "3";
            rb3.UseVisualStyleBackColor = true;
            // 
            // btnRegistros
            // 
            btnRegistros.BackColor = Color.FromArgb(8, 10, 13);
            btnRegistros.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegistros.ForeColor = Color.White;
            btnRegistros.Location = new Point(12, 330);
            btnRegistros.Name = "btnRegistros";
            btnRegistros.Size = new Size(193, 37);
            btnRegistros.TabIndex = 48;
            btnRegistros.Text = "Actualizar API";
            btnRegistros.UseVisualStyleBackColor = false;
            btnRegistros.Click += btnRegistros_Click;
            // 
            // txtCaracteres
            // 
            txtCaracteres.BackColor = Color.FromArgb(8, 10, 13);
            txtCaracteres.BorderStyle = BorderStyle.FixedSingle;
            txtCaracteres.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCaracteres.ForeColor = Color.White;
            txtCaracteres.Location = new Point(183, 47);
            txtCaracteres.MaxLength = 1;
            txtCaracteres.Multiline = true;
            txtCaracteres.Name = "txtCaracteres";
            txtCaracteres.Size = new Size(38, 23);
            txtCaracteres.TabIndex = 49;
            txtCaracteres.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(7, 49);
            label1.Name = "label1";
            label1.Size = new Size(170, 17);
            label1.TabIndex = 50;
            label1.Text = "Minimo de Caracteres:";
            // 
            // ckbNumeros
            // 
            ckbNumeros.AutoSize = true;
            ckbNumeros.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbNumeros.Location = new Point(7, 73);
            ckbNumeros.Name = "ckbNumeros";
            ckbNumeros.Size = new Size(156, 20);
            ckbNumeros.TabIndex = 51;
            ckbNumeros.Text = "Requiere Numeros";
            ckbNumeros.UseVisualStyleBackColor = true;
            // 
            // ckbCaracteresEsp
            // 
            ckbCaracteresEsp.AutoSize = true;
            ckbCaracteresEsp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbCaracteresEsp.Location = new Point(7, 99);
            ckbCaracteresEsp.Name = "ckbCaracteresEsp";
            ckbCaracteresEsp.Size = new Size(160, 20);
            ckbCaracteresEsp.TabIndex = 52;
            ckbCaracteresEsp.Text = "Requiere Especiales";
            ckbCaracteresEsp.UseVisualStyleBackColor = true;
            // 
            // ckbRepetirContra
            // 
            ckbRepetirContra.AutoSize = true;
            ckbRepetirContra.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbRepetirContra.Location = new Point(7, 125);
            ckbRepetirContra.Name = "ckbRepetirContra";
            ckbRepetirContra.Size = new Size(219, 20);
            ckbRepetirContra.TabIndex = 53;
            ckbRepetirContra.Text = "Permite repetir Contraseñas";
            ckbRepetirContra.UseVisualStyleBackColor = true;
            //
            // ckbMayusculas
            //
            ckbMayusculas.AutoSize = true;
            ckbMayusculas.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbMayusculas.Location = new Point(226, 225);
            ckbMayusculas.Name = "ckbMayusculas";
            ckbMayusculas.Size = new Size(179, 20);
            ckbMayusculas.TabIndex = 57;
            ckbMayusculas.Text = "Requiere Mayúsculas";
            ckbMayusculas.UseVisualStyleBackColor = true;
            //
            // ckbMinusculas
            //
            ckbMinusculas.AutoSize = true;
            ckbMinusculas.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbMinusculas.Location = new Point(226, 251);
            ckbMinusculas.Name = "ckbMinusculas";
            ckbMinusculas.Size = new Size(174, 20);
            ckbMinusculas.TabIndex = 58;
            ckbMinusculas.Text = "Requiere Minúsculas";
            ckbMinusculas.UseVisualStyleBackColor = true;
            //
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(7, 167);
            label2.Name = "label2";
            label2.Size = new Size(119, 29);
            label2.TabIndex = 54;
            label2.Text = "Registro:";
            // 
            // ckb2FA
            // 
            ckb2FA.AutoSize = true;
            ckb2FA.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckb2FA.Location = new Point(12, 269);
            ckb2FA.Name = "ckb2FA";
            ckb2FA.Size = new Size(101, 20);
            ckb2FA.TabIndex = 55;
            ckb2FA.Text = "Factor 2FA";
            ckb2FA.UseVisualStyleBackColor = true;
            // 
            // ckbDatosPersonales
            // 
            ckbDatosPersonales.AutoSize = true;
            ckbDatosPersonales.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbDatosPersonales.Location = new Point(12, 295);
            ckbDatosPersonales.Name = "ckbDatosPersonales";
            ckbDatosPersonales.Size = new Size(192, 20);
            ckbDatosPersonales.TabIndex = 56;
            ckbDatosPersonales.Text = "Omitir datos Personales";
            ckbDatosPersonales.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(374, 9);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 82;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.Green;
            btnGuardar.BackgroundImageLayout = ImageLayout.Center;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(96, 389);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(216, 24);
            btnGuardar.TabIndex = 83;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(8, 10, 13);
            button1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(226, 330);
            button1.Name = "button1";
            button1.Size = new Size(193, 37);
            button1.TabIndex = 84;
            button1.Text = "Actualizar credenciales";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            //
            // lblCorreoRemitente
            //
            lblCorreoRemitente.AutoSize = true;
            lblCorreoRemitente.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCorreoRemitente.ForeColor = Color.White;
            lblCorreoRemitente.Location = new Point(226, 40);
            lblCorreoRemitente.Name = "lblCorreoRemitente";
            lblCorreoRemitente.Size = new Size(133, 16);
            lblCorreoRemitente.TabIndex = 85;
            lblCorreoRemitente.Text = "Correo remitente:";
            //
            // txtCorreoRemitente
            //
            txtCorreoRemitente.BackColor = Color.FromArgb(8, 10, 13);
            txtCorreoRemitente.BorderStyle = BorderStyle.FixedSingle;
            txtCorreoRemitente.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCorreoRemitente.ForeColor = Color.White;
            txtCorreoRemitente.Location = new Point(226, 59);
            txtCorreoRemitente.Name = "txtCorreoRemitente";
            txtCorreoRemitente.Size = new Size(185, 23);
            txtCorreoRemitente.TabIndex = 86;
            //
            // lblPasswordNueva
            //
            lblPasswordNueva.AutoSize = true;
            lblPasswordNueva.Cursor = Cursors.Hand;
            lblPasswordNueva.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblPasswordNueva.ForeColor = Color.White;
            lblPasswordNueva.Location = new Point(226, 90);
            lblPasswordNueva.Name = "lblPasswordNueva";
            lblPasswordNueva.Size = new Size(191, 16);
            lblPasswordNueva.TabIndex = 87;
            lblPasswordNueva.Text = "Nueva contraseña de app:";
            toolTipAyuda.SetToolTip(lblPasswordNueva, "Clic para abrir la página de Google donde se genera la contraseña de aplicación (16 caracteres, sin espacios). Requiere tener la verificación en 2 pasos activada en la cuenta de Gmail.");
            lblPasswordNueva.Click += lblPasswordNueva_Click;
            //
            // txtPasswordNueva
            //
            txtPasswordNueva.BackColor = Color.FromArgb(8, 10, 13);
            txtPasswordNueva.BorderStyle = BorderStyle.FixedSingle;
            txtPasswordNueva.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPasswordNueva.ForeColor = Color.White;
            txtPasswordNueva.Location = new Point(226, 109);
            txtPasswordNueva.Name = "txtPasswordNueva";
            txtPasswordNueva.PasswordChar = '*';
            txtPasswordNueva.Size = new Size(185, 23);
            txtPasswordNueva.TabIndex = 88;
            toolTipAyuda.SetToolTip(txtPasswordNueva, "Contraseña de aplicación de Gmail (16 caracteres, sin espacios) - no es la contraseña normal de la cuenta. Se genera en myaccount.google.com/apppasswords, con la verificación en 2 pasos activada.");
            //
            // lblDiasRotacion
            //
            lblDiasRotacion.AutoSize = true;
            lblDiasRotacion.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiasRotacion.ForeColor = Color.White;
            lblDiasRotacion.Location = new Point(226, 140);
            lblDiasRotacion.Name = "lblDiasRotacion";
            lblDiasRotacion.Size = new Size(140, 16);
            lblDiasRotacion.TabIndex = 89;
            lblDiasRotacion.Text = "Días para rotación:";
            //
            // numDiasRotacion
            //
            numDiasRotacion.BackColor = Color.FromArgb(8, 10, 13);
            numDiasRotacion.ForeColor = Color.White;
            numDiasRotacion.Location = new Point(226, 159);
            numDiasRotacion.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numDiasRotacion.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDiasRotacion.Name = "numDiasRotacion";
            numDiasRotacion.Size = new Size(80, 23);
            numDiasRotacion.TabIndex = 90;
            numDiasRotacion.Value = new decimal(new int[] { 90, 0, 0, 0 });
            //
            // lblUltimaRotacion
            //
            lblUltimaRotacion.AutoSize = true;
            lblUltimaRotacion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblUltimaRotacion.ForeColor = Color.White;
            lblUltimaRotacion.Location = new Point(226, 195);
            lblUltimaRotacion.Name = "lblUltimaRotacion";
            lblUltimaRotacion.Size = new Size(150, 15);
            lblUltimaRotacion.TabIndex = 91;
            lblUltimaRotacion.Text = "Última rotación: -";
            //
            // frmConfigAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(431, 436);
            Controls.Add(lblUltimaRotacion);
            Controls.Add(numDiasRotacion);
            Controls.Add(lblDiasRotacion);
            Controls.Add(txtPasswordNueva);
            Controls.Add(lblPasswordNueva);
            Controls.Add(txtCorreoRemitente);
            Controls.Add(lblCorreoRemitente);
            Controls.Add(button1);
            Controls.Add(btnGuardar);
            Controls.Add(pictureBox2);
            Controls.Add(ckbDatosPersonales);
            Controls.Add(ckb2FA);
            Controls.Add(label2);
            Controls.Add(ckbMinusculas);
            Controls.Add(ckbMayusculas);
            Controls.Add(ckbRepetirContra);
            Controls.Add(ckbCaracteresEsp);
            Controls.Add(ckbNumeros);
            Controls.Add(label1);
            Controls.Add(txtCaracteres);
            Controls.Add(btnRegistros);
            Controls.Add(grbCantidadPreg);
            Controls.Add(lblApellido);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "frmConfigAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = " ";
            Load += frmConfigAdmin_Load;
            grbCantidadPreg.ResumeLayout(false);
            grbCantidadPreg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDiasRotacion).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblApellido;
        private RadioButton rb1;
        private GroupBox grbCantidadPreg;
        private RadioButton rb5;
        private RadioButton rb3;
        private Button btnRegistros;
        private TextBox txtCaracteres;
        private Label label1;
        private CheckBox ckbNumeros;
        private CheckBox ckbCaracteresEsp;
        private CheckBox ckbRepetirContra;
        private CheckBox ckbMayusculas;
        private CheckBox ckbMinusculas;
        private Label label2;
        private CheckBox ckb2FA;
        private CheckBox ckbDatosPersonales;
        private PictureBox pictureBox2;
        private Button btnGuardar;
        private Button button1;
        private Label lblCorreoRemitente;
        private TextBox txtCorreoRemitente;
        private Label lblPasswordNueva;
        private TextBox txtPasswordNueva;
        private Label lblDiasRotacion;
        private NumericUpDown numDiasRotacion;
        private Label lblUltimaRotacion;
        private ToolTip toolTipAyuda;
    }
}