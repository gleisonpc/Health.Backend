using Health.Backend.Repository.API.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Health.Backend.Repository.API.Tests
{
    public class CidadeRepositoryTest
    {

        [Fact]
        public async Task Valida_Chama_API_Cidades()
        {
            var repositorio = new CidaddeRepository();

            var cidades = await repositorio.ObterCidades();

            Assert.True(cidades.Cities.Count() > 0);
        }
    }
}
