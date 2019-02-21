namespace Sales.Services
{
    using Common.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApiService
    {
        // Chequear la coneccion 
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet,
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }

        // Metodo Generico para Consumir de cualquier servicio API o cualquier lista
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}"; // Es igual a string.format("{0}{1}", prefix, controller);
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync(); // Es todo el JSON como un string o queda el cod.error.
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //Convertir un string a una lista de Objetos. De-serializar. 
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
