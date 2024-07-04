﻿

using ExamenP3.Models;
using Newtonsoft.Json;

namespace ExamenP3.Repositorio
{
    internal class PaisRepositorio
    {
        public async Task<List<Pais>> DevulevePaises()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("https://restcountries.com/v3.1/all");
            string response_json = await response.Result.Content.ReadAsStringAsync();

            Pais chuck = JsonConvert.DeserializeObject<ChuckNorris>(response_json);

            return chuck;
        }
    }
}
