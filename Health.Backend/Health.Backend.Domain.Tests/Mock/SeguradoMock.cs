using Bogus;
using Health.Backend.Domain.Entities;
using Health.Backend.Domain.Models;
using Health.Backend.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Health.Backend.Domain.Tests.Mock
{
    public class SeguradoMock
    {
        private Faker<SeguradoModel> _fakeSegurado;
        private Faker<EnderecoModel> _fakeEndereco;

        public SeguradoMock(IEnumerable<CoberturaEntity> coberturas)
        {
            _fakeEndereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => f.Address.ZipCode())
                .RuleFor(p => p.Cidade, f => f.Address.City());

            _fakeSegurado = new Faker<SeguradoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Nome, f => f.Name.FullName() )
                .RuleFor(p => p.Nascimento, f => f.Date.Past(30))
                .RuleFor(p => p.Endereco, f => _fakeEndereco.Generate())
                .RuleFor(p => p.Coberturas, f => f.PickRandom(coberturas.Select(x => x.Id), 3));
        }

        public SeguradoModel Segurado { 
            get {
                return _fakeSegurado.Generate();
            } 
        }
    }
}
