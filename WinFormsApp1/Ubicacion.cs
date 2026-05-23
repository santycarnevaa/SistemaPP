using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Ubicacion
{
    public string Id { get; set; }
    public string Nombre { get; set; }
}

public class RespuestaApi
{
    [JsonPropertyName("provincias")]
    public List<Ubicacion> Provincias { get; set; }

    [JsonPropertyName("departamentos")]
    public List<Ubicacion> Departamentos { get; set; }

    [JsonPropertyName("localidades")]
    public List<Ubicacion> Localidades { get; set; }
}
