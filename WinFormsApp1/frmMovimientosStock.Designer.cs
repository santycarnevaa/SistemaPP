namespace CapaVista
{
    partial class frmMovimientosStock
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
            lblTipoMovimiento = new Label();
            cmbTipoMovimiento = new ComboBox();
            lblCantidad = new Label();
            txtCantidad = new TextBox();
            btnRegistrar = new Button();
            dgvKardex = new DataGridView();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
            btnFiltrar = new Button();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvKardex).BeginInit();
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
            cmbProducto.SelectedIndexChanged += cmbProducto_SelectedIndexChanged;
            //
            // lblTipoMovimiento
            //
            lblTipoMovimiento.AutoSize = true;
            lblTipoMovimiento.ForeColor = Color.White;
            lblTipoMovimiento.Location = new Point(330, 15);
            lblTipoMovimiento.Name = "lblTipoMovimiento";
            lblTipoMovimiento.Size = new Size(93, 15);
            lblTipoMovimiento.TabIndex = 2;
            lblTipoMovimiento.Text = "Tipo de movimiento";
            //
            // cmbTipoMovimiento
            //
            cmbTipoMovimiento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoMovimiento.Items.AddRange(new object[] { "Ingreso", "Egreso", "Ajuste" });
            cmbTipoMovimiento.Location = new Point(330, 33);
            cmbTipoMovimiento.Name = "cmbTipoMovimiento";
            cmbTipoMovimiento.Size = new Size(150, 23);
            cmbTipoMovimiento.TabIndex = 3;
            //
            // lblCantidad
            //
            lblCantidad.AutoSize = true;
            lblCantidad.ForeColor = Color.White;
            lblCantidad.Location = new Point(500, 15);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(55, 15);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "Cantidad";
            //
            // txtCantidad
            //
            txtCantidad.Location = new Point(500, 33);
            txtCantidad.Name = "txtCantidad";
            txtCantidad.Size = new Size(100, 23);
            txtCantidad.TabIndex = 5;
            //
            // btnRegistrar
            //
            btnRegistrar.BackColor = Color.Green;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Location = new Point(620, 31);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(140, 27);
            btnRegistrar.TabIndex = 6;
            btnRegistrar.Text = "Registrar";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // dgvKardex
            //
            dgvKardex.Location = new Point(12, 110);
            dgvKardex.Name = "dgvKardex";
            dgvKardex.Size = new Size(860, 400);
            dgvKardex.TabIndex = 7;
            dgvKardex.ReadOnly = true;
            dgvKardex.AllowUserToAddRows = false;
            dgvKardex.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKardex.MultiSelect = false;
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
            btnFiltrar.Text = "Filtrar Kardex";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            //
            // btnVolver
            //
            btnVolver.BackColor = Color.FromArgb(40, 40, 45);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(12, 520);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(140, 30);
            btnVolver.TabIndex = 13;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            //
            // frmMovimientosStock
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(884, 562);
            Controls.Add(lblProducto);
            Controls.Add(cmbProducto);
            Controls.Add(lblTipoMovimiento);
            Controls.Add(cmbTipoMovimiento);
            Controls.Add(lblCantidad);
            Controls.Add(txtCantidad);
            Controls.Add(btnRegistrar);
            Controls.Add(lblDesde);
            Controls.Add(dtpDesde);
            Controls.Add(lblHasta);
            Controls.Add(dtpHasta);
            Controls.Add(btnFiltrar);
            Controls.Add(dgvKardex);
            Controls.Add(btnVolver);
            Name = "frmMovimientosStock";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Movimientos de Stock";
            Load += frmMovimientosStock_Load;
            ((System.ComponentModel.ISupportInitialize)dgvKardex).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProducto;
        private ComboBox cmbProducto;
        private Label lblTipoMovimiento;
        private ComboBox cmbTipoMovimiento;
        private Label lblCantidad;
        private TextBox txtCantidad;
        private Button btnRegistrar;
        private DataGridView dgvKardex;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnFiltrar;
        private Button btnVolver;
    }
}
