using System;

namespace Entidades
{
    public class AuditoriaCambio
    {
        public int ID { get; set; }
        public string Tabla_Afectada { get; set; }
        public int ID_Registro_Afectado { get; set; }
        public string Accion { get; set; }
        public int FK_Usuario { get; set; }
        public DateTime Fecha_Hora { get; set; }
        public string Detalle_Cambio { get; set; }
    }
}
