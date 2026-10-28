using CapaDatos;
using Entidades;

namespace CapaLogica
{
    public class CL_servicioAuditoria
    {
        private CD_auditoriaDatos auditoriaDatos = new CD_auditoriaDatos();

        public List<AuditoriaCambio> ConsultarAuditoria(string tablaAfectada = null, int? idUsuario = null, DateTime? desde = null, DateTime? hasta = null)
        {
            return auditoriaDatos.Obtener(tablaAfectada, idUsuario, desde, hasta);
        }
    }
}
