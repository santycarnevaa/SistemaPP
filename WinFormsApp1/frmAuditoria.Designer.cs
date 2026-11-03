namespace CapaVista
{
    partial class frmAuditoria
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAuditoria));
            lblTabla = new Label();
            cmbTabla = new ComboBox();
            lblUsuario = new Label();
            cmbUsuario = new ComboBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnFiltrar = new Button();
            dgvAuditoria = new DataGridView();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            //
            // lblTabla
            //
            lblTabla.AutoSize = true;
            lblTabla.ForeColor = Color.White;
            lblTabla.Location = new Point(12, 15);
            lblTabla.Name = "lblTabla";
            lblTabla.Size = new Size(78, 15);
            lblTabla.TabIndex = 0;
            lblTabla.Text = "Tabla afectada";
            //
            // cmbTabla
            //
            cmbTabla.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTabla.Location = new Point(12, 33);
            cmbTabla.Name = "cmbTabla";
            cmbTabla.Size = new Size(190, 23);
            cmbTabla.TabIndex = 1;
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.ForeColor = Color.White;
            lblUsuario.Location = new Point(215, 15);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(47, 15);
            lblUsuario.TabIndex = 9;
            lblUsuario.Text = "Usuario";
            //
            // cmbUsuario
            //
            cmbUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUsuario.Location = new Point(215, 33);
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.Size = new Size(150, 23);
            cmbUsuario.TabIndex = 10;
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.ForeColor = Color.White;
            lblDesde.Location = new Point(380, 15);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.TabIndex = 2;
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(380, 33);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(130, 23);
            dtpDesde.TabIndex = 3;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.ForeColor = Color.White;
            lblHasta.Location = new Point(525, 15);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(34, 15);
            lblHasta.TabIndex = 4;
            lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(525, 33);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(130, 23);
            dtpHasta.TabIndex = 5;
            //
            // btnFiltrar
            //
            btnFiltrar.BackColor = Color.RoyalBlue;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(675, 31);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(120, 27);
            btnFiltrar.TabIndex = 6;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            //
            // dgvAuditoria
            //
            dgvAuditoria.Location = new Point(12, 75);
            dgvAuditoria.Name = "dgvAuditoria";
            dgvAuditoria.Size = new Size(960, 440);
            dgvAuditoria.TabIndex = 7;
            dgvAuditoria.ReadOnly = true;
            dgvAuditoria.AllowUserToAddRows = false;
            dgvAuditoria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditoria.MultiSelect = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(949, 6);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            //
            // frmAuditoria
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(984, 567);
            Controls.Add(lblTabla);
            Controls.Add(cmbTabla);
            Controls.Add(lblUsuario);
            Controls.Add(cmbUsuario);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(btnFiltrar);
            Controls.Add(dgvAuditoria);
            Controls.Add(pictureBox2);
            Name = "frmAuditoria";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auditoría de Cambios";
            Load += frmAuditoria_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTabla;
        private ComboBox cmbTabla;
        private Label lblUsuario;
        private ComboBox cmbUsuario;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnFiltrar;
        private DataGridView dgvAuditoria;
        private PictureBox pictureBox2;
    }
}
