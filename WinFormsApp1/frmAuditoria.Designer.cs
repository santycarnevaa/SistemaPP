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
            lblTabla = new Label();
            cmbTabla = new ComboBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnFiltrar = new Button();
            dgvAuditoria = new DataGridView();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).BeginInit();
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
            cmbTabla.Items.AddRange(new object[] { "(Todas)", "Productos", "Movimientos_Stock", "Registros_Scrap" });
            cmbTabla.Location = new Point(12, 33);
            cmbTabla.Name = "cmbTabla";
            cmbTabla.Size = new Size(220, 23);
            cmbTabla.TabIndex = 1;
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.ForeColor = Color.White;
            lblDesde.Location = new Point(250, 15);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.TabIndex = 2;
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(250, 33);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(160, 23);
            dtpDesde.TabIndex = 3;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.ForeColor = Color.White;
            lblHasta.Location = new Point(430, 15);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(34, 15);
            lblHasta.TabIndex = 4;
            lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(430, 33);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(160, 23);
            dtpHasta.TabIndex = 5;
            //
            // btnFiltrar
            //
            btnFiltrar.BackColor = Color.RoyalBlue;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(610, 31);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(140, 27);
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
            // btnVolver
            //
            btnVolver.BackColor = Color.FromArgb(40, 40, 45);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(12, 525);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(140, 30);
            btnVolver.TabIndex = 8;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            //
            // frmAuditoria
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(984, 567);
            Controls.Add(lblTabla);
            Controls.Add(cmbTabla);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(btnFiltrar);
            Controls.Add(dgvAuditoria);
            Controls.Add(btnVolver);
            Name = "frmAuditoria";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auditoría de Cambios";
            Load += frmAuditoria_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTabla;
        private ComboBox cmbTabla;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnFiltrar;
        private DataGridView dgvAuditoria;
        private Button btnVolver;
    }
}
