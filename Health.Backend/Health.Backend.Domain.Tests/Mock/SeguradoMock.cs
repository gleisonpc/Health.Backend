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
        private IEnumerable<CoberturaEntity> _coberturas;

        private Faker<SeguradoModel> _fakeSegurado;
        private Faker<EnderecoModel> _fakeEndereco;

        private readonly ICidadeRepository _cidadeRepository;
        private readonly ICoberturaRepository _coberturaRepository;

        public SeguradoMock(
            IEnumerable<CoberturaEntity> coberturas,
            ICidadeRepository cidadeRepository,
            ICoberturaRepository coberturaRepository)
        {
            _coberturas = coberturas;
            _cidadeRepository = cidadeRepository;
            _coberturaRepository = coberturaRepository;

            _fakeEndereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => f.Address.ZipCode())
                .RuleFor(p => p.Cidade, f => f.Address.City());

            _fakeSegurado = _fakeSegurado = CriarFaker(22, 43);

            Segurado = _fakeSegurado.Generate();
        }

        public SeguradoModel Segurado { get; }

        public SeguradoModel SeguradoComCepValido()
        {
            var endereco = CriarFakerEndereco("00000-000");

            _fakeSegurado = CriarFaker(18, 30, endereco);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoComCepInvalido()
        {
            var endereco = CriarFakerEndereco("00000");

            _fakeSegurado = CriarFaker(18, 30, endereco);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoComCidadeValida(IEnumerable<CidadeEntity> cidades)
        {
            var endereco = CriarFakerEndereco(null, cidades);

            _fakeSegurado = CriarFaker(18, 30, endereco);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoComCoberturasInvalidas()
        {
            _fakeSegurado = CriarFaker(18, 30, null, true);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoComMaisDeQuatroCoberturas()
        {
            _fakeSegurado = CriarFaker(18, 30, null, false, true);

            return _fakeSegurado.Generate();
        }

        public SeguradoModel SeguradoComPremioEspecificos(Func<CoberturaEntity, bool> expressao)
        {
            _fakeSegurado = CriarFaker(30, 30, null, false, false, expressao);

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

        private Faker<SeguradoModel> CriarFaker(int idadeMin, int idadeMax, Faker<EnderecoModel> endereco = null, bool coberturaInvalida = false, bool maisDeQuatroCoberturas = false, Func<CoberturaEntity, bool> valorPremio = null)
        {
            var coberturas = valorPremio == null ? _coberturas.Where(x => x.Principal == "S").Select(x => x.Id) : _coberturas.Where(valorPremio).Select(x => x.Id);
            var coberturasInvalida = _coberturas.Where(x => x.Principal == "N").Select(x => x.Id);

            var minDate = DateTime.Today.AddYears(idadeMin * (-1));
            var maxDate = DateTime.Today.AddYears(idadeMax * (-1));
            return new Faker<SeguradoModel>()
                .CustomInstantiator(f => new SeguradoModel(_cidadeRepository, _coberturaRepository))
               .StrictMode(true)
               .RuleFor(p => p.Nome, f => f.Name.FullName())
               .RuleFor(p => p.Nascimento, f => f.Date.Between(minDate, maxDate))
               .RuleFor(p => p.Endereco, f => endereco == null ? _fakeEndereco.Generate() : endereco)
               .RuleFor(p => p.Coberturas, f => coberturaInvalida ? f.PickRandom(coberturasInvalida, 3) : maisDeQuatroCoberturas ? f.PickRandom(coberturas, 5).ToList() : f.PickRandom(coberturas, 2).ToList());
        }

        private Faker<EnderecoModel> CriarFakerEndereco(string formatoCep = null, IEnumerable<CidadeEntity> cidades = null)
        {
            var endereco = new Faker<EnderecoModel>()
                .StrictMode(true)
                .RuleFor(p => p.Logradouro, f => f.Address.StreetAddress() + ", " + f.Address.SecondaryAddress())
                .RuleFor(p => p.Bairro, f => string.Empty)
                .RuleFor(p => p.Cep, f => formatoCep == null ? f.Address.ZipCode() : f.Address.ZipCode(formatoCep))
                .RuleFor(p => p.Cidade, f => cidades == null ? f.Address.City() : f.PickRandom(cidades.Select(x => x.Name)));

            return endereco;
        }
    }
}
