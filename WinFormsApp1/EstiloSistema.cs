using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaVista
{
    // Colores y estilo comun de todas las ventanas del sistema
    public static class EstiloSistema
    {
        public static readonly Color Fondo = Color.FromArgb(11, 14, 19);
        public static readonly Color Campo = Color.FromArgb(28, 34, 43);
        public static readonly Color Borde = Color.FromArgb(70, 80, 95);
        public static readonly Color Texto = Color.FromArgb(235, 238, 242);
        public static readonly Color Azul = Color.RoyalBlue;
        public static readonly Color Verde = Color.FromArgb(35, 134, 84);
        public static readonly Color Rojo = Color.FromArgb(200, 60, 60);
        public static readonly Color Gris = Color.FromArgb(90, 98, 110);
        public static readonly Color Deshabilitado = Color.FromArgb(45, 50, 58);

        // Se llama una vez en el constructor de cada ventana
        public static void Aplicar(Form ventana)
        {
            ventana.BackColor = Fondo;
            AplicarControles(ventana);
        }

        // Recorre los controles, tambien los que estan adentro de paneles y solapas
        public static void AplicarControles(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                CambiarFuente(control);

                if (control is LinkLabel link)
                {
                    link.LinkColor = Color.CornflowerBlue;
                    link.ActiveLinkColor = Color.LightSkyBlue;
                    link.VisitedLinkColor = Color.CornflowerBlue;
                }
                else if (control is Label etiqueta)
                {
                    etiqueta.BackColor = Color.Transparent;
                    if (etiqueta.ForeColor == Color.Black)
                        etiqueta.ForeColor = Texto;
                }
                else if (control is Button boton)
                    EstilizarBoton(boton);
                else if (control is TextBox caja)
                {
                    caja.BackColor = Campo;
                    caja.ForeColor = Texto;
                    caja.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (control is NumericUpDown numero)
                {
                    numero.BackColor = Campo;
                    numero.ForeColor = Texto;
                    numero.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (control is ComboBox combo)
                {
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.BackColor = Campo;
                    combo.ForeColor = Texto;
                }
                else if (control is CheckBox || control is RadioButton)
                {
                    control.ForeColor = Texto;
                    control.Cursor = Cursors.Hand;
                }
                else if (control is PictureBox)
                {
                    control.BackColor = Color.Transparent;
                    control.Cursor = Cursors.Hand;
                }
                else if (control is DataGridView grilla)
                    EstiloGrilla.Aplicar(grilla);
                else if (control is TabControl solapas)
                    EstilizarSolapas(solapas);
                else if (control is GroupBox || control is Panel)
                {
                    control.BackColor = Fondo;
                    control.ForeColor = Texto;
                }

                if (control.HasChildren)
                    AplicarControles(control);
            }
        }

        // Todo en la misma letra, respetando el tamaño que ya tenia cada control
        private static void CambiarFuente(Control control)
        {
            control.Font = new Font("Segoe UI", control.Font.Size, control.Font.Style, control.Font.Unit);
        }

        private static void EstilizarBoton(Button boton)
        {
            Color color;

            if (boton.BackColor == Color.Green)
                color = Verde;
            else if (boton.BackColor == Color.Red)
                color = Rojo;
            else if (boton.BackColor == Color.RoyalBlue)
                color = Azul;
            else if (boton.BackColor == Color.Gray)
                color = Gris;
            else
                color = Campo;

            // en los botones chicos la letra grande no entra
            if (boton.Height < 28 && boton.Font.SizeInPoints > 9F)
                boton.Font = new Font(boton.Font.FontFamily, 9F, boton.Font.Style);

            boton.FlatStyle = FlatStyle.Flat;
            boton.UseVisualStyleBackColor = false;
            boton.BackColor = color;
            boton.ForeColor = Color.White;
            boton.Cursor = Cursors.Hand;
            boton.FlatAppearance.BorderSize = color == Campo ? 1 : 0;
            boton.FlatAppearance.BorderColor = Borde;
            boton.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color, 0.2F);

            // un boton deshabilitado se ve apagado y vuelve a su color al habilitarse
            Color colorNormal = color;
            boton.EnabledChanged += (s, e) =>
            {
                if (boton.Enabled)
                {
                    boton.BackColor = colorNormal;
                }
                else
                {
                    colorNormal = boton.BackColor;
                    boton.BackColor = Deshabilitado;
                }
            };

            if (!boton.Enabled)
                boton.BackColor = Deshabilitado;
        }

        private static void EstilizarSolapas(TabControl solapas)
        {
            solapas.DrawMode = TabDrawMode.OwnerDrawFixed;
            solapas.SizeMode = TabSizeMode.Fixed;
            solapas.ItemSize = new Size((solapas.Width - 6) / solapas.TabCount, 30);

            // Windows pinta un marco claro alrededor de las solapas y no deja cambiarle el color,
            // asi que se recorta para que no se vea
            solapas.FindForm().Shown += (s, e) =>
            {
                Rectangle pagina = solapas.DisplayRectangle;
                Rectangle pestaña = solapas.GetTabRect(0);

                Region visible = new Region(pagina);
                visible.Union(new Rectangle(pagina.Left, pestaña.Top, pagina.Width, pestaña.Height));
                solapas.Region = visible;
            };

            solapas.DrawItem += (s, e) =>
            {
                bool elegida = e.Index == solapas.SelectedIndex;

                using (SolidBrush fondo = new SolidBrush(elegida ? Azul : Campo))
                    e.Graphics.FillRectangle(fondo, e.Bounds);

                TextRenderer.DrawText(e.Graphics, solapas.TabPages[e.Index].Text, solapas.Font, e.Bounds, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            foreach (TabPage pagina in solapas.TabPages)
            {
                pagina.UseVisualStyleBackColor = false;
                pagina.BackColor = Fondo;
                pagina.ForeColor = Texto;
            }
        }
    }
}
