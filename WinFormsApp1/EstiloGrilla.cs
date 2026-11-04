using System.Drawing;
using System.Windows.Forms;

namespace CapaVista
{
    // Estilo comun de las grillas, para que todas las pantallas se vean iguales
    public static class EstiloGrilla
    {
        private static readonly Color FondoGrilla = Color.FromArgb(16, 20, 26);
        private static readonly Color FondoEncabezado = Color.FromArgb(32, 38, 48);
        private static readonly Color FondoFila = Color.FromArgb(22, 27, 34);
        private static readonly Color FondoFilaAlterna = Color.FromArgb(28, 34, 43);
        private static readonly Color FondoSeleccion = Color.RoyalBlue;
        private static readonly Color Lineas = Color.FromArgb(48, 55, 66);
        private static readonly Color Texto = Color.FromArgb(235, 238, 242);

        public static void Aplicar(DataGridView grilla)
        {
            // comportamiento: solo se mira, se selecciona la fila entera y no se cambian los tamaños
            grilla.ReadOnly = true;
            grilla.MultiSelect = false;
            grilla.AllowUserToAddRows = false;
            grilla.AllowUserToDeleteRows = false;
            grilla.AllowUserToResizeRows = false;
            grilla.AllowUserToResizeColumns = false;
            grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // fondo y lineas
            grilla.RowHeadersVisible = false;
            grilla.BackgroundColor = FondoGrilla;
            grilla.BorderStyle = BorderStyle.None;
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla.GridColor = Lineas;

            // encabezados
            grilla.EnableHeadersVisualStyles = false;
            grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.ColumnHeadersHeight = 36;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = FondoEncabezado;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = FondoEncabezado;
            grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grilla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grilla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // filas: todo el contenido centrado
            grilla.DefaultCellStyle.BackColor = FondoFila;
            grilla.DefaultCellStyle.ForeColor = Texto;
            grilla.DefaultCellStyle.SelectionBackColor = FondoSeleccion;
            grilla.DefaultCellStyle.SelectionForeColor = Color.White;
            grilla.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            grilla.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grilla.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            grilla.AlternatingRowsDefaultCellStyle.BackColor = FondoFilaAlterna;

            // alto de las filas, tanto las que ya estan cargadas como las que vengan
            grilla.RowTemplate.Height = 32;
            foreach (DataGridViewRow fila in grilla.Rows)
                fila.Height = 32;
        }
    }
}
