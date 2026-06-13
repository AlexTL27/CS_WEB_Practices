namespace APILocalizacion.Modelos
{
    public class ApiResponse
    {

        public string? ip { get; set; }
        public string? country { get; set; }

        public string? city { get; set; }

        public double? latitude{ get; set; }

        public double? longitude { get; set; }

        public Flag? flag { get; set; }

        public bool error { get; set; }

    }
    public class Flag
    {
        public string img { get; set; }
    }
}
