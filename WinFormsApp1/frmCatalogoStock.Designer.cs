namespace CapaVista
{
    partial class frmCatalogoStock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCatalogoStock));
            btnProductos = new Button();
            btnMovimientos = new Button();
            btnScrap = new Button();
            btnAuditoria = new Button();
            btnReportes = new Button();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btnProductos
            // 
            btnProductos.BackColor = Color.FromArgb(8, 10, 13);
            btnProductos.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnProductos.ForeColor = Color.White;
            btnProductos.Location = new Point(27, 55);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(280, 42);
            btnProductos.TabIndex = 0;
            btnProductos.Text = "Productos";
            btnProductos.UseVisualStyleBackColor = false;
            btnProductos.Click += btnProductos_Click;
            // 
            // btnMovimientos
            // 
            btnMovimientos.BackColor = Color.FromArgb(8, 10, 13);
            btnMovimientos.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMovimientos.ForeColor = Color.White;
            btnMovimientos.Location = new Point(27, 110);
            btnMovimientos.Name = "btnMovimientos";
            btnMovimientos.Size = new Size(280, 42);
            btnMovimientos.TabIndex = 1;
            btnMovimientos.Text = "Movimientos de Stock";
            btnMovimientos.UseVisualStyleBackColor = false;
            btnMovimientos.Click += btnMovimientos_Click;
            // 
            // btnScrap
            // 
            btnScrap.BackColor = Color.FromArgb(8, 10, 13);
            btnScrap.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnScrap.ForeColor = Color.White;
            btnScrap.Location = new Point(27, 165);
            btnScrap.Name = "btnScrap";
            btnScrap.Size = new Size(280, 42);
            btnScrap.TabIndex = 2;
            btnScrap.Text = "Scrap";
            btnScrap.UseVisualStyleBackColor = false;
            btnScrap.Click += btnScrap_Click;
            // 
            // btnAuditoria
            // 
            btnAuditoria.BackColor = Color.FromArgb(8, 10, 13);
            btnAuditoria.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAuditoria.ForeColor = Color.White;
            btnAuditoria.Location = new Point(27, 220);
            btnAuditoria.Name = "btnAuditoria";
            btnAuditoria.Size = new Size(280, 42);
            btnAuditoria.TabIndex = 3;
            btnAuditoria.Text = "Auditoría de Cambios";
            btnAuditoria.UseVisualStyleBackColor = false;
            btnAuditoria.Click += btnAuditoria_Click;
            // 
            // btnReportes
            // 
            btnReportes.BackColor = Color.FromArgb(8, 10, 13);
            btnReportes.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReportes.ForeColor = Color.White;
            btnReportes.Location = new Point(27, 275);
            btnReportes.Name = "btnReportes";
            btnReportes.Size = new Size(280, 42);
            btnReportes.TabIndex = 4;
            btnReportes.Text = "Reportes";
            btnReportes.UseVisualStyleBackColor = false;
            btnReportes.Click += btnReportes_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(299, 6);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // frmCatalogoStock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(334, 345);
            Controls.Add(btnProductos);
            Controls.Add(btnMovimientos);
            Controls.Add(btnScrap);
            Controls.Add(btnAuditoria);
            Controls.Add(btnReportes);
            Controls.Add(pictureBox2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmCatalogoStock";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Catálogo, Stock y Auditoría";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnProductos;
        private Button btnMovimientos;
        private Button btnScrap;
        private Button btnAuditoria;
        private Button btnReportes;
        private PictureBox pictureBox2;
    }
}
