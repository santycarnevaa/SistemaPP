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

        // Datos de lectura que arma el SP del kardex (no son columnas de la tabla).
        public string Producto { get; set; }
        public string Usuario { get; set; }
        public int Stock_Posterior { get; set; }
    }
}
