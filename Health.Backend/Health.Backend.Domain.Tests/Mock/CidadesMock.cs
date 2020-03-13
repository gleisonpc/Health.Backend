using Bogus;
using Health.Backend.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Health.Backend.Domain.Tests.Mock
{

    public class CidadesMock
    {
        private Faker<CidadesEntity> _cidadesMock;

        public CidadesMock()
        {
            var cidade = new Faker<CidadeEntity>()
                            .StrictMode(true)
                            .RuleFor(x => x.Name, f => f.Address.City())
                            .RuleFor(x => x.Uf, f => f.Address.StateAbbr())
                            .RuleFor(x => x.Pais, f => f.Address.Country());

            ListaCidades = cidade.Generate(10).ToList();

            _cidadesMock = new Faker<CidadesEntity>()
                .StrictMode(true)
                .RuleFor(p => p.Cities, f => ListaCidades);

            Cidades = _cidadesMock.Generate();
        }

        public CidadesEntity Cidades { get; }

        public IEnumerable<CidadeEntity> ListaCidades { get; }
    }
}
