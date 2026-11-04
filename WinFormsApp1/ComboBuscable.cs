using System;
using System.Windows.Forms;

namespace CapaVista
{
    // Deja escribir en el combo para buscar una opcion de la lista
    public static class ComboBuscable
    {
        public static void Activar(ComboBox combo)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDown;
            combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            combo.AutoCompleteSource = AutoCompleteSource.ListItems;

            // deshabilitado, Windows lo pinta de blanco si es de escritura; asi se ve como los demas
            combo.EnabledChanged += (s, e) =>
            {
                combo.DropDownStyle = combo.Enabled ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            };

            // si escribio algo que no esta en la lista no queda elegido nada
            combo.Leave += (s, e) =>
            {
                int posicion = combo.FindStringExact(combo.Text);

                if (posicion < 0)
                {
                    combo.SelectedIndex = -1;
                    combo.Text = "";
                }
                else if (posicion != combo.SelectedIndex)
                {
                    combo.SelectedIndex = posicion;
                }
            };

            // al quitarle la lista tampoco tiene que quedar el texto viejo
            combo.DataSourceChanged += (s, e) =>
            {
                if (combo.DataSource == null)
                    combo.Text = "";
            };
        }
    }
}
