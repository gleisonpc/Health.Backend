using Health.Backend.Domain.Constants;
using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Tests.Mock;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Health.Backend.Domain.Tests.Models.Requests
{
    public class SeguradoModelTest
    {
        private Mock<ICidadeRepository> _cidadeRepository;
        private Mock<ICoberturaRepository> _coberturaRepository;

        private readonly CidadesMock _cidadesMock;
        private readonly CoberturasMock _coberturaMock;
        private readonly SeguradoMock _seguradoMock;

        public SeguradoModelTest()
        {
            _cidadeRepository = new Mock<ICidadeRepository>();
            _coberturaRepository = new Mock<ICoberturaRepository>();

            _cidadesMock = new CidadesMock();
            _coberturaMock = new CoberturasMock();
            _seguradoMock = new SeguradoMock(
                _coberturaMock.Coberturas,
                _cidadeRepository.Object,
                _coberturaRepository.Object
                );
        }

        //Validar Segurado com todos parametros validos
        //Validar segurado com algum dos parametros invalidos

        [Fact]
        public void Validar_Segurado_Com_Cep_Valido()
        {
            _cidadeRepository.Setup(x => x.ObterCidades()).ReturnsAsync(_cidadesMock.Cidades);

            var segurado = _seguradoMock.SeguradoComCepValido();

            Assert.True(segurado.ValidoCep);
            Assert.True(!segurado.Erros.Any(x => x == MensagensErros.CEP_FORA_DO_PADRA_PERMITIDO));
        }

        [Fact]
        public void Validar_Segurado_Com_Cep_Invalida()
        {
            _cidadeRepository.Setup(x => x.ObterCidades()).ReturnsAsync(_cidadesMock.Cidades);

            var segurado = _seguradoMock.SeguradoComCepInvalido();

            Assert.False(segurado.ValidoCep);
            Assert.Contains(segurado.Erros, x => x == MensagensErros.CEP_FORA_DO_PADRA_PERMITIDO);
        }

        [Fact]
        public void Validar_Segurado_Com_Cidade_Valido()
        {
            _cidadeRepository.Setup(x => x.ObterCidades()).ReturnsAsync(_cidadesMock.Cidades);

            var segurado = _seguradoMock.SeguradoComCidadeValida(_cidadesMock.ListaCidades);

            Assert.True(segurado.ValidoCidade);
            Assert.True(!segurado.Erros.Any(x => x == MensagensErros.CIDADE_NAO_ENCONTRADA));
        }

        [Fact]
        public void Validar_Segurado_Com_Cidade_Invalida()
        {
            _cidadeRepository.Setup(x => x.ObterCidades()).ReturnsAsync(_cidadesMock.Cidades);

            var segurado = _seguradoMock.Segurado;

            Assert.False(segurado.ValidoCidade);
            Assert.Contains(segurado.Erros, x => x == MensagensErros.CIDADE_NAO_ENCONTRADA);
        }

        [Fact]
        public void Validar_Segurado_Com_Cobertura_Invalida()
        {
            _cidadeRepository.Setup(x => x.ObterCidades()).ReturnsAsync(_cidadesMock.Cidades);

            var segurado = _seguradoMock.Segurado;

            Assert.False(segurado.ValidoCoberturaObrigatoria);
            Assert.Contains(segurado.Erros, x => x == MensagensErros.SEGURADO_SEM_NENHUMA_COBERTURA_OBRIGATORIA);
        }

        [Fact]
        public void Validar_Segurado_Com_Menos_18_anos()
        {
            _cidadeRepository.Setup(x => x.ObterCidades()).ReturnsAsync(_cidadesMock.Cidades);
            var segurado = _seguradoMock.SeguradoEntre0e17Anos();

            Assert.False(segurado.ValidoIdade);
            Assert.Contains(segurado.Erros, x => x == MensagensErros.SEGURADO_COM_MENOS_DE_18_ANOS);
        }

        [Fact]
        public void Validar_Data_Nascimento()
        {
            var segurado = _seguradoMock.SeguradoEntre18e30Anos();

            var idade = DateTime.Now.Year - segurado.Nascimento.Year;
            idade = DateTime.Now.Month < segurado.Nascimento.Month ? --idade : DateTime.Now.Day < segurado.Nascimento.Day ? --idade : idade;

            Assert.Equal(segurado.Idade, idade);
        }
    }
}
