namespace CapaVista
{
    partial class frmScrap
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
            lblProducto = new Label();
            cmbProducto = new ComboBox();
            lblCantidad = new Label();
            txtCantidad = new TextBox();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            btnRegistrar = new Button();
            dgvScrap = new DataGridView();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnFiltrar = new Button();
            lblTotalPeriodo = new Label();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvScrap).BeginInit();
            SuspendLayout();
            //
            // lblProducto
            //
            lblProducto.AutoSize = true;
            lblProducto.ForeColor = Color.White;
            lblProducto.Location = new Point(12, 15);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(58, 15);
            lblProducto.TabIndex = 0;
            lblProducto.Text = "Producto";
            //
            // cmbProducto
            //
            cmbProducto.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProducto.Location = new Point(12, 33);
            cmbProducto.Name = "cmbProducto";
            cmbProducto.Size = new Size(300, 23);
            cmbProducto.TabIndex = 1;
            //
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.ForeColor = Color.White;
            lblCantidad.Location = new Point(330, 15);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(55, 15);
            lblCantidad.TabIndex = 2;
            lblCantidad.Text = "Cantidad";
            //
            // txtCantidad
            //
            txtCantidad.Location = new Point(330, 33);
            txtCantidad.Name = "txtCantidad";
            txtCantidad.Size = new Size(100, 23);
            txtCantidad.TabIndex = 3;
            //
            // lblMotivo
            //
            lblMotivo.AutoSize = true;
            lblMotivo.ForeColor = Color.White;
            lblMotivo.Location = new Point(450, 15);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(45, 15);
            lblMotivo.TabIndex = 4;
            lblMotivo.Text = "Motivo";
            //
            // txtMotivo
            //
            txtMotivo.Location = new Point(450, 33);
            txtMotivo.Name = "txtMotivo";
            txtMotivo.Size = new Size(260, 23);
            txtMotivo.TabIndex = 5;
            //
            // btnRegistrar
            //
            btnRegistrar.BackColor = Color.Green;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Location = new Point(720, 31);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(150, 27);
            btnRegistrar.TabIndex = 6;
            btnRegistrar.Text = "Registrar Scrap";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // dgvScrap
            //
            dgvScrap.Location = new Point(12, 110);
            dgvScrap.Name = "dgvScrap";
            dgvScrap.Size = new Size(858, 380);
            dgvScrap.TabIndex = 7;
            dgvScrap.ReadOnly = true;
            dgvScrap.AllowUserToAddRows = false;
            dgvScrap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvScrap.MultiSelect = false;
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.ForeColor = Color.White;
            lblDesde.Location = new Point(12, 70);
            lblDesde.Name = "lblDesde";
            lblDesde.Size = new Size(39, 15);
            lblDesde.TabIndex = 8;
            lblDesde.Text = "Desde";
            //
            // dtpDesde
            //
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(12, 88);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(160, 23);
            dtpDesde.TabIndex = 9;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.ForeColor = Color.White;
            lblHasta.Location = new Point(190, 70);
            lblHasta.Name = "lblHasta";
            lblHasta.Size = new Size(34, 15);
            lblHasta.TabIndex = 10;
            lblHasta.Text = "Hasta";
            //
            // dtpHasta
            //
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(190, 88);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(160, 23);
            dtpHasta.TabIndex = 11;
            //
            // btnFiltrar
            //
            btnFiltrar.BackColor = Color.RoyalBlue;
            btnFiltrar.FlatStyle = FlatStyle.Flat;
            btnFiltrar.ForeColor = Color.White;
            btnFiltrar.Location = new Point(360, 86);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(140, 27);
            btnFiltrar.TabIndex = 12;
            btnFiltrar.Text = "Filtrar período";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            //
            // lblTotalPeriodo
            //
            lblTotalPeriodo.AutoSize = true;
            lblTotalPeriodo.ForeColor = Color.White;
            lblTotalPeriodo.Location = new Point(520, 92);
            lblTotalPeriodo.Name = "lblTotalPeriodo";
            lblTotalPeriodo.Size = new Size(170, 15);
            lblTotalPeriodo.TabIndex = 13;
            lblTotalPeriodo.Text = "Costo valorizado total: $0";
            //
            // btnVolver
            //
            btnVolver.BackColor = Color.FromArgb(40, 40, 45);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(12, 505);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(140, 30);
            btnVolver.TabIndex = 14;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            //
            // frmScrap
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(882, 547);
            Controls.Add(lblProducto);
            Controls.Add(cmbProducto);
            Controls.Add(lblCantidad);
            Controls.Add(txtCantidad);
            Controls.Add(lblMotivo);
            Controls.Add(txtMotivo);
            Controls.Add(btnRegistrar);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(btnFiltrar);
            Controls.Add(lblTotalPeriodo);
            Controls.Add(dgvScrap);
            Controls.Add(btnVolver);
            Name = "frmScrap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Registros de Scrap";
            Load += frmScrap_Load;
            ((System.ComponentModel.ISupportInitialize)dgvScrap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProducto;
        private ComboBox cmbProducto;
        private Label lblCantidad;
        private TextBox txtCantidad;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Button btnRegistrar;
        private DataGridView dgvScrap;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnFiltrar;
        private Label lblTotalPeriodo;
        private Button btnVolver;
    }
}
