namespace CapaVista
{
    public partial class frmRegistro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRegistro));
            btnAgregar = new Button();
            cmbRol = new ComboBox();
            lblRol = new Label();
            lblFecha = new Label();
            datFecha = new DateTimePicker();
            lblCorreo = new Label();
            txtCorreo = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            btnEliminar = new Button();
            label1 = new Label();
            txtTelefono = new TextBox();
            lblDni = new Label();
            txtDni = new TextBox();
            label2 = new Label();
            txtCalle = new TextBox();
            label3 = new Label();
            txtNumCalle = new TextBox();
            label4 = new Label();
            txtCP = new TextBox();
            label8 = new Label();
            txtDepto = new TextBox();
            label7 = new Label();
            txtPiso = new TextBox();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            cmbProvincia = new ComboBox();
            cmbLocalidad = new ComboBox();
            cmbPartido = new ComboBox();
            pictureBox2 = new PictureBox();
            DgvPrueba = new DataGridView();
            groupBox1 = new GroupBox();
            btnModificar = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DgvPrueba).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.Green;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            btnAgregar.Location = new Point(690, 199);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(146, 35);
            btnAgregar.TabIndex = 17;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += btnAceptar_Click;
            // 
            // cmbRol
            // 
            cmbRol.BackColor = Color.White;
            cmbRol.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            cmbRol.ForeColor = Color.Black;
            cmbRol.FormattingEnabled = true;
            cmbRol.Items.AddRange(new object[] { "Usuario", "Administrador" });
            cmbRol.Location = new Point(678, 109);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(141, 27);
            cmbRol.TabIndex = 15;
            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            lblRol.ForeColor = Color.White;
            lblRol.Location = new Point(731, 87);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(39, 19);
            lblRol.TabIndex = 48;
            lblRol.Text = "Rol*";
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            lblFecha.ForeColor = Color.White;
            lblFecha.Location = new Point(461, 147);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(185, 19);
            lblFecha.TabIndex = 47;
            lblFecha.Text = "Fecha de Nacimiento*";
            // 
            // datFecha
            // 
            datFecha.CalendarFont = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            datFecha.CalendarForeColor = Color.White;
            datFecha.CalendarMonthBackground = Color.FromArgb(8, 10, 13);
            datFecha.CalendarTitleBackColor = Color.FromArgb(64, 64, 64);
            datFecha.Font = new Font("Century Gothic", 10F, FontStyle.Bold);
            datFecha.Location = new Point(420, 169);
            datFecha.MaxDate = new DateTime(2100, 12, 31, 0, 0, 0, 0);
            datFecha.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            datFecha.Name = "datFecha";
            datFecha.Size = new Size(264, 24);
            datFecha.TabIndex = 16;
            // 
            // lblCorreo
            // 
            lblCorreo.AutoSize = true;
            lblCorreo.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            lblCorreo.ForeColor = Color.White;
            lblCorreo.Location = new Point(19, 88);
            lblCorreo.Name = "lblCorreo";
            lblCorreo.Size = new Size(68, 19);
            lblCorreo.TabIndex = 45;
            lblCorreo.Text = "Correo*";
            // 
            // txtCorreo
            // 
            txtCorreo.BackColor = Color.FromArgb(8, 10, 13);
            txtCorreo.BorderStyle = BorderStyle.FixedSingle;
            txtCorreo.Enabled = false;
            txtCorreo.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtCorreo.ForeColor = Color.White;
            txtCorreo.Location = new Point(19, 110);
            txtCorreo.Name = "txtCorreo";
            txtCorreo.Size = new Size(147, 27);
            txtCorreo.TabIndex = 6;
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            lblApellido.ForeColor = Color.White;
            lblApellido.Location = new Point(187, 28);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(82, 19);
            lblApellido.TabIndex = 43;
            lblApellido.Text = "Apellido*";
            // 
            // txtApellido
            // 
            txtApellido.BackColor = Color.FromArgb(8, 10, 13);
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Enabled = false;
            txtApellido.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtApellido.ForeColor = Color.White;
            txtApellido.Location = new Point(187, 52);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(147, 27);
            txtApellido.TabIndex = 2;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(19, 28);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(80, 19);
            lblNombre.TabIndex = 41;
            lblNombre.Text = "Nombre*";
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.FromArgb(8, 10, 13);
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Enabled = false;
            txtNombre.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtNombre.ForeColor = Color.White;
            txtNombre.Location = new Point(20, 52);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(147, 27);
            txtNombre.TabIndex = 1;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(678, 30);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(72, 19);
            lblUsuario.TabIndex = 39;
            lblUsuario.Text = "Usuario*";
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.FromArgb(8, 10, 13);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Enabled = false;
            txtUsuario.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtUsuario.ForeColor = Color.White;
            txtUsuario.Location = new Point(680, 52);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(147, 27);
            txtUsuario.TabIndex = 5;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.Red;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(690, 240);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(146, 25);
            btnEliminar.TabIndex = 20;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(513, 28);
            label1.Name = "label1";
            label1.Size = new Size(81, 19);
            label1.TabIndex = 57;
            label1.Text = "Telefono*";
            // 
            // txtTelefono
            // 
            txtTelefono.BackColor = Color.FromArgb(8, 10, 13);
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Enabled = false;
            txtTelefono.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtTelefono.ForeColor = Color.White;
            txtTelefono.Location = new Point(513, 52);
            txtTelefono.MaxLength = 10;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(147, 27);
            txtTelefono.TabIndex = 4;
            txtTelefono.KeyPress += txtTelefono_KeyPress;
            // 
            // lblDni
            // 
            lblDni.AutoSize = true;
            lblDni.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            lblDni.ForeColor = Color.White;
            lblDni.Location = new Point(345, 28);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(41, 19);
            lblDni.TabIndex = 55;
            lblDni.Text = "Dni*";
            // 
            // txtDni
            // 
            txtDni.BackColor = Color.FromArgb(8, 10, 13);
            txtDni.BorderStyle = BorderStyle.FixedSingle;
            txtDni.Enabled = false;
            txtDni.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtDni.ForeColor = Color.White;
            txtDni.Location = new Point(346, 52);
            txtDni.MaxLength = 10;
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(147, 27);
            txtDni.TabIndex = 3;
            txtDni.KeyPress += txtDni_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(185, 88);
            label2.Name = "label2";
            label2.Size = new Size(57, 19);
            label2.TabIndex = 59;
            label2.Text = "Calle*";
            // 
            // txtCalle
            // 
            txtCalle.BackColor = Color.FromArgb(8, 10, 13);
            txtCalle.BorderStyle = BorderStyle.FixedSingle;
            txtCalle.Enabled = false;
            txtCalle.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtCalle.ForeColor = Color.White;
            txtCalle.Location = new Point(195, 110);
            txtCalle.Name = "txtCalle";
            txtCalle.Size = new Size(147, 27);
            txtCalle.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(334, 88);
            label3.Name = "label3";
            label3.Size = new Size(79, 19);
            label3.TabIndex = 61;
            label3.Text = "Numero*";
            // 
            // txtNumCalle
            // 
            txtNumCalle.BackColor = Color.FromArgb(8, 10, 13);
            txtNumCalle.BorderStyle = BorderStyle.FixedSingle;
            txtNumCalle.Enabled = false;
            txtNumCalle.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtNumCalle.ForeColor = Color.White;
            txtNumCalle.Location = new Point(346, 110);
            txtNumCalle.MaxLength = 5;
            txtNumCalle.Name = "txtNumCalle";
            txtNumCalle.Size = new Size(67, 27);
            txtNumCalle.TabIndex = 8;
            txtNumCalle.KeyPress += txtNumCalle_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(420, 88);
            label4.Name = "label4";
            label4.Size = new Size(34, 19);
            label4.TabIndex = 63;
            label4.Text = "C.P";
            // 
            // txtCP
            // 
            txtCP.BackColor = Color.FromArgb(8, 10, 13);
            txtCP.BorderStyle = BorderStyle.FixedSingle;
            txtCP.Enabled = false;
            txtCP.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtCP.ForeColor = Color.White;
            txtCP.Location = new Point(422, 110);
            txtCP.MaxLength = 5;
            txtCP.Name = "txtCP";
            txtCP.Size = new Size(67, 27);
            txtCP.TabIndex = 9;
            txtCP.KeyPress += txtCP_KeyPress;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label8.ForeColor = Color.White;
            label8.Location = new Point(496, 88);
            label8.Name = "label8";
            label8.Size = new Size(55, 19);
            label8.TabIndex = 69;
            label8.Text = "Depto";
            // 
            // txtDepto
            // 
            txtDepto.BackColor = Color.FromArgb(8, 10, 13);
            txtDepto.BorderStyle = BorderStyle.FixedSingle;
            txtDepto.Enabled = false;
            txtDepto.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtDepto.ForeColor = Color.White;
            txtDepto.Location = new Point(496, 110);
            txtDepto.Name = "txtDepto";
            txtDepto.Size = new Size(67, 27);
            txtDepto.TabIndex = 10;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label7.ForeColor = Color.White;
            label7.Location = new Point(569, 88);
            label7.Name = "label7";
            label7.Size = new Size(38, 19);
            label7.TabIndex = 71;
            label7.Text = "Piso";
            // 
            // txtPiso
            // 
            txtPiso.BackColor = Color.FromArgb(8, 10, 13);
            txtPiso.BorderStyle = BorderStyle.FixedSingle;
            txtPiso.Enabled = false;
            txtPiso.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtPiso.ForeColor = Color.White;
            txtPiso.Location = new Point(571, 110);
            txtPiso.MaxLength = 2;
            txtPiso.Name = "txtPiso";
            txtPiso.Size = new Size(67, 27);
            txtPiso.TabIndex = 11;
            txtPiso.KeyPress += txtPiso_KeyPress;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label9.ForeColor = Color.White;
            label9.Location = new Point(26, 146);
            label9.Name = "label9";
            label9.Size = new Size(88, 19);
            label9.TabIndex = 73;
            label9.Text = "Provincia*";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label10.ForeColor = Color.White;
            label10.Location = new Point(281, 146);
            label10.Name = "label10";
            label10.Size = new Size(95, 19);
            label10.TabIndex = 75;
            label10.Text = "Localidad*";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            label11.ForeColor = Color.White;
            label11.Location = new Point(154, 146);
            label11.Name = "label11";
            label11.Size = new Size(70, 19);
            label11.TabIndex = 77;
            label11.Text = "Partido*";
            // 
            // cmbProvincia
            // 
            cmbProvincia.Location = new Point(27, 168);
            cmbProvincia.Name = "cmbProvincia";
            cmbProvincia.Size = new Size(121, 25);
            cmbProvincia.TabIndex = 12;
            // 
            // cmbLocalidad
            // 
            cmbLocalidad.Location = new Point(281, 168);
            cmbLocalidad.Name = "cmbLocalidad";
            cmbLocalidad.Size = new Size(121, 25);
            cmbLocalidad.TabIndex = 14;
            // 
            // cmbPartido
            // 
            cmbPartido.BackColor = Color.White;
            cmbPartido.Location = new Point(154, 168);
            cmbPartido.Name = "cmbPartido";
            cmbPartido.Size = new Size(121, 25);
            cmbPartido.TabIndex = 13;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(925, 6);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 81;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // DgvPrueba
            // 
            DgvPrueba.BackgroundColor = Color.FromArgb(8, 10, 13);
            DgvPrueba.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvPrueba.GridColor = SystemColors.WindowText;
            DgvPrueba.Location = new Point(23, 299);
            DgvPrueba.Name = "DgvPrueba";
            DgvPrueba.Size = new Size(853, 288);
            DgvPrueba.TabIndex = 87;
            DgvPrueba.CellContentClick += DgvPrueba_CellContentClick;
            DgvPrueba.RowEnter += DgvPrueba_RowEnter;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnModificar);
            groupBox1.Controls.Add(txtCalle);
            groupBox1.Controls.Add(txtNombre);
            groupBox1.Controls.Add(txtUsuario);
            groupBox1.Controls.Add(lblUsuario);
            groupBox1.Controls.Add(btnEliminar);
            groupBox1.Controls.Add(lblNombre);
            groupBox1.Controls.Add(txtApellido);
            groupBox1.Controls.Add(lblApellido);
            groupBox1.Controls.Add(cmbPartido);
            groupBox1.Controls.Add(txtCorreo);
            groupBox1.Controls.Add(cmbLocalidad);
            groupBox1.Controls.Add(lblCorreo);
            groupBox1.Controls.Add(cmbProvincia);
            groupBox1.Controls.Add(datFecha);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(lblFecha);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(lblRol);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(cmbRol);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(btnAgregar);
            groupBox1.Controls.Add(txtPiso);
            groupBox1.Controls.Add(txtDni);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(lblDni);
            groupBox1.Controls.Add(txtDepto);
            groupBox1.Controls.Add(txtTelefono);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtCP);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtNumCalle);
            groupBox1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.Gainsboro;
            groupBox1.Location = new Point(23, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(853, 276);
            groupBox1.TabIndex = 89;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos Personales";
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.Green;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            btnModificar.Location = new Point(690, 158);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(146, 35);
            btnModificar.TabIndex = 78;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModifica_Click;
            // 
            // frmRegistro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(960, 599);
            Controls.Add(DgvPrueba);
            Controls.Add(pictureBox2);
            Controls.Add(groupBox1);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmRegistro";
            Text = "Form1";
            Load += frmRegistro_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)DgvPrueba).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button btnAgregar;
        private ComboBox cmbRol;
        private Label lblRol;
        private Label lblFecha;
        private DateTimePicker datFecha;
        private Label lblCorreo;
        private TextBox txtCorreo;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Button btnEliminar;
        private Label label1;
        private TextBox txtTelefono;
        private Label lblDni;
        private TextBox txtDni;
        private Label label2;
        private TextBox txtCalle;
        private Label label3;
        private TextBox txtNumCalle;
        private Label label4;
        private TextBox txtCP;
        private Label label8;
        private TextBox txtDepto;
        private Label label7;
        private TextBox txtPiso;
        private Label label9;
        private Label label10;
        private Label label11;
        private ComboBox cmbProvincia;
        private ComboBox cmbLocalidad;
        private ComboBox cmbPartido;
        private PictureBox pictureBox2;
        private DataGridView DgvPrueba;
        private GroupBox groupBox1;
        private Button btnModificar;
    }
}