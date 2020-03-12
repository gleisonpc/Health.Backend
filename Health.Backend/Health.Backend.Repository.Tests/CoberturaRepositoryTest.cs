using Health.Backend.Repository.Repositories;
using System;
using System.Linq;
using Xunit;

namespace Health.Backend.Repository.Tests
{
    public class CoberturaRepositoryTest
    {
        [Fact]
        public void Obter_Todas_coberturas()
        {
            var repositorio = new CoberturaRepository();

            var coberturas = repositorio.ObterCoberturas();

            Assert.True(coberturas.Count() == 10);
        }
    }
}
