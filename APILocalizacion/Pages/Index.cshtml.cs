using APILocalizacion.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.Json;

namespace APILocalizacion.Pages
{
    public class IndexModel : PageModel
    {

        /*
            IP
            Pais
            Ciudad
            Latitud
            Longitud
            
            Imagen
         
         */

        public readonly HttpClient? _httpClient;
        public ApiResponse modelo = new ApiResponse();

        [BindProperty]
        public string? IpIngresada { get; set; }

        //Es capaz de realizar peticiones en la web
        public IndexModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }



        public async Task OnPostAsync()
        {
           

            var url = !string.IsNullOrWhiteSpace(IpIngresada)
                ? $"https://ipwho.is/{IpIngresada}"
                : "https://ipwho.is/";



            var respuesta = await _httpClient.GetAsync(url);


            //deserealiza la respuesta en mi clase ApiResponse
            modelo = await respuesta.Content.ReadFromJsonAsync<ApiResponse>();

            //si son letras
            if ( 
                (modelo.ip.Any(c => char.IsLetter(c)))
               )
            {
                modelo = new ApiResponse();
                modelo.error = true;
            }
        }
    }
}
