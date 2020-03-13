using Bogus;
using Health.Backend.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Health.Backend.Domain.Tests.Mock
{
    public class CoberturasMock
    {
        private Faker<CoberturaEntity> _fake;

        public CoberturasMock()
        {
            _fake = new Faker<CoberturaEntity>()
                .StrictMode(true)
                .RuleFor(p => p.Id, f => f.IndexFaker)
                .RuleFor(p => p.Nome, f => f.Lorem.Word() + " " + f.Lorem.Word())
                .RuleFor(p => p.Premio, f => f.Random.Int(0, 2500))
                .RuleFor(p => p.Principal, f => f.PickRandomParam("S", "N"))
                .RuleFor(p => p.Valor, f => f.Random.Int(0, 100000));

            Coberturas = _fake.Generate(100).ToList();
        }

        public IEnumerable<CoberturaEntity> Coberturas { get; }
    }
}
