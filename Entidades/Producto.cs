using System;

namespace Entidades
{
    public class Producto
    {
        public int ID_Codigo { get; set; }
        public int FK_Categoria { get; set; }
        public int FK_Marca { get; set; }
        public string Nombre { get; set; }
        public decimal Precio_Compra { get; set; }
        public decimal Precio_Venta { get; set; }
        public int Stock_Actual { get; set; }
        public int Stock_Critico { get; set; }
        public int Stock_Minimo { get; set; }
        public int Stock_Ideal { get; set; }
        public int Stock_Maximo { get; set; }
        public int Punto_Reposicion { get; set; }
        public string Numero_Lote { get; set; }
        public DateTime? Fecha_Vencimiento { get; set; }
        public string Estado { get; set; }
        public bool Habilitado { get; set; }
    }
}
