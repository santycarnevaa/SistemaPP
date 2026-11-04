namespace CapaVista
{
    partial class frmProductoDetalle
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
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            //
            // lblCategoria
            //
            lblCategoria.AutoSize = true;
            lblCategoria.ForeColor = Color.White;
            lblCategoria.Location = new Point(12, 15);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.TabIndex = 0;
            lblCategoria.Text = "Categoría";
            //
            // cmbCategoria
            //
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.Location = new Point(12, 33);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(300, 23);
            cmbCategoria.TabIndex = 1;
            //
            // lblMarca
            //
            lblMarca.AutoSize = true;
            lblMarca.ForeColor = Color.White;
            lblMarca.Location = new Point(12, 77);
            lblMarca.Name = "lblMarca";
            lblMarca.TabIndex = 2;
            lblMarca.Text = "Marca";
            //
            // cmbMarca
            //
            cmbMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMarca.Location = new Point(12, 95);
            cmbMarca.Name = "cmbMarca";
            cmbMarca.Size = new Size(300, 23);
            cmbMarca.TabIndex = 3;
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(12, 139);
            lblNombre.Name = "lblNombre";
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            txtNombre.Location = new Point(12, 157);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(300, 23);
            txtNombre.TabIndex = 5;
            //
            // lblPrecioCompra
            //
            lblPrecioCompra.AutoSize = true;
            lblPrecioCompra.ForeColor = Color.White;
            lblPrecioCompra.Location = new Point(12, 201);
            lblPrecioCompra.Name = "lblPrecioCompra";
            lblPrecioCompra.TabIndex = 6;
            lblPrecioCompra.Text = "Precio de compra";
            //
            // txtPrecioCompra
            //
            txtPrecioCompra.Location = new Point(12, 219);
            txtPrecioCompra.Name = "txtPrecioCompra";
            txtPrecioCompra.Size = new Size(140, 23);
            txtPrecioCompra.TabIndex = 7;
            //
            // lblPrecioVenta
            //
            lblPrecioVenta.AutoSize = true;
            lblPrecioVenta.ForeColor = Color.White;
            lblPrecioVenta.Location = new Point(172, 201);
            lblPrecioVenta.Name = "lblPrecioVenta";
            lblPrecioVenta.TabIndex = 8;
            lblPrecioVenta.Text = "Precio de venta";
            //
            // txtPrecioVenta
            //
            txtPrecioVenta.Location = new Point(172, 219);
            txtPrecioVenta.Name = "txtPrecioVenta";
            txtPrecioVenta.Size = new Size(140, 23);
            txtPrecioVenta.TabIndex = 9;
            //
            // lblStockCritico
            //
            lblStockCritico.AutoSize = true;
            lblStockCritico.ForeColor = Color.White;
            lblStockCritico.Location = new Point(12, 263);
            lblStockCritico.Name = "lblStockCritico";
            lblStockCritico.TabIndex = 10;
            lblStockCritico.Text = "Stock crítico";
            //
            // txtStockCritico
            //
            txtStockCritico.Location = new Point(12, 281);
            txtStockCritico.Name = "txtStockCritico";
            txtStockCritico.Size = new Size(140, 23);
            txtStockCritico.TabIndex = 11;
            //
            // lblStockMinimo
            //
            lblStockMinimo.AutoSize = true;
            lblStockMinimo.ForeColor = Color.White;
            lblStockMinimo.Location = new Point(172, 263);
            lblStockMinimo.Name = "lblStockMinimo";
            lblStockMinimo.TabIndex = 12;
            lblStockMinimo.Text = "Stock mínimo";
            //
            // txtStockMinimo
            //
            txtStockMinimo.Location = new Point(172, 281);
            txtStockMinimo.Name = "txtStockMinimo";
            txtStockMinimo.Size = new Size(140, 23);
            txtStockMinimo.TabIndex = 13;
            //
            // lblStockIdeal
            //
            lblStockIdeal.AutoSize = true;
            lblStockIdeal.ForeColor = Color.White;
            lblStockIdeal.Location = new Point(12, 325);
            lblStockIdeal.Name = "lblStockIdeal";
            lblStockIdeal.TabIndex = 14;
            lblStockIdeal.Text = "Stock ideal";
            //
            // txtStockIdeal
            //
            txtStockIdeal.Location = new Point(12, 343);
            txtStockIdeal.Name = "txtStockIdeal";
            txtStockIdeal.Size = new Size(140, 23);
            txtStockIdeal.TabIndex = 15;
            //
            // lblStockMaximo
            //
            lblStockMaximo.AutoSize = true;
            lblStockMaximo.ForeColor = Color.White;
            lblStockMaximo.Location = new Point(172, 325);
            lblStockMaximo.Name = "lblStockMaximo";
            lblStockMaximo.TabIndex = 16;
            lblStockMaximo.Text = "Stock máximo";
            //
            // txtStockMaximo
            //
            txtStockMaximo.Location = new Point(172, 343);
            txtStockMaximo.Name = "txtStockMaximo";
            txtStockMaximo.Size = new Size(140, 23);
            txtStockMaximo.TabIndex = 17;
            //
            // lblPuntoReposicion
            //
            lblPuntoReposicion.AutoSize = true;
            lblPuntoReposicion.ForeColor = Color.White;
            lblPuntoReposicion.Location = new Point(12, 387);
            lblPuntoReposicion.Name = "lblPuntoReposicion";
            lblPuntoReposicion.TabIndex = 18;
            lblPuntoReposicion.Text = "Punto de reposición";
            //
            // txtPuntoReposicion
            //
            txtPuntoReposicion.Location = new Point(12, 405);
            txtPuntoReposicion.Name = "txtPuntoReposicion";
            txtPuntoReposicion.Size = new Size(140, 23);
            txtPuntoReposicion.TabIndex = 19;
            //
            // lblNumeroLote
            //
            lblNumeroLote.AutoSize = true;
            lblNumeroLote.ForeColor = Color.White;
            lblNumeroLote.Location = new Point(172, 387);
            lblNumeroLote.Name = "lblNumeroLote";
            lblNumeroLote.TabIndex = 20;
            lblNumeroLote.Text = "Número de lote";
            //
            // txtNumeroLote
            //
            txtNumeroLote.Location = new Point(172, 405);
            txtNumeroLote.Name = "txtNumeroLote";
            txtNumeroLote.Size = new Size(140, 23);
            txtNumeroLote.TabIndex = 21;
            //
            // chkTieneVencimiento
            //
            chkTieneVencimiento.AutoSize = true;
            chkTieneVencimiento.ForeColor = Color.White;
            chkTieneVencimiento.Location = new Point(12, 449);
            chkTieneVencimiento.Name = "chkTieneVencimiento";
            chkTieneVencimiento.TabIndex = 22;
            chkTieneVencimiento.Text = "Tiene vencimiento";
            chkTieneVencimiento.CheckedChanged += chkTieneVencimiento_CheckedChanged;
            //
            // dtpFechaVencimiento
            //
            dtpFechaVencimiento.Enabled = false;
            dtpFechaVencimiento.Format = DateTimePickerFormat.Short;
            dtpFechaVencimiento.Location = new Point(12, 474);
            dtpFechaVencimiento.Name = "dtpFechaVencimiento";
            dtpFechaVencimiento.Size = new Size(200, 23);
            dtpFechaVencimiento.TabIndex = 23;
            //
            // btnGuardar
            //
            btnGuardar.BackColor = Color.Green;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(12, 528);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(140, 34);
            btnGuardar.TabIndex = 24;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.Gray;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(172, 528);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(140, 34);
            btnCancelar.TabIndex = 25;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // frmProductoDetalle
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(324, 586);
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
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmProductoDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Producto";
            Load += frmProductoDetalle_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
