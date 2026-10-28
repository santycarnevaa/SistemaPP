namespace CapaVista
{
    partial class frmProductos
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
            dgvProductos = new DataGridView();
            lblCategoria = new Label();
            cmbCategoria = new ComboBox();
            lblMarca = new Label();
            cmbMarca = new ComboBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblPrecioCompra = new Label();
            txtPrecioCompra = new TextBox();
            lblPrecioVenta = new Label();
            txtPrecioVenta = new TextBox();
            lblStockCritico = new Label();
            txtStockCritico = new TextBox();
            lblStockMinimo = new Label();
            txtStockMinimo = new TextBox();
            lblStockIdeal = new Label();
            txtStockIdeal = new TextBox();
            lblStockMaximo = new Label();
            txtStockMaximo = new TextBox();
            lblPuntoReposicion = new Label();
            txtPuntoReposicion = new TextBox();
            lblNumeroLote = new Label();
            txtNumeroLote = new TextBox();
            chkTieneVencimiento = new CheckBox();
            dtpFechaVencimiento = new DateTimePicker();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnBaja = new Button();
            btnCancelar = new Button();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            //
            // dgvProductos
            //
            dgvProductos.Location = new Point(12, 12);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.Size = new Size(630, 540);
            dgvProductos.TabIndex = 0;
            dgvProductos.ReadOnly = true;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
            dgvProductos.RowEnter += dgvProductos_RowEnter;
            //
            // lblCategoria
            //
            lblCategoria.AutoSize = true;
            lblCategoria.ForeColor = Color.White;
            lblCategoria.Location = new Point(660, 15);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(70, 15);
            lblCategoria.TabIndex = 1;
            lblCategoria.Text = "Categoría";
            //
            // cmbCategoria
            //
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.Location = new Point(660, 33);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(300, 23);
            cmbCategoria.TabIndex = 2;
            //
            // lblMarca
            //
            lblMarca.AutoSize = true;
            lblMarca.ForeColor = Color.White;
            lblMarca.Location = new Point(660, 65);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(46, 15);
            lblMarca.TabIndex = 3;
            lblMarca.Text = "Marca";
            //
            // cmbMarca
            //
            cmbMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMarca.Location = new Point(660, 83);
            cmbMarca.Name = "cmbMarca";
            cmbMarca.Size = new Size(300, 23);
            cmbMarca.TabIndex = 4;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(660, 115);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(58, 15);
            lblNombre.TabIndex = 5;
            lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(660, 133);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(300, 23);
            txtNombre.TabIndex = 6;
            //
            // lblPrecioCompra
            //
            lblPrecioCompra.AutoSize = true;
            lblPrecioCompra.ForeColor = Color.White;
            lblPrecioCompra.Location = new Point(660, 165);
            lblPrecioCompra.Name = "lblPrecioCompra";
            lblPrecioCompra.Size = new Size(91, 15);
            lblPrecioCompra.TabIndex = 7;
            lblPrecioCompra.Text = "Precio de compra";
            //
            // txtPrecioCompra
            //
            txtPrecioCompra.Location = new Point(660, 183);
            txtPrecioCompra.Name = "txtPrecioCompra";
            txtPrecioCompra.Size = new Size(140, 23);
            txtPrecioCompra.TabIndex = 8;
            //
            // lblPrecioVenta
            //
            lblPrecioVenta.AutoSize = true;
            lblPrecioVenta.ForeColor = Color.White;
            lblPrecioVenta.Location = new Point(820, 165);
            lblPrecioVenta.Name = "lblPrecioVenta";
            lblPrecioVenta.Size = new Size(79, 15);
            lblPrecioVenta.TabIndex = 9;
            lblPrecioVenta.Text = "Precio de venta";
            //
            // txtPrecioVenta
            //
            txtPrecioVenta.Location = new Point(820, 183);
            txtPrecioVenta.Name = "txtPrecioVenta";
            txtPrecioVenta.Size = new Size(140, 23);
            txtPrecioVenta.TabIndex = 10;
            //
            // lblStockCritico
            //
            lblStockCritico.AutoSize = true;
            lblStockCritico.ForeColor = Color.White;
            lblStockCritico.Location = new Point(660, 215);
            lblStockCritico.Name = "lblStockCritico";
            lblStockCritico.Size = new Size(79, 15);
            lblStockCritico.TabIndex = 11;
            lblStockCritico.Text = "Stock crítico";
            //
            // txtStockCritico
            //
            txtStockCritico.Location = new Point(660, 233);
            txtStockCritico.Name = "txtStockCritico";
            txtStockCritico.Size = new Size(140, 23);
            txtStockCritico.TabIndex = 12;
            //
            // lblStockMinimo
            //
            lblStockMinimo.AutoSize = true;
            lblStockMinimo.ForeColor = Color.White;
            lblStockMinimo.Location = new Point(820, 215);
            lblStockMinimo.Name = "lblStockMinimo";
            lblStockMinimo.Size = new Size(82, 15);
            lblStockMinimo.TabIndex = 13;
            lblStockMinimo.Text = "Stock mínimo";
            //
            // txtStockMinimo
            //
            txtStockMinimo.Location = new Point(820, 233);
            txtStockMinimo.Name = "txtStockMinimo";
            txtStockMinimo.Size = new Size(140, 23);
            txtStockMinimo.TabIndex = 14;
            //
            // lblStockIdeal
            //
            lblStockIdeal.AutoSize = true;
            lblStockIdeal.ForeColor = Color.White;
            lblStockIdeal.Location = new Point(660, 265);
            lblStockIdeal.Name = "lblStockIdeal";
            lblStockIdeal.Size = new Size(70, 15);
            lblStockIdeal.TabIndex = 15;
            lblStockIdeal.Text = "Stock ideal";
            //
            // txtStockIdeal
            //
            txtStockIdeal.Location = new Point(660, 283);
            txtStockIdeal.Name = "txtStockIdeal";
            txtStockIdeal.Size = new Size(140, 23);
            txtStockIdeal.TabIndex = 16;
            //
            // lblStockMaximo
            //
            lblStockMaximo.AutoSize = true;
            lblStockMaximo.ForeColor = Color.White;
            lblStockMaximo.Location = new Point(820, 265);
            lblStockMaximo.Name = "lblStockMaximo";
            lblStockMaximo.Size = new Size(86, 15);
            lblStockMaximo.TabIndex = 17;
            lblStockMaximo.Text = "Stock máximo";
            //
            // txtStockMaximo
            //
            txtStockMaximo.Location = new Point(820, 283);
            txtStockMaximo.Name = "txtStockMaximo";
            txtStockMaximo.Size = new Size(140, 23);
            txtStockMaximo.TabIndex = 18;
            //
            // lblPuntoReposicion
            //
            lblPuntoReposicion.AutoSize = true;
            lblPuntoReposicion.ForeColor = Color.White;
            lblPuntoReposicion.Location = new Point(660, 315);
            lblPuntoReposicion.Name = "lblPuntoReposicion";
            lblPuntoReposicion.Size = new Size(97, 15);
            lblPuntoReposicion.TabIndex = 19;
            lblPuntoReposicion.Text = "Punto de reposición";
            //
            // txtPuntoReposicion
            //
            txtPuntoReposicion.Location = new Point(660, 333);
            txtPuntoReposicion.Name = "txtPuntoReposicion";
            txtPuntoReposicion.Size = new Size(140, 23);
            txtPuntoReposicion.TabIndex = 20;
            //
            // lblNumeroLote
            //
            lblNumeroLote.AutoSize = true;
            lblNumeroLote.ForeColor = Color.White;
            lblNumeroLote.Location = new Point(820, 315);
            lblNumeroLote.Name = "lblNumeroLote";
            lblNumeroLote.Size = new Size(85, 15);
            lblNumeroLote.TabIndex = 21;
            lblNumeroLote.Text = "Número de lote";
            //
            // txtNumeroLote
            //
            txtNumeroLote.Location = new Point(820, 333);
            txtNumeroLote.Name = "txtNumeroLote";
            txtNumeroLote.Size = new Size(140, 23);
            txtNumeroLote.TabIndex = 22;
            //
            // chkTieneVencimiento
            //
            chkTieneVencimiento.AutoSize = true;
            chkTieneVencimiento.ForeColor = Color.White;
            chkTieneVencimiento.Location = new Point(660, 365);
            chkTieneVencimiento.Name = "chkTieneVencimiento";
            chkTieneVencimiento.Size = new Size(127, 19);
            chkTieneVencimiento.TabIndex = 23;
            chkTieneVencimiento.Text = "Tiene vencimiento";
            chkTieneVencimiento.CheckedChanged += chkTieneVencimiento_CheckedChanged;
            //
            // dtpFechaVencimiento
            //
            dtpFechaVencimiento.Enabled = false;
            dtpFechaVencimiento.Format = DateTimePickerFormat.Short;
            dtpFechaVencimiento.Location = new Point(660, 390);
            dtpFechaVencimiento.Name = "dtpFechaVencimiento";
            dtpFechaVencimiento.Size = new Size(200, 23);
            dtpFechaVencimiento.TabIndex = 24;
            //
            // btnNuevo
            //
            btnNuevo.BackColor = Color.RoyalBlue;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(660, 480);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(140, 34);
            btnNuevo.TabIndex = 25;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            //
            // btnGuardar
            //
            btnGuardar.BackColor = Color.Green;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(820, 480);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(140, 34);
            btnGuardar.TabIndex = 26;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnBaja
            //
            btnBaja.BackColor = Color.Red;
            btnBaja.FlatStyle = FlatStyle.Flat;
            btnBaja.ForeColor = Color.White;
            btnBaja.Location = new Point(660, 520);
            btnBaja.Name = "btnBaja";
            btnBaja.Size = new Size(140, 34);
            btnBaja.TabIndex = 27;
            btnBaja.Text = "Dar de baja";
            btnBaja.UseVisualStyleBackColor = false;
            btnBaja.Click += btnBaja_Click;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(820, 520);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(140, 34);
            btnCancelar.TabIndex = 28;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // btnVolver
            //
            btnVolver.BackColor = Color.FromArgb(40, 40, 45);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(12, 560);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(140, 30);
            btnVolver.TabIndex = 29;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            //
            // frmProductos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(976, 605);
            Controls.Add(dgvProductos);
            Controls.Add(lblCategoria);
            Controls.Add(cmbCategoria);
            Controls.Add(lblMarca);
            Controls.Add(cmbMarca);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblPrecioCompra);
            Controls.Add(txtPrecioCompra);
            Controls.Add(lblPrecioVenta);
            Controls.Add(txtPrecioVenta);
            Controls.Add(lblStockCritico);
            Controls.Add(txtStockCritico);
            Controls.Add(lblStockMinimo);
            Controls.Add(txtStockMinimo);
            Controls.Add(lblStockIdeal);
            Controls.Add(txtStockIdeal);
            Controls.Add(lblStockMaximo);
            Controls.Add(txtStockMaximo);
            Controls.Add(lblPuntoReposicion);
            Controls.Add(txtPuntoReposicion);
            Controls.Add(lblNumeroLote);
            Controls.Add(txtNumeroLote);
            Controls.Add(chkTieneVencimiento);
            Controls.Add(dtpFechaVencimiento);
            Controls.Add(btnNuevo);
            Controls.Add(btnGuardar);
            Controls.Add(btnBaja);
            Controls.Add(btnCancelar);
            Controls.Add(btnVolver);
            Name = "frmProductos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Productos";
            Load += frmProductos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvProductos;
        private Label lblCategoria;
        private ComboBox cmbCategoria;
        private Label lblMarca;
        private ComboBox cmbMarca;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblPrecioCompra;
        private TextBox txtPrecioCompra;
        private Label lblPrecioVenta;
        private TextBox txtPrecioVenta;
        private Label lblStockCritico;
        private TextBox txtStockCritico;
        private Label lblStockMinimo;
        private TextBox txtStockMinimo;
        private Label lblStockIdeal;
        private TextBox txtStockIdeal;
        private Label lblStockMaximo;
        private TextBox txtStockMaximo;
        private Label lblPuntoReposicion;
        private TextBox txtPuntoReposicion;
        private Label lblNumeroLote;
        private TextBox txtNumeroLote;
        private CheckBox chkTieneVencimiento;
        private DateTimePicker dtpFechaVencimiento;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnBaja;
        private Button btnCancelar;
        private Button btnVolver;
    }
}
