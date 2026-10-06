using System.Net.Http.Json;
using CapaDatos;
using Entidades;

namespace CapaLogica
{
    public class CL_servicioGeoref
    {
        private readonly HttpClient client = new HttpClient();
        private readonly CD_georefDatos datos = new CD_georefDatos();

        public async Task<bool> cargarGeorefEnBase()
        {
            try
            {
                List<itemGeoref> provincias = await ObtenerProvinciasApi();

                if (provincias.Count == 0)
                    return false;

                foreach (itemGeoref provincia in provincias)
                {
                    if (!datos.guardarProvincia(provincia.Id, provincia.Nombre))
                        return false;

                    List<itemGeoref> partidos = await ObtenerPartidosApi(provincia.Id);

                    foreach (itemGeoref partido in partidos)
                    {
                        datos.guardarPartido(partido.Id, partido.Nombre, provincia.Id);
                    }

                    // una sola llamada por provincia en lugar de una por partido
                    List<itemGeoref> localidades = await ObtenerLocalidadesPorProvinciaApi(provincia.Id);

                    foreach (itemGeoref localidad in localidades)
                    {
                        if (localidad.Departamento == null || string.IsNullOrWhiteSpace(localidad.Departamento.Id))
                            continue;

                        datos.guardarLocalidad(localidad.Id, localidad.Nombre, localidad.Departamento.Id);
                    }
                }

                return datos.hayDatosCargados();
            }
            catch
            {
                return false;
            }
        }

        private async Task<List<itemGeoref>> ObtenerProvinciasApi()
        {
            string url = "https://apis.datos.gob.ar/georef/api/provincias?campos=id,nombre&max=100";

            respuestasGeoref res = await client.GetFromJsonAsync<respuestasGeoref>(url);

            if (res == null || res.Provincias == null)
                return new List<itemGeoref>();

            return res.Provincias
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        private async Task<List<itemGeoref>> ObtenerPartidosApi(string idProvincia)
        {
            string url = $"https://apis.datos.gob.ar/georef/api/departamentos?provincia={idProvincia}&campos=id,nombre&max=500";

            respuestasGeoref res = await client.GetFromJsonAsync<respuestasGeoref>(url);

            if (res == null || res.Departamentos == null)
                return new List<itemGeoref>();

            return res.Departamentos
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        private async Task<List<itemGeoref>> ObtenerLocalidadesPorProvinciaApi(string idProvincia)
        {
            string url = $"https://apis.datos.gob.ar/georef/api/localidades?provincia={idProvincia}&campos=id,nombre,departamento.id&max=5000";

            respuestasGeoref res = await client.GetFromJsonAsync<respuestasGeoref>(url);

            if (res == null || res.Localidades == null)
                return new List<itemGeoref>();

            return res.Localidades
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        public List<Provincia> ObtenerProvincias()
        {
            return datos.obtenerProvincias();
        }

        public List<Partido> ObtenerPartidosPorProvincia(string idProvincia)
        {
            if (string.IsNullOrWhiteSpace(idProvincia))
                return new List<Partido>();

            return datos.obtenerPartidosPorProvincia(idProvincia);
        }

        public List<Localidad> ObtenerLocalidadesPorPartido(string idPartido)
        {
            if (string.IsNullOrWhiteSpace(idPartido))
                return new List<Localidad>();

            return datos.obtenerLocalidadesPorPartido(idPartido);
        }
    }
}