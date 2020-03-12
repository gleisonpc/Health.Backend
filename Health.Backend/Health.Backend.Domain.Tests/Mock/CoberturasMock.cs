using Bogus;
using Health.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Tests.Mock
{
    public class CoberturasMock
    {
        private Faker<CoberturaEntity> _fake;
        private IEnumerable<CoberturaEntity> _coberturas;

        public CoberturasMock()
        {
            _fake = new Faker<CoberturaEntity>()
                .StrictMode(true)
                .RuleFor(p => p.Id, f => f.IndexFaker)
                .RuleFor(p => p.Nome, f => f.Lorem.Word() + " " + f.Lorem.Word())
                .RuleFor(p => p.Premio, f => f.Random.Double(0.00, 150.00))
                .RuleFor(p => p.Principal, f => f.PickRandomParam("S", "N"))
                .RuleFor(p => p.Valor, f => f.Random.Double(2500.00, 50000.00));

            _coberturas = _fake.Generate(10);
        }

        public IEnumerable<CoberturaEntity> Coberturas
        {
            get
            {
                return _coberturas;
            }
        }
    }
}
