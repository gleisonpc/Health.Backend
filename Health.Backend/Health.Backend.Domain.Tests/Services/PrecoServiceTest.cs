using Health.Backend.Domain.Repositories.Interfaces;
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
        }
    }
}
