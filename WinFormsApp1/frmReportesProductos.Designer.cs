namespace CapaVista
{
    partial class frmReportesProductos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportesProductos));
            lblReporte = new Label();
            cmbReporte = new ComboBox();
            lblDias = new Label();
            numDias = new NumericUpDown();
            btnGenerar = new Button();
            dgvReporte = new DataGridView();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)numDias).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            //
            // lblReporte
            //
            lblReporte.AutoSize = true;
            lblReporte.ForeColor = Color.White;
            lblReporte.Location = new Point(12, 15);
            lblReporte.Name = "lblReporte";
            lblReporte.Size = new Size(49, 15);
            lblReporte.TabIndex = 0;
            lblReporte.Text = "Reporte";
            //
            // cmbReporte
            //
            cmbReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbReporte.Items.AddRange(new object[] {
                "Productos bajo el punto de reposición",
                "Productos próximos a vencer",
                "Stock por categoría",
                "Stock por marca"
            });
            cmbReporte.Location = new Point(12, 33);
            cmbReporte.Name = "cmbReporte";
            cmbReporte.Size = new Size(320, 23);
            cmbReporte.TabIndex = 1;
            cmbReporte.SelectedIndexChanged += cmbReporte_SelectedIndexChanged;
            //
            // lblDias
            //
            lblDias.AutoSize = true;
            lblDias.ForeColor = Color.White;
            lblDias.Location = new Point(350, 15);
            lblDias.Name = "lblDias";
            lblDias.Size = new Size(135, 15);
            lblDias.TabIndex = 2;
            lblDias.Text = "Días de anticipación";
            //
            // numDias
            //
            numDias.Location = new Point(350, 33);
            numDias.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            numDias.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDias.Name = "numDias";
            numDias.Size = new Size(100, 23);
            numDias.TabIndex = 3;
            numDias.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numDias.Enabled = false;
            numDias.ValueChanged += numDias_ValueChanged;
            //
            // btnGenerar
            //
            btnGenerar.BackColor = Color.RoyalBlue;
            btnGenerar.FlatStyle = FlatStyle.Flat;
            btnGenerar.ForeColor = Color.White;
            btnGenerar.Location = new Point(470, 31);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(140, 27);
            btnGenerar.TabIndex = 4;
            btnGenerar.Text = "Generar";
            btnGenerar.UseVisualStyleBackColor = false;
            btnGenerar.Click += btnGenerar_Click;
            //
            // dgvReporte
            //
            dgvReporte.Location = new Point(12, 70);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.Size = new Size(760, 440);
            dgvReporte.TabIndex = 5;
            dgvReporte.ReadOnly = true;
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.MultiSelect = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(749, 6);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            //
            // frmReportesProductos
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(784, 562);
            Controls.Add(lblReporte);
            Controls.Add(cmbReporte);
            Controls.Add(lblDias);
            Controls.Add(numDias);
            Controls.Add(btnGenerar);
            Controls.Add(dgvReporte);
            Controls.Add(pictureBox2);
            Name = "frmReportesProductos";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reportes de Productos";
            Load += frmReportesProductos_Load;
            ((System.ComponentModel.ISupportInitialize)numDias).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblReporte;
        private ComboBox cmbReporte;
        private Label lblDias;
        private NumericUpDown numDias;
        private Button btnGenerar;
        private DataGridView dgvReporte;
        private PictureBox pictureBox2;
    }
}
