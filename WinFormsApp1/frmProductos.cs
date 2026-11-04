using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaLogica;
using Entidades;

namespace CapaVista
{
    // Listado de productos. El alta, la modificacion y la consulta se hacen en frmProductoDetalle.
    public partial class frmProductos : Form
    {
        private readonly string usuario;
        private int idUsuario;

        private readonly CL_servicioProductos servicioProductos = new CL_servicioProductos();
        private readonly CL_servicioUsuarios servicioUsuarios = new CL_servicioUsuarios();

        private readonly bool soloLectura;

        private List<Producto> productos = new List<Producto>();
        private List<Categoria> categorias = new List<Categoria>();
        private List<Marca> marcas = new List<Marca>();

        public frmProductos(string usuario, bool soloLectura = false)
        {
            InitializeComponent();
            EstiloSistema.Aplicar(this);
            ComboBuscable.Activar(cmbFiltroCategoria);
            ComboBuscable.Activar(cmbFiltroMarca);
            this.usuario = usuario;
            this.soloLectura = soloLectura;

            if (soloLectura)
            {
                Text = "Productos (solo consulta)";
                btnNuevo.Visible = false;
            }
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            idUsuario = servicioUsuarios.ObtenerIdUsuario(usuario);

            ConfigurarGrilla();
            CargarFiltros();
            CargarProductos();

            // el filtrado es automatico: se aplica cada vez que cambia algun filtro
            cmbFiltroCategoria.SelectedIndexChanged += CambioFiltro;
            cmbFiltroMarca.SelectedIndexChanged += CambioFiltro;
            chkMostrarBaja.CheckedChanged += CambioFiltro;
        }

        private void frmProductos_Shown(object sender, EventArgs e)
        {
            dgvProductos.ClearSelection();
        }

        private void ConfigurarGrilla()
        {
            dgvProductos.AutoGenerateColumns = false;
            EstiloGrilla.Aplicar(dgvProductos);

            AgregarColumna("ID_Codigo", "N°", 55);
            AgregarColumna("Nombre", "Nombre", 0);
            AgregarColumna("", "Categoría", 120, "colCategoria");
            AgregarColumna("", "Marca", 130, "colMarca");

            DataGridViewTextBoxColumn precio = AgregarColumna("Precio_Venta", "Precio venta", 115);
            precio.DefaultCellStyle.Format = "N2";

            AgregarColumna("Stock_Actual", "Stock", 70);
            AgregarColumna("", "Estado", 80, "colEstado");

            AgregarColumnaIcono("colVer", "Ver detalle", Color.LightGray);

            if (!soloLectura)
            {
                AgregarColumnaIcono("colEditar", "Modificar", Color.CornflowerBlue);
                AgregarColumnaIcono("colBaja", "Dar de baja", Color.Tomato);
            }
        }

        private DataGridViewTextBoxColumn AgregarColumna(string propiedad, string titulo, int ancho, string nombre = null)
        {
            DataGridViewTextBoxColumn columna = new DataGridViewTextBoxColumn
            {
                Name = nombre ?? propiedad,
                DataPropertyName = propiedad,
                HeaderText = titulo,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            // cada columna tiene un ancho fijo y el nombre (ancho 0) ocupa lo que sobra
            if (ancho > 0)
            {
                columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                columna.Width = ancho;
            }

            dgvProductos.Columns.Add(columna);
            return columna;
        }

        // Los iconos son letras de la fuente de iconos de Windows
        private void AgregarColumnaIcono(string nombre, string descripcion, Color color)
        {
            DataGridViewTextBoxColumn columna = new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = "",
                ToolTipText = descripcion,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 46,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            columna.DefaultCellStyle.Font = new Font("Segoe MDL2 Assets", 11F);
            columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            columna.DefaultCellStyle.ForeColor = color;
            columna.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvProductos.Columns.Add(columna);
        }

        private void CargarFiltros()
        {
            categorias = servicioProductos.ObtenerCategorias();
            marcas = servicioProductos.ObtenerMarcas();

            List<Categoria> opcionesCategoria = new List<Categoria> { new Categoria { IdCategoria = 0, Nombre = "(Todas)" } };
            opcionesCategoria.AddRange(categorias);
            cmbFiltroCategoria.DisplayMember = "Nombre";
            cmbFiltroCategoria.DataSource = opcionesCategoria;

            List<Marca> opcionesMarca = new List<Marca> { new Marca { IdMarca = 0, Nombre = "(Todas)" } };
            opcionesMarca.AddRange(marcas);
            cmbFiltroMarca.DisplayMember = "Nombre";
            cmbFiltroMarca.DataSource = opcionesMarca;
        }

        private void CargarProductos()
        {
            productos = servicioProductos.ObtenerProductos(false);
            AplicarFiltros();
        }

        private void CambioFiltro(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            IEnumerable<Producto> filtrados = productos;

            if (cmbFiltroCategoria.SelectedItem is Categoria categoria && categoria.IdCategoria > 0)
                filtrados = filtrados.Where(p => p.FK_Categoria == categoria.IdCategoria);

            if (cmbFiltroMarca.SelectedItem is Marca marca && marca.IdMarca > 0)
                filtrados = filtrados.Where(p => p.FK_Marca == marca.IdMarca);

            if (!chkMostrarBaja.Checked)
                filtrados = filtrados.Where(p => p.Habilitado);

            List<Producto> lista = filtrados.ToList();

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = lista;
            dgvProductos.ClearSelection();

            lblRegistros.Text = "Registros: " + lista.Count;
        }

        private Producto ProductoDeFila(int fila)
        {
            if (fila < 0 || fila >= dgvProductos.Rows.Count)
                return null;

            return dgvProductos.Rows[fila].DataBoundItem as Producto;
        }

        private void dgvProductos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Producto p = ProductoDeFila(e.RowIndex);

            if (p == null)
                return;

            switch (dgvProductos.Columns[e.ColumnIndex].Name)
            {
                case "colCategoria":
                    e.Value = categorias.FirstOrDefault(c => c.IdCategoria == p.FK_Categoria)?.Nombre;
                    break;
                case "colMarca":
                    e.Value = marcas.FirstOrDefault(m => m.IdMarca == p.FK_Marca)?.Nombre;
                    break;
                case "colEstado":
                    e.Value = p.Habilitado ? p.Estado : "Baja";
                    break;
                case "colVer":
                    e.Value = "";
                    break;
                case "colEditar":
                    e.Value = p.Habilitado ? "" : "";
                    break;
                case "colBaja":
                    e.Value = p.Habilitado ? "" : "";
                    break;
            }
        }

        // Dice si en esa celda hay un icono que se pueda usar
        private bool HayIcono(int fila, int columna)
        {
            Producto p = ProductoDeFila(fila);

            if (p == null || columna < 0)
                return false;

            string nombre = dgvProductos.Columns[columna].Name;

            return nombre == "colVer" || ((nombre == "colEditar" || nombre == "colBaja") && p.Habilitado);
        }

        private void dgvProductos_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvProductos.Cursor = HayIcono(e.RowIndex, e.ColumnIndex) ? Cursors.Hand : Cursors.Default;
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!HayIcono(e.RowIndex, e.ColumnIndex))
                return;

            Producto p = ProductoDeFila(e.RowIndex);

            switch (dgvProductos.Columns[e.ColumnIndex].Name)
            {
                case "colVer":
                    AbrirDetalle(p, true);
                    break;
                case "colEditar":
                    AbrirDetalle(p, false);
                    break;
                case "colBaja":
                    DarDeBaja(p);
                    break;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            AbrirDetalle(null, false);
        }

        private void AbrirDetalle(Producto producto, bool consulta)
        {
            using (frmProductoDetalle frm = new frmProductoDetalle(idUsuario, producto, consulta))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    CargarProductos();
            }
        }

        private void DarDeBaja(Producto producto)
        {
            if (!new MensajeConfirmacion("¿Confirmás dar de baja el producto \"" + producto.Nombre + "\"?").Confirmado())
                return;

            if (servicioProductos.DarBajaProducto(producto.ID_Codigo, idUsuario))
            {
                new MensajeExito("Producto dado de baja.").Mostrar();
                CargarProductos();
            }
            else
            {
                new MensajeError("No se pudo dar de baja el producto.").Mostrar();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
