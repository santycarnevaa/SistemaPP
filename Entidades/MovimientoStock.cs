using System;

namespace Entidades
{
    public class MovimientoStock
    {
        public int ID { get; set; }
        public int FK_Producto { get; set; }
        public int FK_Usuario { get; set; }
        public string Tipo_Movimiento { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo_Documento_Origen { get; set; }
        public int? ID_Documento_Origen { get; set; }
    }
}
