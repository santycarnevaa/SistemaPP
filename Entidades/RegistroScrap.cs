using System;

namespace Entidades
{
    public class RegistroScrap
    {
        public int ID { get; set; }
        public int FK_Producto { get; set; }
        public int FK_Usuario { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; }
        public decimal Costo_Valorizado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
