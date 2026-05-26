namespace CapaVista
{
    partial class frmCodigo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCodigo));
            btnSiguiente = new Button();
            txt5 = new TextBox();
            txt4 = new TextBox();
            txt3 = new TextBox();
            txt2 = new TextBox();
            txt1 = new TextBox();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btnSiguiente
            // 
            btnSiguiente.BackColor = Color.Green;
            btnSiguiente.Enabled = false;
            btnSiguiente.FlatStyle = FlatStyle.Flat;
            btnSiguiente.Font = new Font("Century Gothic", 9F, FontStyle.Bold);
            btnSiguiente.Location = new Point(130, 118);
            btnSiguiente.Name = "btnSiguiente";
            btnSiguiente.Size = new Size(83, 28);
            btnSiguiente.TabIndex = 18;
            btnSiguiente.Text = "SIGUIENTE";
            btnSiguiente.UseVisualStyleBackColor = false;
            btnSiguiente.Click += btnSiguiente_Click;
            // 
            // txt5
            // 
            txt5.BackColor = Color.FromArgb(8, 10, 13);
            txt5.BorderStyle = BorderStyle.FixedSingle;
            txt5.Enabled = false;
            txt5.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txt5.ForeColor = Color.White;
            txt5.Location = new Point(257, 72);
            txt5.Margin = new Padding(4);
            txt5.MaxLength = 1;
            txt5.Multiline = true;
            txt5.Name = "txt5";
            txt5.Size = new Size(46, 33);
            txt5.TabIndex = 17;
            txt5.TextAlign = HorizontalAlignment.Center;
            txt5.TextChanged += txt5_TextChanged;
            txt5.KeyPress += txt5_KeyPress;
            // 
            // txt4
            // 
            txt4.BackColor = Color.FromArgb(8, 10, 13);
            txt4.BorderStyle = BorderStyle.FixedSingle;
            txt4.Enabled = false;
            txt4.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txt4.ForeColor = Color.White;
            txt4.Location = new Point(203, 72);
            txt4.Margin = new Padding(4);
            txt4.MaxLength = 1;
            txt4.Multiline = true;
            txt4.Name = "txt4";
            txt4.Size = new Size(46, 33);
            txt4.TabIndex = 16;
            txt4.TextAlign = HorizontalAlignment.Center;
            txt4.TextChanged += txt4_TextChanged;
            txt4.KeyPress += txt4_KeyPress;
            // 
            // txt3
            // 
            txt3.BackColor = Color.FromArgb(8, 10, 13);
            txt3.BorderStyle = BorderStyle.FixedSingle;
            txt3.Enabled = false;
            txt3.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txt3.ForeColor = Color.White;
            txt3.Location = new Point(149, 72);
            txt3.Margin = new Padding(4);
            txt3.MaxLength = 1;
            txt3.Multiline = true;
            txt3.Name = "txt3";
            txt3.Size = new Size(46, 33);
            txt3.TabIndex = 15;
            txt3.TextAlign = HorizontalAlignment.Center;
            txt3.TextChanged += txt3_TextChanged;
            txt3.KeyPress += txt3_KeyPress;
            // 
            // txt2
            // 
            txt2.BackColor = Color.FromArgb(8, 10, 13);
            txt2.BorderStyle = BorderStyle.FixedSingle;
            txt2.Enabled = false;
            txt2.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txt2.ForeColor = Color.White;
            txt2.Location = new Point(95, 72);
            txt2.Margin = new Padding(4);
            txt2.MaxLength = 1;
            txt2.Multiline = true;
            txt2.Name = "txt2";
            txt2.Size = new Size(46, 33);
            txt2.TabIndex = 14;
            txt2.TextAlign = HorizontalAlignment.Center;
            txt2.TextChanged += txt2_TextChanged;
            txt2.KeyPress += txt2_KeyPress;
            // 
            // txt1
            // 
            txt1.BackColor = Color.FromArgb(8, 10, 13);
            txt1.BorderStyle = BorderStyle.FixedSingle;
            txt1.Font = new Font("Century Gothic", 12F, FontStyle.Bold);
            txt1.ForeColor = Color.White;
            txt1.Location = new Point(41, 72);
            txt1.Margin = new Padding(4);
            txt1.MaxLength = 1;
            txt1.Multiline = true;
            txt1.Name = "txt1";
            txt1.Size = new Size(46, 33);
            txt1.TabIndex = 13;
            txt1.TextAlign = HorizontalAlignment.Center;
            txt1.TextChanged += txt1_TextChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new Point(313, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 19;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 9F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(81, 38);
            label1.Name = "label1";
            label1.Size = new Size(181, 16);
            label1.TabIndex = 51;
            label1.Text = "Ingrese el codigo de 5 dígitos";
            // 
            // frmCodigo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(348, 169);
            Controls.Add(label1);
            Controls.Add(pictureBox2);
            Controls.Add(btnSiguiente);
            Controls.Add(txt5);
            Controls.Add(txt4);
            Controls.Add(txt3);
            Controls.Add(txt2);
            Controls.Add(txt1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmCodigo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmCodigo";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSiguiente;
        private TextBox txt5;
        private TextBox txt4;
        private TextBox txt3;
        private TextBox txt2;
        private TextBox txt1;
        private PictureBox pictureBox2;
        private Label label1;
    }
}