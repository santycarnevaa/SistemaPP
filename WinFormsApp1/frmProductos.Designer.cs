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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductos));
            lblTitulo = new Label();
            btnNuevo = new Button();
            lblFiltroCategoria = new Label();
            cmbFiltroCategoria = new ComboBox();
            lblFiltroMarca = new Label();
            cmbFiltroMarca = new ComboBox();
            chkMostrarBaja = new CheckBox();
            dgvProductos = new DataGridView();
            lblRegistros = new Label();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(16, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(120, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Productos";
            //
            // btnNuevo
            //
            btnNuevo.BackColor = Color.RoyalBlue;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(724, 16);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(160, 34);
            btnNuevo.TabIndex = 1;
            btnNuevo.Text = "+ Nuevo producto";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            //
            // lblFiltroCategoria
            //
            lblFiltroCategoria.AutoSize = true;
            lblFiltroCategoria.ForeColor = Color.White;
            lblFiltroCategoria.Location = new Point(16, 73);
            lblFiltroCategoria.Name = "lblFiltroCategoria";
            lblFiltroCategoria.Size = new Size(61, 15);
            lblFiltroCategoria.TabIndex = 2;
            lblFiltroCategoria.Text = "Categoría:";
            //
            // cmbFiltroCategoria
            //
            cmbFiltroCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroCategoria.Location = new Point(85, 70);
            cmbFiltroCategoria.Name = "cmbFiltroCategoria";
            cmbFiltroCategoria.Size = new Size(210, 23);
            cmbFiltroCategoria.TabIndex = 3;
            //
            // lblFiltroMarca
            //
            lblFiltroMarca.AutoSize = true;
            lblFiltroMarca.ForeColor = Color.White;
            lblFiltroMarca.Location = new Point(315, 73);
            lblFiltroMarca.Name = "lblFiltroMarca";
            lblFiltroMarca.Size = new Size(43, 15);
            lblFiltroMarca.TabIndex = 4;
            lblFiltroMarca.Text = "Marca:";
            //
            // cmbFiltroMarca
            //
            cmbFiltroMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroMarca.Location = new Point(365, 70);
            cmbFiltroMarca.Name = "cmbFiltroMarca";
            cmbFiltroMarca.Size = new Size(210, 23);
            cmbFiltroMarca.TabIndex = 5;
            //
            // chkMostrarBaja
            //
            chkMostrarBaja.AutoSize = true;
            chkMostrarBaja.ForeColor = Color.White;
            chkMostrarBaja.Location = new Point(600, 72);
            chkMostrarBaja.Name = "chkMostrarBaja";
            chkMostrarBaja.Size = new Size(143, 19);
            chkMostrarBaja.TabIndex = 6;
            chkMostrarBaja.Text = "Mostrar dados de baja";
            //
            // dgvProductos
            //
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AllowUserToResizeColumns = false;
            dgvProductos.AllowUserToResizeRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.BackgroundColor = Color.White;
            dgvProductos.BorderStyle = BorderStyle.None;
            dgvProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProductos.ColumnHeadersHeight = 32;
            dgvProductos.Location = new Point(16, 110);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.RowTemplate.Height = 30;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(908, 410);
            dgvProductos.TabIndex = 7;
            dgvProductos.CellClick += dgvProductos_CellClick;
            dgvProductos.CellFormatting += dgvProductos_CellFormatting;
            dgvProductos.CellMouseMove += dgvProductos_CellMouseMove;
            //
            // lblRegistros
            //
            lblRegistros.AutoSize = true;
            lblRegistros.ForeColor = Color.White;
            lblRegistros.Location = new Point(16, 530);
            lblRegistros.Name = "lblRegistros";
            lblRegistros.Size = new Size(66, 15);
            lblRegistros.TabIndex = 8;
            lblRegistros.Text = "Registros: 0";
            //
            // pictureBox2
            //
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(901, 21);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 9;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            //
            // frmProductos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(940, 558);
            Controls.Add(lblTitulo);
            Controls.Add(btnNuevo);
            Controls.Add(lblFiltroCategoria);
            Controls.Add(cmbFiltroCategoria);
            Controls.Add(lblFiltroMarca);
            Controls.Add(cmbFiltroMarca);
            Controls.Add(chkMostrarBaja);
            Controls.Add(dgvProductos);
            Controls.Add(lblRegistros);
            Controls.Add(pictureBox2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmProductos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Productos";
            Load += frmProductos_Load;
            Shown += frmProductos_Shown;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Button btnNuevo;
        private Label lblFiltroCategoria;
        private ComboBox cmbFiltroCategoria;
        private Label lblFiltroMarca;
        private ComboBox cmbFiltroMarca;
        private CheckBox chkMostrarBaja;
        private DataGridView dgvProductos;
        private Label lblRegistros;
        private PictureBox pictureBox2;
    }
}
