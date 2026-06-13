namespace GeolocalizacionIPs.Modelos
{
    public class CSitio
    {
        public int id { get; set; }
        public string ip { get; set; }

        public string sitio { get; set; }

        // Datos posteriores a la consutla
        public string country { get; set; }
        public string city { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }




    }
}
