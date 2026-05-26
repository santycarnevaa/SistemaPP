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
            label2 = new Label();
            ckb2FA = new CheckBox();
            ckbDatosPersonales = new CheckBox();
            pictureBox2 = new PictureBox();
            btnGuardar = new Button();
            grbCantidadPreg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Century Gothic", 18F, FontStyle.Bold);
            lblApellido.ForeColor = Color.White;
            lblApellido.Location = new Point(7, 9);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(318, 28);
            lblApellido.TabIndex = 45;
            lblApellido.Text = "Seguridad de Contraseña:";
            // 
            // rb1
            // 
            rb1.AutoSize = true;
            rb1.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rb1.Location = new Point(24, 25);
            rb1.Name = "rb1";
            rb1.Size = new Size(32, 20);
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
            grbCantidadPreg.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbCantidadPreg.ForeColor = Color.White;
            grbCantidadPreg.Location = new Point(12, 203);
            grbCantidadPreg.Name = "grbCantidadPreg";
            grbCantidadPreg.Size = new Size(174, 60);
            grbCantidadPreg.TabIndex = 47;
            grbCantidadPreg.TabStop = false;
            grbCantidadPreg.Text = "Cantidad de Preguntas";
            // 
            // rb5
            // 
            rb5.AutoSize = true;
            rb5.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rb5.Location = new Point(116, 25);
            rb5.Name = "rb5";
            rb5.Size = new Size(32, 20);
            rb5.TabIndex = 48;
            rb5.Text = "5";
            rb5.UseVisualStyleBackColor = true;
            // 
            // rb3
            // 
            rb3.AutoSize = true;
            rb3.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rb3.Location = new Point(70, 25);
            rb3.Name = "rb3";
            rb3.Size = new Size(32, 20);
            rb3.TabIndex = 47;
            rb3.Text = "3";
            rb3.UseVisualStyleBackColor = true;
            // 
            // btnRegistros
            // 
            btnRegistros.BackColor = Color.FromArgb(8, 10, 13);
            btnRegistros.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
            txtCaracteres.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCaracteres.ForeColor = Color.White;
            txtCaracteres.Location = new Point(173, 47);
            txtCaracteres.MaxLength = 1;
            txtCaracteres.Multiline = true;
            txtCaracteres.Name = "txtCaracteres";
            txtCaracteres.Size = new Size(31, 23);
            txtCaracteres.TabIndex = 49;
            txtCaracteres.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 10F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(7, 49);
            label1.Name = "label1";
            label1.Size = new Size(164, 17);
            label1.TabIndex = 50;
            label1.Text = "Minimo de Caracteres:";
            // 
            // ckbNumeros
            // 
            ckbNumeros.AutoSize = true;
            ckbNumeros.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbNumeros.Location = new Point(7, 73);
            ckbNumeros.Name = "ckbNumeros";
            ckbNumeros.Size = new Size(145, 20);
            ckbNumeros.TabIndex = 51;
            ckbNumeros.Text = "Requiere Numeros";
            ckbNumeros.UseVisualStyleBackColor = true;
            // 
            // ckbCaracteresEsp
            // 
            ckbCaracteresEsp.AutoSize = true;
            ckbCaracteresEsp.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbCaracteresEsp.Location = new Point(7, 99);
            ckbCaracteresEsp.Name = "ckbCaracteresEsp";
            ckbCaracteresEsp.Size = new Size(231, 20);
            ckbCaracteresEsp.TabIndex = 52;
            ckbCaracteresEsp.Text = "Requiere caracteres Especiales";
            ckbCaracteresEsp.UseVisualStyleBackColor = true;
            // 
            // ckbRepetirContra
            // 
            ckbRepetirContra.AutoSize = true;
            ckbRepetirContra.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbRepetirContra.Location = new Point(7, 125);
            ckbRepetirContra.Name = "ckbRepetirContra";
            ckbRepetirContra.Size = new Size(206, 20);
            ckbRepetirContra.TabIndex = 53;
            ckbRepetirContra.Text = "Permite repetir Contraseñas";
            ckbRepetirContra.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 18F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(7, 167);
            label2.Name = "label2";
            label2.Size = new Size(111, 28);
            label2.TabIndex = 54;
            label2.Text = "Registro:";
            // 
            // ckb2FA
            // 
            ckb2FA.AutoSize = true;
            ckb2FA.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckb2FA.Location = new Point(12, 269);
            ckb2FA.Name = "ckb2FA";
            ckb2FA.Size = new Size(93, 20);
            ckb2FA.TabIndex = 55;
            ckb2FA.Text = "Factor 2FA";
            ckb2FA.UseVisualStyleBackColor = true;
            // 
            // ckbDatosPersonales
            // 
            ckbDatosPersonales.AutoSize = true;
            ckbDatosPersonales.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbDatosPersonales.Location = new Point(12, 295);
            ckbDatosPersonales.Name = "ckbDatosPersonales";
            ckbDatosPersonales.Size = new Size(179, 20);
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
            btnGuardar.Font = new Font("Century Gothic", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(96, 389);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(216, 24);
            btnGuardar.TabIndex = 83;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // frmConfigAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(409, 425);
            Controls.Add(btnGuardar);
            Controls.Add(pictureBox2);
            Controls.Add(ckbDatosPersonales);
            Controls.Add(ckb2FA);
            Controls.Add(label2);
            Controls.Add(ckbRepetirContra);
            Controls.Add(ckbCaracteresEsp);
            Controls.Add(ckbNumeros);
            Controls.Add(label1);
            Controls.Add(txtCaracteres);
            Controls.Add(btnRegistros);
            Controls.Add(grbCantidadPreg);
            Controls.Add(lblApellido);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmConfigAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = " ";
            Load += frmConfigAdmin_Load;
            grbCantidadPreg.ResumeLayout(false);
            grbCantidadPreg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
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
        private Label label2;
        private CheckBox ckb2FA;
        private CheckBox ckbDatosPersonales;
        private PictureBox pictureBox2;
        private Button btnGuardar;
    }
}