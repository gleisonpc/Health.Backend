using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Services;
using Health.Backend.Domain.Services.Interfaces;
using Moq;

namespace Health.Backend.Domain.Tests.Services
{
    public class PrecoServiceTest
    {
        private const MockBehavior BEHAVIOR = MockBehavior.Strict;

        private IPrecoService _precoService;

        private Mock<ICoberturaRepository> _coberturaRepository;

        public PrecoServiceTest()
        {
            _coberturaRepository = new Mock<ICoberturaRepository>(BEHAVIOR);
            _precoService = new PrecoService(_coberturaRepository.Object);
        }

        //[Fact]
        //public void Calcular_Preco_Para_Segurado()
        //{
        //    var mockCobertura = new CoberturasMock();
        //    var mockSegurado = new SeguradoMock(mockCobertura.Coberturas);
        //    _coberturaRepository.Setup(x => x.ObterCoberturas()).Returns(mockCobertura.Coberturas);

        //    var preco = _precoService.ObterPrecoParaSegurado(mockSegurado.Segurado);
        //}
    }
}
