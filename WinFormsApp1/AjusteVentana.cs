using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaVista
{
    // Ajusta el ancho de la ventana al de la grilla, asi no hace falta
    // mover la barra de abajo para ver todas las columnas
    public static class AjusteVentana
    {
        public static void AjustarAGrilla(Form ventana, DataGridView grilla)
        {
            EstiloGrilla.Aplicar(grilla);

            // los nombres de las columnas vienen de la base con guion bajo, en pantalla se muestran con espacios
            foreach (DataGridViewColumn columna in grilla.Columns)
                columna.HeaderText = columna.HeaderText.Replace('_', ' ');

            // calculo el ancho que necesita cada columna segun lo que tiene adentro
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grilla.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            int anchoColumnas = 0;
            foreach (DataGridViewColumn columna in grilla.Columns)
            {
                if (columna.Visible)
                    anchoColumnas += columna.Width;
            }

            if (anchoColumnas == 0)
                return;

            // le sumo el rectangulo del costado y los bordes de la grilla
            int anchoGrilla = anchoColumnas + 2;
            if (grilla.RowHeadersVisible)
                anchoGrilla += grilla.RowHeadersWidth;

            // la barra de la derecha solo ocupa lugar si las filas no entran en la grilla
            int altoFilas = grilla.ColumnHeadersHeight + grilla.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
            if (altoFilas > grilla.Height)
                anchoGrilla += SystemInformation.VerticalScrollBarWidth;

            int diferencia = anchoGrilla - grilla.Width;
            int borde = ventana.Width - ventana.ClientSize.Width;
            int anchoMaximo = Screen.FromControl(ventana).WorkingArea.Width - borde;

            // los que estan anclados a la derecha (como el icono de volver) se mueven solos con el borde
            // de la ventana, asi que no cuentan para calcular el ancho
            var controles = ventana.Controls.Cast<Control>()
                .Where(c => !(c.Anchor.HasFlag(AnchorStyles.Right) && !c.Anchor.HasFlag(AnchorStyles.Left)))
                .ToList();

            // lo que hay a la derecha de la grilla, a su misma altura, se corre lo mismo que ella
            // (lo que esta arriba, como los filtros, se queda donde esta para que no se corte)
            int derechaGrilla = grilla.Right;
            var aLaDerecha = controles
                .Where(c => c != grilla && c.Left >= derechaGrilla
                            && c.Top < grilla.Bottom && c.Bottom > grilla.Top)
                .ToList();

            int margen = ventana.ClientSize.Width - controles.Max(c => c.Right);

            int nuevoAncho = controles
                .Max(c => c == grilla ? c.Left + anchoGrilla : c.Right + (aLaDerecha.Contains(c) ? diferencia : 0)) + margen;

            // si no entra en la pantalla, la grilla se achica y las columnas se reparten el espacio
            bool entraEnPantalla = nuevoAncho <= anchoMaximo;
            if (!entraEnPantalla)
            {
                int exceso = nuevoAncho - anchoMaximo;
                anchoGrilla -= exceso;
                diferencia -= exceso;
                nuevoAncho = anchoMaximo;
            }

            // si la grilla es mas angosta que la ventana (por los filtros de arriba), ocupa todo el ancho
            int anchoLibre = nuevoAncho - margen - grilla.Left;
            bool sobraEspacio = aLaDerecha.Count == 0 && anchoGrilla < anchoLibre;
            if (sobraEspacio)
                anchoGrilla = anchoLibre;

            if (!entraEnPantalla || sobraEspacio)
            {
                foreach (DataGridViewColumn columna in grilla.Columns)
                    columna.FillWeight = Math.Max(columna.Width, 1);

                grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            grilla.Width = anchoGrilla;

            foreach (Control control in aLaDerecha)
                control.Left += diferencia;

            ventana.ClientSize = new Size(nuevoAncho, ventana.ClientSize.Height);

            // que la ventana no quede cortada por el borde de la pantalla
            Rectangle area = Screen.FromControl(ventana).WorkingArea;
            if (ventana.Right > area.Right)
                ventana.Left = area.Right - ventana.Width;
            if (ventana.Left < area.Left)
                ventana.Left = area.Left;
        }
    }
}
