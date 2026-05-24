namespace Entidades
{
    public class respuestasGeoref
    {
        public List<itemGeoref> Provincias { get; set; }
        public List<itemGeoref> Departamentos { get; set; }
        public List<itemGeoref> Localidades { get; set; }
    }

    public class itemGeoref
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}