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
            lblTitulo = new Label();
            pictureBox2 = new PictureBox();
            tabConfig = new TabControl();
            tabRegistro = new TabPage();
            grbCantidadPreg = new GroupBox();
            rb5 = new RadioButton();
            rb3 = new RadioButton();
            rb1 = new RadioButton();
            ckb2FA = new CheckBox();
            lblApi = new Label();
            btnRegistros = new Button();
            tabSeguridad = new TabPage();
            label1 = new Label();
            cmbMinCaracteres = new ComboBox();
            ckbNumeros = new CheckBox();
            ckbCaracteresEsp = new CheckBox();
            ckbMinusculas = new CheckBox();
            ckbMayusculas = new CheckBox();
            ckbRepetirContra = new CheckBox();
            ckbDatosPersonales = new CheckBox();
            lblDiasRotacion = new Label();
            numDiasRotacion = new NumericUpDown();
            tabRoles = new TabPage();
            dgvRoles = new DataGridView();
            btnModificarRol = new Button();
            btnBajaRol = new Button();
            lblNuevoRol = new Label();
            txtNuevoRol = new TextBox();
            btnAgregarRol = new Button();
            tabCuenta = new TabPage();
            lblPasswordNueva = new Label();
            txtPasswordNueva = new TextBox();
            btnActualizarCuenta = new Button();
            lblUltimaRotacion = new Label();
            btnGuardar = new Button();
            toolTipAyuda = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            tabConfig.SuspendLayout();
            tabRegistro.SuspendLayout();
            grbCantidadPreg.SuspendLayout();
            tabSeguridad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDiasRotacion).BeginInit();
            tabRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            tabCuenta.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(168, 29);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Configuración";
            //
            // pictureBox2
            //
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(445, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            //
            // tabConfig
            //
            tabConfig.Controls.Add(tabRegistro);
            tabConfig.Controls.Add(tabSeguridad);
            tabConfig.Controls.Add(tabRoles);
            tabConfig.Controls.Add(tabCuenta);
            tabConfig.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabConfig.Location = new Point(12, 48);
            tabConfig.Name = "tabConfig";
            tabConfig.SelectedIndex = 0;
            tabConfig.Size = new Size(456, 360);
            tabConfig.TabIndex = 2;
            tabConfig.SelectedIndexChanged += tabConfig_SelectedIndexChanged;
            //
            // tabRegistro
            //
            tabRegistro.BackColor = Color.FromArgb(8, 10, 13);
            tabRegistro.Controls.Add(grbCantidadPreg);
            tabRegistro.Controls.Add(ckb2FA);
            tabRegistro.Controls.Add(lblApi);
            tabRegistro.Controls.Add(btnRegistros);
            tabRegistro.ForeColor = Color.White;
            tabRegistro.Location = new Point(4, 26);
            tabRegistro.Name = "tabRegistro";
            tabRegistro.Padding = new Padding(3);
            tabRegistro.Size = new Size(448, 330);
            tabRegistro.TabIndex = 0;
            tabRegistro.Text = "Registro";
            //
            // grbCantidadPreg
            //
            grbCantidadPreg.BackColor = Color.FromArgb(8, 10, 13);
            grbCantidadPreg.Controls.Add(rb5);
            grbCantidadPreg.Controls.Add(rb3);
            grbCantidadPreg.Controls.Add(rb1);
            grbCantidadPreg.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbCantidadPreg.ForeColor = Color.White;
            grbCantidadPreg.Location = new Point(12, 14);
            grbCantidadPreg.Name = "grbCantidadPreg";
            grbCantidadPreg.Size = new Size(200, 60);
            grbCantidadPreg.TabIndex = 0;
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
            rb5.TabIndex = 2;
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
            rb3.TabIndex = 1;
            rb3.Text = "3";
            rb3.UseVisualStyleBackColor = true;
            //
            // rb1
            //
            rb1.AutoSize = true;
            rb1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rb1.Location = new Point(24, 25);
            rb1.Name = "rb1";
            rb1.Size = new Size(33, 20);
            rb1.TabIndex = 0;
            rb1.Text = "1";
            rb1.UseVisualStyleBackColor = true;
            //
            // ckb2FA
            //
            ckb2FA.AutoSize = true;
            ckb2FA.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckb2FA.Location = new Point(12, 92);
            ckb2FA.Name = "ckb2FA";
            ckb2FA.Size = new Size(101, 20);
            ckb2FA.TabIndex = 1;
            ckb2FA.Text = "Factor 2FA";
            ckb2FA.UseVisualStyleBackColor = true;
            //
            // lblApi
            //
            lblApi.AutoSize = true;
            lblApi.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblApi.ForeColor = Color.White;
            lblApi.Location = new Point(12, 138);
            lblApi.Name = "lblApi";
            lblApi.Size = new Size(361, 16);
            lblApi.TabIndex = 2;
            lblApi.Text = "Datos geográficos (provincias, partidos y localidades):";
            //
            // btnRegistros
            //
            btnRegistros.BackColor = Color.FromArgb(8, 10, 13);
            btnRegistros.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegistros.ForeColor = Color.White;
            btnRegistros.Location = new Point(12, 160);
            btnRegistros.Name = "btnRegistros";
            btnRegistros.Size = new Size(193, 37);
            btnRegistros.TabIndex = 3;
            btnRegistros.Text = "Actualizar API";
            btnRegistros.UseVisualStyleBackColor = false;
            btnRegistros.Click += btnRegistros_Click;
            //
            // tabSeguridad
            //
            tabSeguridad.BackColor = Color.FromArgb(8, 10, 13);
            tabSeguridad.Controls.Add(label1);
            tabSeguridad.Controls.Add(cmbMinCaracteres);
            tabSeguridad.Controls.Add(ckbNumeros);
            tabSeguridad.Controls.Add(ckbCaracteresEsp);
            tabSeguridad.Controls.Add(ckbMinusculas);
            tabSeguridad.Controls.Add(ckbMayusculas);
            tabSeguridad.Controls.Add(ckbRepetirContra);
            tabSeguridad.Controls.Add(ckbDatosPersonales);
            tabSeguridad.Controls.Add(lblDiasRotacion);
            tabSeguridad.Controls.Add(numDiasRotacion);
            tabSeguridad.ForeColor = Color.White;
            tabSeguridad.Location = new Point(4, 26);
            tabSeguridad.Name = "tabSeguridad";
            tabSeguridad.Padding = new Padding(3);
            tabSeguridad.Size = new Size(448, 330);
            tabSeguridad.TabIndex = 1;
            tabSeguridad.Text = "Seguridad";
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(158, 16);
            label1.TabIndex = 0;
            label1.Text = "Mínimo de caracteres:";
            //
            // cmbMinCaracteres
            //
            cmbMinCaracteres.BackColor = Color.White;
            cmbMinCaracteres.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMinCaracteres.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbMinCaracteres.ForeColor = Color.Black;
            cmbMinCaracteres.FormattingEnabled = true;
            cmbMinCaracteres.Items.AddRange(new object[] { "5", "8", "15" });
            cmbMinCaracteres.Location = new Point(186, 16);
            cmbMinCaracteres.Name = "cmbMinCaracteres";
            cmbMinCaracteres.Size = new Size(70, 24);
            cmbMinCaracteres.TabIndex = 1;
            //
            // ckbNumeros
            //
            ckbNumeros.AutoSize = true;
            ckbNumeros.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbNumeros.Location = new Point(12, 56);
            ckbNumeros.Name = "ckbNumeros";
            ckbNumeros.Size = new Size(156, 20);
            ckbNumeros.TabIndex = 2;
            ckbNumeros.Text = "Requiere Números";
            ckbNumeros.UseVisualStyleBackColor = true;
            //
            // ckbCaracteresEsp
            //
            ckbCaracteresEsp.AutoSize = true;
            ckbCaracteresEsp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbCaracteresEsp.Location = new Point(12, 82);
            ckbCaracteresEsp.Name = "ckbCaracteresEsp";
            ckbCaracteresEsp.Size = new Size(160, 20);
            ckbCaracteresEsp.TabIndex = 3;
            ckbCaracteresEsp.Text = "Requiere Especiales";
            ckbCaracteresEsp.UseVisualStyleBackColor = true;
            //
            // ckbMinusculas
            //
            ckbMinusculas.AutoSize = true;
            ckbMinusculas.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbMinusculas.Location = new Point(12, 108);
            ckbMinusculas.Name = "ckbMinusculas";
            ckbMinusculas.Size = new Size(174, 20);
            ckbMinusculas.TabIndex = 4;
            ckbMinusculas.Text = "Requiere Minúsculas";
            ckbMinusculas.UseVisualStyleBackColor = true;
            //
            // ckbMayusculas
            //
            ckbMayusculas.AutoSize = true;
            ckbMayusculas.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbMayusculas.Location = new Point(12, 134);
            ckbMayusculas.Name = "ckbMayusculas";
            ckbMayusculas.Size = new Size(179, 20);
            ckbMayusculas.TabIndex = 5;
            ckbMayusculas.Text = "Requiere Mayúsculas";
            ckbMayusculas.UseVisualStyleBackColor = true;
            //
            // ckbRepetirContra
            //
            ckbRepetirContra.AutoSize = true;
            ckbRepetirContra.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbRepetirContra.Location = new Point(12, 160);
            ckbRepetirContra.Name = "ckbRepetirContra";
            ckbRepetirContra.Size = new Size(219, 20);
            ckbRepetirContra.TabIndex = 6;
            ckbRepetirContra.Text = "Permite repetir Contraseñas";
            ckbRepetirContra.UseVisualStyleBackColor = true;
            //
            // ckbDatosPersonales
            //
            ckbDatosPersonales.AutoSize = true;
            ckbDatosPersonales.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbDatosPersonales.Location = new Point(12, 186);
            ckbDatosPersonales.Name = "ckbDatosPersonales";
            ckbDatosPersonales.Size = new Size(192, 20);
            ckbDatosPersonales.TabIndex = 7;
            ckbDatosPersonales.Text = "Omitir datos Personales";
            ckbDatosPersonales.UseVisualStyleBackColor = true;
            //
            // lblDiasRotacion
            //
            lblDiasRotacion.AutoSize = true;
            lblDiasRotacion.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiasRotacion.ForeColor = Color.White;
            lblDiasRotacion.Location = new Point(12, 226);
            lblDiasRotacion.Name = "lblDiasRotacion";
            lblDiasRotacion.Size = new Size(333, 16);
            lblDiasRotacion.TabIndex = 8;
            lblDiasRotacion.Text = "Días de rotación (contraseña del correo del sistema):";
            //
            // numDiasRotacion
            //
            numDiasRotacion.BackColor = Color.FromArgb(8, 10, 13);
            numDiasRotacion.ForeColor = Color.White;
            numDiasRotacion.Location = new Point(12, 248);
            numDiasRotacion.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numDiasRotacion.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDiasRotacion.Name = "numDiasRotacion";
            numDiasRotacion.Size = new Size(80, 23);
            numDiasRotacion.TabIndex = 9;
            numDiasRotacion.Value = new decimal(new int[] { 90, 0, 0, 0 });
            //
            // tabRoles
            //
            tabRoles.BackColor = Color.FromArgb(8, 10, 13);
            tabRoles.Controls.Add(dgvRoles);
            tabRoles.Controls.Add(btnModificarRol);
            tabRoles.Controls.Add(btnBajaRol);
            tabRoles.Controls.Add(lblNuevoRol);
            tabRoles.Controls.Add(txtNuevoRol);
            tabRoles.Controls.Add(btnAgregarRol);
            tabRoles.ForeColor = Color.White;
            tabRoles.Location = new Point(4, 26);
            tabRoles.Name = "tabRoles";
            tabRoles.Padding = new Padding(3);
            tabRoles.Size = new Size(448, 330);
            tabRoles.TabIndex = 2;
            tabRoles.Text = "Roles";
            //
            // dgvRoles
            //
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.AllowUserToResizeRows = false;
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.BackgroundColor = Color.FromArgb(8, 10, 13);
            dgvRoles.Location = new Point(12, 12);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.RowHeadersVisible = false;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.Size = new Size(424, 150);
            dgvRoles.TabIndex = 0;
            dgvRoles.SelectionChanged += dgvRoles_SelectionChanged;
            //
            // btnModificarRol
            //
            btnModificarRol.BackColor = Color.FromArgb(8, 10, 13);
            btnModificarRol.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnModificarRol.ForeColor = Color.White;
            btnModificarRol.Location = new Point(12, 172);
            btnModificarRol.Name = "btnModificarRol";
            btnModificarRol.Size = new Size(130, 30);
            btnModificarRol.TabIndex = 1;
            btnModificarRol.Text = "Modificar";
            btnModificarRol.UseVisualStyleBackColor = false;
            btnModificarRol.Click += btnModificarRol_Click;
            //
            // btnBajaRol
            //
            btnBajaRol.BackColor = Color.FromArgb(8, 10, 13);
            btnBajaRol.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBajaRol.ForeColor = Color.White;
            btnBajaRol.Location = new Point(152, 172);
            btnBajaRol.Name = "btnBajaRol";
            btnBajaRol.Size = new Size(130, 30);
            btnBajaRol.TabIndex = 2;
            btnBajaRol.Text = "Dar de baja";
            btnBajaRol.UseVisualStyleBackColor = false;
            btnBajaRol.Click += btnBajaRol_Click;
            //
            // lblNuevoRol
            //
            lblNuevoRol.AutoSize = true;
            lblNuevoRol.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNuevoRol.ForeColor = Color.White;
            lblNuevoRol.Location = new Point(12, 226);
            lblNuevoRol.Name = "lblNuevoRol";
            lblNuevoRol.Size = new Size(112, 16);
            lblNuevoRol.TabIndex = 3;
            lblNuevoRol.Text = "Nombre del rol:";
            //
            // txtNuevoRol
            //
            txtNuevoRol.BackColor = Color.FromArgb(8, 10, 13);
            txtNuevoRol.BorderStyle = BorderStyle.FixedSingle;
            txtNuevoRol.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtNuevoRol.ForeColor = Color.White;
            txtNuevoRol.Location = new Point(12, 246);
            txtNuevoRol.MaxLength = 50;
            txtNuevoRol.Name = "txtNuevoRol";
            txtNuevoRol.Size = new Size(250, 23);
            txtNuevoRol.TabIndex = 4;
            //
            // btnAgregarRol
            //
            btnAgregarRol.BackColor = Color.FromArgb(8, 10, 13);
            btnAgregarRol.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAgregarRol.ForeColor = Color.White;
            btnAgregarRol.Location = new Point(272, 242);
            btnAgregarRol.Name = "btnAgregarRol";
            btnAgregarRol.Size = new Size(164, 30);
            btnAgregarRol.TabIndex = 5;
            btnAgregarRol.Text = "Agregar rol";
            btnAgregarRol.UseVisualStyleBackColor = false;
            btnAgregarRol.Click += btnAgregarRol_Click;
            //
            // tabCuenta
            //
            tabCuenta.BackColor = Color.FromArgb(8, 10, 13);
            tabCuenta.Controls.Add(lblPasswordNueva);
            tabCuenta.Controls.Add(txtPasswordNueva);
            tabCuenta.Controls.Add(btnActualizarCuenta);
            tabCuenta.Controls.Add(lblUltimaRotacion);
            tabCuenta.ForeColor = Color.White;
            tabCuenta.Location = new Point(4, 26);
            tabCuenta.Name = "tabCuenta";
            tabCuenta.Padding = new Padding(3);
            tabCuenta.Size = new Size(448, 330);
            tabCuenta.TabIndex = 3;
            tabCuenta.Text = "Cuenta";
            //
            // lblPasswordNueva
            //
            lblPasswordNueva.AutoSize = true;
            lblPasswordNueva.Cursor = Cursors.Hand;
            lblPasswordNueva.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblPasswordNueva.ForeColor = Color.White;
            lblPasswordNueva.Location = new Point(12, 20);
            lblPasswordNueva.Name = "lblPasswordNueva";
            lblPasswordNueva.Size = new Size(191, 16);
            lblPasswordNueva.TabIndex = 0;
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
            txtPasswordNueva.Location = new Point(12, 42);
            txtPasswordNueva.Name = "txtPasswordNueva";
            txtPasswordNueva.PasswordChar = '*';
            txtPasswordNueva.Size = new Size(250, 23);
            txtPasswordNueva.TabIndex = 1;
            toolTipAyuda.SetToolTip(txtPasswordNueva, "Contraseña de aplicación de Gmail (16 caracteres, sin espacios) - no es la contraseña normal de la cuenta. Se genera en myaccount.google.com/apppasswords, con la verificación en 2 pasos activada.");
            //
            // btnActualizarCuenta
            //
            btnActualizarCuenta.BackColor = Color.FromArgb(8, 10, 13);
            btnActualizarCuenta.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnActualizarCuenta.ForeColor = Color.White;
            btnActualizarCuenta.Location = new Point(272, 38);
            btnActualizarCuenta.Name = "btnActualizarCuenta";
            btnActualizarCuenta.Size = new Size(164, 30);
            btnActualizarCuenta.TabIndex = 2;
            btnActualizarCuenta.Text = "Actualizar contraseña";
            btnActualizarCuenta.UseVisualStyleBackColor = false;
            btnActualizarCuenta.Click += btnActualizarCuenta_Click;
            //
            // lblUltimaRotacion
            //
            lblUltimaRotacion.AutoSize = true;
            lblUltimaRotacion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblUltimaRotacion.ForeColor = Color.White;
            lblUltimaRotacion.Location = new Point(12, 86);
            lblUltimaRotacion.Name = "lblUltimaRotacion";
            lblUltimaRotacion.Size = new Size(150, 15);
            lblUltimaRotacion.TabIndex = 3;
            lblUltimaRotacion.Text = "Última rotación: -";
            //
            // btnGuardar
            //
            btnGuardar.BackColor = Color.Green;
            btnGuardar.BackgroundImageLayout = ImageLayout.Center;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(132, 424);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(216, 28);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            //
            // frmConfigAdmin
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(480, 470);
            Controls.Add(btnGuardar);
            Controls.Add(tabConfig);
            Controls.Add(pictureBox2);
            Controls.Add(lblTitulo);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmConfigAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = " ";
            Load += frmConfigAdmin_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            tabConfig.ResumeLayout(false);
            tabRegistro.ResumeLayout(false);
            tabRegistro.PerformLayout();
            grbCantidadPreg.ResumeLayout(false);
            grbCantidadPreg.PerformLayout();
            tabSeguridad.ResumeLayout(false);
            tabSeguridad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDiasRotacion).EndInit();
            tabRoles.ResumeLayout(false);
            tabRoles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            tabCuenta.ResumeLayout(false);
            tabCuenta.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private PictureBox pictureBox2;
        private TabControl tabConfig;
        private TabPage tabRegistro;
        private GroupBox grbCantidadPreg;
        private RadioButton rb5;
        private RadioButton rb3;
        private RadioButton rb1;
        private CheckBox ckb2FA;
        private Label lblApi;
        private Button btnRegistros;
        private TabPage tabSeguridad;
        private Label label1;
        private ComboBox cmbMinCaracteres;
        private CheckBox ckbNumeros;
        private CheckBox ckbCaracteresEsp;
        private CheckBox ckbMinusculas;
        private CheckBox ckbMayusculas;
        private CheckBox ckbRepetirContra;
        private CheckBox ckbDatosPersonales;
        private Label lblDiasRotacion;
        private NumericUpDown numDiasRotacion;
        private TabPage tabRoles;
        private DataGridView dgvRoles;
        private Button btnModificarRol;
        private Button btnBajaRol;
        private Label lblNuevoRol;
        private TextBox txtNuevoRol;
        private Button btnAgregarRol;
        private TabPage tabCuenta;
        private Label lblPasswordNueva;
        private TextBox txtPasswordNueva;
        private Button btnActualizarCuenta;
        private Label lblUltimaRotacion;
        private Button btnGuardar;
        private ToolTip toolTipAyuda;
    }
}
