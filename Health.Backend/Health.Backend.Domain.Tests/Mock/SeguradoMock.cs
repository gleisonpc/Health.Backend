using Bogus;
using Health.Backend.Domain.Entities;
using Health.Backend.Domain.Models;
using Health.Backend.Domain.Models.Requests;
using Health.Backend.Domain.Repositories.Interfaces;
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

        private readonly ICidadeRepository _cidadeRepository;
        private readonly ICoberturaRepository _coberturaRepository;

        public SeguradoMock(
            IEnumerable<CoberturaEntity> coberturas,
            ICidadeRepository cidadeRepository,
            ICoberturaRepository coberturaRepository)
        {
            _cidadeRepository = cidadeRepository;
            _coberturaRepository = coberturaRepository;
            _coberturas = coberturas.Select(x => x.Id);

            _fakeEndereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => f.Address.ZipCode())
                .RuleFor(p => p.Cidade, f => f.Address.City());

            _fakeSegurado = new Faker<SeguradoModel>()
                .CustomInstantiator(f => new SeguradoModel(_cidadeRepository, _coberturaRepository))
                .StrictMode(true)
                .RuleFor(p => p.Nome, f => f.Name.FullName())
                .RuleFor(p => p.Nascimento, f => f.Date.Soon())
                .RuleFor(p => p.Endereco, f => _fakeEndereco.Generate())
                .RuleFor(p => p.Coberturas, f => f.PickRandom(_coberturas, 3).ToList());

            Segurado = _fakeSegurado.Generate();
        }

        public SeguradoModel Segurado { get; }

        public SeguradoModel SeguradoComCepValido()
        {
            var endereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => f.Address.ZipCode("00000-000"))
                .RuleFor(p => p.Cidade, f => f.Address.City());

            _fakeSegurado = CriarFaker(18, 30, endereco);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoComCepInvalido()
        {
            var endereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => f.Address.ZipCode("00000"))
                .RuleFor(p => p.Cidade, f => f.Address.City());

            _fakeSegurado = CriarFaker(18, 30, endereco);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoComCidadeValida(IEnumerable<CidadeEntity> cidades)
        {
            var endereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => f.Address.ZipCode())
                .RuleFor(p => p.Cidade, f => f.PickRandom(cidades.Select(x => x.Name)));

            _fakeSegurado = CriarFaker(18, 30, endereco);

            return _fakeSegurado.Generate();
        }

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

        private Faker<SeguradoModel> CriarFaker(int idadeMin, int idadeMax, Faker<EnderecoModel> endereco = null)
        {
            var minDate = DateTime.Today.AddYears(idadeMin * (-1));
            var maxDate = DateTime.Today.AddYears(idadeMax * (-1));
            return new Faker<SeguradoModel>()
                .CustomInstantiator(f => new SeguradoModel(_cidadeRepository, _coberturaRepository))
               .StrictMode(true)
               .RuleFor(p => p.Nome, f => f.Name.FullName())
               .RuleFor(p => p.Nascimento, f => f.Date.Between(minDate, maxDate))
               .RuleFor(p => p.Endereco, f => endereco == null ? _fakeEndereco.Generate() : endereco)
               .RuleFor(p => p.Coberturas, f => f.PickRandom(_coberturas, 3).ToList());
        }
    }
}
