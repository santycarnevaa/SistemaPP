namespace CapaVista
{
    partial class frmUserConfig
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
            btnPreg = new Button();
            btnConfig = new Button();
            btnContraseña = new Button();
            SuspendLayout();
            // 
            // btnPreg
            // 
            btnPreg.BackColor = Color.FromArgb(8, 10, 13);
            btnPreg.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPreg.ForeColor = Color.White;
            btnPreg.Location = new Point(27, 35);
            btnPreg.Name = "btnPreg";
            btnPreg.Size = new Size(258, 37);
            btnPreg.TabIndex = 3;
            btnPreg.Text = "Personaliza tus preguntas";
            btnPreg.UseVisualStyleBackColor = false;
            btnPreg.Click += button1_Click;
            // 
            // btnConfig
            // 
            btnConfig.BackColor = Color.FromArgb(8, 10, 13);
            btnConfig.Enabled = false;
            btnConfig.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfig.ForeColor = Color.White;
            btnConfig.Location = new Point(27, 140);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(258, 37);
            btnConfig.TabIndex = 4;
            btnConfig.Text = "Configuracion";
            btnConfig.UseVisualStyleBackColor = false;
            //btnConfig.Click += btnConfig_Click;
            // 
            // btnContraseña
            // 
            btnContraseña.BackColor = Color.FromArgb(8, 10, 13);
            btnContraseña.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnContraseña.ForeColor = Color.White;
            btnContraseña.Location = new Point(27, 87);
            btnContraseña.Name = "btnContraseña";
            btnContraseña.Size = new Size(258, 37);
            btnContraseña.TabIndex = 5;
            btnContraseña.Text = "Cambiar contraseña";
            btnContraseña.UseVisualStyleBackColor = false;
            btnContraseña.Click += btnContraseña_Click;
            // 
            // frmUserConfig
            // 
            AccessibleName = "z";
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(8, 10, 13);
            ClientSize = new Size(315, 215);
            Controls.Add(btnContraseña);
            Controls.Add(btnConfig);
            Controls.Add(btnPreg);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmUserConfig";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmConfig";
            ResumeLayout(false);
        }

        #endregion

        private Button btnPreg;
        private Button btnConfig;
        private Button btnContraseña;
    }
}