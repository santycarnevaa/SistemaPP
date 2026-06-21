namespace CapaVista
{
    partial class frmAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdmin));
            btnRegistros = new Button();
            btnPreg = new Button();
            lblBienvenido = new Label();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btnRegistros
            // 
            btnRegistros.BackColor = Color.FromArgb(8, 10, 13);
            btnRegistros.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegistros.ForeColor = Color.White;
            btnRegistros.Location = new Point(88, 170);
            btnRegistros.Name = "btnRegistros";
            btnRegistros.Size = new Size(258, 37);
            btnRegistros.TabIndex = 1;
            btnRegistros.Text = "Registros";
            btnRegistros.UseVisualStyleBackColor = false;
            btnRegistros.Click += btnRegistros_Click;
            // 
            // btnPreg
            // 
            btnPreg.BackColor = Color.FromArgb(8, 10, 13);
            btnPreg.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPreg.ForeColor = Color.White;
            btnPreg.Location = new Point(88, 127);
            btnPreg.Name = "btnPreg";
            btnPreg.Size = new Size(258, 37);
            btnPreg.TabIndex = 0;
            btnPreg.Text = "Configurar sistema";
            btnPreg.UseVisualStyleBackColor = false;
            btnPreg.Click += btnPreg_Click;
            // 
            // lblBienvenido
            // 
            lblBienvenido.AutoSize = true;
            lblBienvenido.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold);
            lblBienvenido.ForeColor = Color.White;
            lblBienvenido.Location = new Point(88, 20);
            lblBienvenido.Name = "lblBienvenido";
            lblBienvenido.Size = new Size(41, 31);
            lblBienvenido.TabIndex = 44;
            lblBienvenido.Text = "...";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(421, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 83;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // frmAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(456, 271);
            Controls.Add(pictureBox2);
            Controls.Add(lblBienvenido);
            Controls.Add(btnRegistros);
            Controls.Add(btnPreg);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "frmAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ConfigAdmin";
            Load += frmAdmin_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRegistros;
        private Button btnPreg;
        private Label lblBienvenido;
        private PictureBox pictureBox2;
    }
}