using Health.Backend.Domain.Entities;
using Health.Backend.Domain.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Health.Backend.Repository.API.Repositories
{
    public class CidaddeRepository : ICidadeRepository
    {
        public async Task<CidadesEntity> ObterCidades()
        {
            CidadesEntity cidades = null;
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("https://www.redesocialdecidades.org.br/cities");
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = await response.Content.ReadAsStringAsync();
                cidades = JsonConvert.DeserializeObject<CidadesEntity>(dataObjects);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            client.Dispose();

            return cidades;
        }
    }
}
