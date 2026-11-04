namespace CapaVista
{
    partial class frmModificarRol
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
            lblTitulo = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            grbPermisos = new GroupBox();
            lblProximamente = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            grbPermisos.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(131, 24);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Modificar rol";
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(12, 50);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(112, 16);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre del rol:";
            //
            // txtNombre
            //
            txtNombre.BackColor = Color.FromArgb(8, 10, 13);
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtNombre.ForeColor = Color.White;
            txtNombre.Location = new Point(12, 69);
            txtNombre.MaxLength = 50;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(336, 23);
            txtNombre.TabIndex = 2;
            //
            // grbPermisos
            //
            grbPermisos.Controls.Add(lblProximamente);
            grbPermisos.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbPermisos.ForeColor = Color.White;
            grbPermisos.Location = new Point(12, 108);
            grbPermisos.Name = "grbPermisos";
            grbPermisos.Size = new Size(336, 76);
            grbPermisos.TabIndex = 3;
            grbPermisos.TabStop = false;
            grbPermisos.Text = "Permisos";
            //
            // lblProximamente
            //
            lblProximamente.AutoSize = true;
            lblProximamente.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblProximamente.ForeColor = Color.Gray;
            lblProximamente.Location = new Point(12, 32);
            lblProximamente.Name = "lblProximamente";
            lblProximamente.Size = new Size(88, 16);
            lblProximamente.TabIndex = 0;
            lblProximamente.Text = "Próximamente";
            //
            // btnGuardar
            //
            btnGuardar.BackColor = Color.Green;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(12, 204);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(162, 28);
            btnGuardar.TabIndex = 4;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.BackColor = Color.FromArgb(8, 10, 13);
            btnCancelar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(186, 204);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(162, 28);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "CANCELAR";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            //
            // frmModificarRol
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(360, 248);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(grbPermisos);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(lblTitulo);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmModificarRol";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Modificar rol";
            grbPermisos.ResumeLayout(false);
            grbPermisos.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private GroupBox grbPermisos;
        private Label lblProximamente;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
