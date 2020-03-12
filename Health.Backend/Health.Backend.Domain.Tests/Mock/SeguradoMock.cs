using Bogus;
using Health.Backend.Domain.Entities;
using Health.Backend.Domain.Models;
using Health.Backend.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Health.Backend.Domain.Tests.Mock
{
    public class SeguradoMock
    {
        private IEnumerable<int> _coberturas;

        private Faker<SeguradoModel> _fakeSegurado;
        private Faker<EnderecoModel> _fakeEndereco;

        public SeguradoMock(IEnumerable<CoberturaEntity> coberturas)
        {
            _coberturas = coberturas.Select(x => x.Id);

            _fakeEndereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => f.Address.ZipCode())
                .RuleFor(p => p.Cidade, f => f.Address.City());

            _fakeSegurado = new Faker<SeguradoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Nome, f => f.Name.FullName())
                .RuleFor(p => p.Nascimento, f => f.Date.Soon())
                .RuleFor(p => p.Endereco, f => _fakeEndereco.Generate())
                .RuleFor(p => p.Coberturas, f => f.PickRandom(_coberturas, 3).ToList());

            Segurado = _fakeSegurado.Generate();
        }

        public SeguradoModel Segurado { get; }

        public SeguradoModel SeguradoEntre0e17Anos()
        {
            _fakeSegurado = CriarFaker(1, 17);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoEntre18e30Anos()
        {
            _fakeSegurado = CriarFaker(18, 30);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoEntre31e45Anos()
        {
            _fakeSegurado = CriarFaker(31, 45);

            return _fakeSegurado.Generate();
        }

        private Faker<SeguradoModel> CriarFaker(int idadeMin, int idadeMax)
        {
            var minDate = DateTime.Today.AddYears(idadeMin * (-1));
            var maxDate = DateTime.Today.AddYears(idadeMax * (-1));
            return new Faker<SeguradoModel>()
               .StrictMode(true)
               .RuleFor(p => p.Nome, f => f.Name.FullName())
               .RuleFor(p => p.Nascimento, f => f.Date.Between(minDate, maxDate))
               .RuleFor(p => p.Endereco, f => _fakeEndereco.Generate())
               .RuleFor(p => p.Coberturas, f => f.PickRandom(_coberturas, 3).ToList());
        }
    }
}
