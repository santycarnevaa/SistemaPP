namespace CapaVista
{
    partial class frmContraseña
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmContraseña));
            btnSiguiente = new Button();
            txtNuevaContra = new TextBox();
            label1 = new Label();
            txtConfirmarContra = new TextBox();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btnSiguiente
            // 
            btnSiguiente.BackColor = Color.Green;
            btnSiguiente.Enabled = false;
            btnSiguiente.FlatStyle = FlatStyle.Flat;
            btnSiguiente.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            btnSiguiente.ForeColor = Color.White;
            btnSiguiente.Location = new Point(210, 192);
            btnSiguiente.Name = "btnSiguiente";
            btnSiguiente.Size = new Size(135, 34);
            btnSiguiente.TabIndex = 13;
            btnSiguiente.Text = "SIGUIENTE";
            btnSiguiente.UseVisualStyleBackColor = false;
            btnSiguiente.Click += btnSiguiente_Click;
            // 
            // txtNuevaContra
            // 
            txtNuevaContra.BackColor = Color.FromArgb(8, 10, 13);
            txtNuevaContra.BorderStyle = BorderStyle.FixedSingle;
            txtNuevaContra.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtNuevaContra.ForeColor = Color.White;
            txtNuevaContra.Location = new Point(166, 76);
            txtNuevaContra.Multiline = true;
            txtNuevaContra.Name = "txtNuevaContra";
            txtNuevaContra.Size = new Size(221, 33);
            txtNuevaContra.TabIndex = 14;
            txtNuevaContra.TextAlign = HorizontalAlignment.Center;
            txtNuevaContra.TextChanged += txtnueva_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(166, 52);
            label1.Name = "label1";
            label1.Size = new Size(130, 16);
            label1.TabIndex = 15;
            label1.Text = "Nueva contraseña:";
            // 
            // txtConfirmarContra
            // 
            txtConfirmarContra.BackColor = Color.FromArgb(8, 10, 13);
            txtConfirmarContra.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmarContra.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txtConfirmarContra.ForeColor = Color.White;
            txtConfirmarContra.Location = new Point(167, 141);
            txtConfirmarContra.Multiline = true;
            txtConfirmarContra.Name = "txtConfirmarContra";
            txtConfirmarContra.Size = new Size(221, 33);
            txtConfirmarContra.TabIndex = 16;
            txtConfirmarContra.TextAlign = HorizontalAlignment.Center;
            txtConfirmarContra.TextChanged += txtnueva2_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(167, 118);
            label2.Name = "label2";
            label2.Size = new Size(178, 16);
            label2.TabIndex = 17;
            label2.Text = "Repetir nueva contraseña:";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(521, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 18;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // frmContraseña
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(556, 286);
            Controls.Add(pictureBox2);
            Controls.Add(label2);
            Controls.Add(txtConfirmarContra);
            Controls.Add(label1);
            Controls.Add(txtNuevaContra);
            Controls.Add(btnSiguiente);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmContraseña";
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSiguiente;
        private TextBox txtNuevaContra;
        private Label label1;
        private TextBox txtConfirmarContra;
        private Label label2;
        private PictureBox pictureBox2;
    }
}