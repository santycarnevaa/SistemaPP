namespace CapaVista
{
    partial class frmPregUsuario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPregUsuario));
            pnlPreguntas = new Panel();
            label9 = new Label();
            btnEnviar = new Button();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pnlPreguntas
            // 
            pnlPreguntas.AutoScroll = true;
            pnlPreguntas.BackColor = Color.FromArgb(8, 10, 13);
            pnlPreguntas.Location = new Point(29, 46);
            pnlPreguntas.Name = "pnlPreguntas";
            pnlPreguntas.Size = new Size(504, 410);
            pnlPreguntas.TabIndex = 0;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold);
            label9.ForeColor = Color.White;
            label9.Location = new Point(102, 7);
            label9.Name = "label9";
            label9.Size = new Size(367, 31);
            label9.TabIndex = 17;
            label9.Text = "Personaliza tus respuestas";
            label9.Click += label9_Click;
            // 
            // btnEnviar
            // 
            btnEnviar.BackColor = Color.Green;
            btnEnviar.FlatStyle = FlatStyle.Flat;
            btnEnviar.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnEnviar.ForeColor = Color.White;
            btnEnviar.Location = new Point(204, 466);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(146, 35);
            btnEnviar.TabIndex = 51;
            btnEnviar.Text = "Enviar";
            btnEnviar.UseVisualStyleBackColor = false;
            btnEnviar.Click += btnEnviar_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(527, 7);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 83;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // frmPregUsuario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(562, 509);
            Controls.Add(pictureBox2);
            Controls.Add(btnEnviar);
            Controls.Add(label9);
            Controls.Add(pnlPreguntas);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "frmPregUsuario";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Configuracion";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlPreguntas;
        private Label label9;
        private Button btnEnviar;
        private PictureBox pictureBox2;
    }
}