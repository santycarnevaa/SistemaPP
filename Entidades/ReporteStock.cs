namespace Entidades
{
    // una fila de los reportes de stock (por categoria o por marca)
    // nombre es la categoria o la marca segun el reporte
    public class ReporteStock
    {
        public string Nombre { get; set; }
        public int Cantidad_Productos { get; set; }
        public int Stock_Total { get; set; }
    }
}
