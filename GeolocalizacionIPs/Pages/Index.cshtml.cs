using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

//////////////////////
using System.IO;
using System.Text.Json;
//////////////////////
using GeolocalizacionIPs.Modelos;
using System.Threading.Tasks;

namespace GeolocalizacionIPs.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(5);
        // 5 requests simultáneos máximo

        private readonly IWebHostEnvironment env;
        private readonly HttpClient httpClient;
        public List<CSitio?> SitiosVisitados { get; set; }

        //public string IpIngresada { get; set; }
        public string ErrorMsj { get; set; }


        //No se pasa por Program.cs porque ya esta agregado automaticamente por Asp
        public IndexModel(IWebHostEnvironment _env, IHttpClientFactory factory)
        {
            env = _env;
            httpClient = factory.CreateClient();


            SitiosVisitados = new List<CSitio?>();
        }
       

        //Clase para deserealizar
        public class RaizIp
        {
            public List<CSitio>? ips { get; set; }
        }

        //=======================================================
        //================Al Iniciar=============================
        //=======================================================

        public async Task OnGet()
        {

            //Leer el json
            string json = LeerJson();


            //Transformar el objeto a json
            var Sitios = JsonSerializer.Deserialize<RaizIp>(json);

            if (Sitios?.ips != null) 
            {
                //Consumir la Api por cada sitio
                var tareas = Sitios.ips
                    .Select(i => ConsumirAPI(i));

               

               IEnumerable<CSitio?> resultados =  await Task.WhenAll(tareas);


                SitiosVisitados = resultados
                    .Where(i => i != null)
                    .ToList();

            }
        }


        public string LeerJson()
        {
            try
            {
                var ruta = Path.Combine(env.ContentRootPath, "Datos", "IPs");
                string json = System.IO.File.ReadAllText(ruta);

                //Console.WriteLine(json);
                return json;
            }
            catch 
            {
                Console.WriteLine("No se encontro el archivo");
                return "";
            }
       
        }


        public async Task<CSitio?> ConsumirAPI(CSitio Sitio) 
        {

            await _semaphore.WaitAsync();

            try
            {
                var url = $"https://ipwho.is/{Sitio.ip}";


                //Respuesta solo contiene el objeto de la api
                var respuesta = await httpClient.GetAsync(url);


                if (!respuesta.IsSuccessStatusCode)
                {
                    ErrorMsj = "Algunos datos no se mostraran completos debido a exceso de límite en la Api \"Who.Is\"";
                }
                    string contenido = await respuesta.Content.ReadAsStringAsync();

                    Console.WriteLine($"Status: {respuesta.StatusCode}");
                    Console.WriteLine($"Body: {contenido}");
               //     return null; 
               // }

                
                string json = await respuesta.Content.ReadAsStringAsync();


                //===============Dezerealizar en mis objetos=================

                CSitio? temp = JsonSerializer.Deserialize<CSitio>(json);
                temp.id = Sitio.id;
                temp.sitio = Sitio.sitio;
                temp.ip = Sitio.ip;
                return temp;
            }
            catch
            {

                return null;
            }
            finally
            {
                _semaphore.Release();
            }

      
   
        }
    }
}
