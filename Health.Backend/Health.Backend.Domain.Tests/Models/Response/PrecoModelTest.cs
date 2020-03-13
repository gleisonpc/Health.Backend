using Health.Backend.Domain.Models.Responses;
using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Tests.Mock;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Health.Backend.Domain.Tests.Models.Response
{
    public class PrecoModelTest
    {
        private const int IDADE_MAX_ACRESCIMO = 30;
        private const int PORCENTAGEM_ACRESCIMO = 8;
        private const int IDADE_MIN_DESCONTO = 30;
        private const int IDADE_MAX_DESCONTO = 45;

        private Mock<ICidadeRepository> _cidadeRepository;
        private Mock<ICoberturaRepository> _coberturaRepository;

        private readonly CoberturasMock _coberturaMock;
        private readonly SeguradoMock _seguradoMock;

        public PrecoModelTest()
        {
            _cidadeRepository = new Mock<ICidadeRepository>();
            _coberturaRepository = new Mock<ICoberturaRepository>();

            _coberturaMock = new CoberturasMock();
            _seguradoMock = new SeguradoMock(
                _coberturaMock.Coberturas,
                _cidadeRepository.Object,
                _coberturaRepository.Object
                );
        }

        [Fact]
        public void Validar_Calculo_Acrescimo()
        {
            var segurado = _seguradoMock.SeguradoEntre18e30Anos();
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            double acrescimo = (IDADE_MAX_ACRESCIMO - segurado.Idade) * PORCENTAGEM_ACRESCIMO;
            acrescimo = (acrescimo / 100) * precoModel.SubTotal;

            Assert.Equal(acrescimo, precoModel.Acrescimo);
        }

        [Fact]
        public void Validar_Calculo_Desconto()
        {
            var segurado = _seguradoMock.SeguradoEntre31e45Anos();
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            var desconto = segurado.Idade > IDADE_MIN_DESCONTO && segurado.Idade <= IDADE_MAX_DESCONTO ? (segurado.Idade - IDADE_MIN_DESCONTO) * 2 : 0.00;
            desconto = desconto / 100 * precoModel.SubTotal;

            Assert.Equal(desconto, precoModel.Desconto);
        }

        [Fact]
        public void Validar_Calculo_Premio_Acrescimo()
        {
            var segurado = _seguradoMock.SeguradoEntre18e30Anos();
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            double subTotal = CalcularSubTotal(segurado.Coberturas);
            double acrescimo = (IDADE_MAX_ACRESCIMO - segurado.Idade) * PORCENTAGEM_ACRESCIMO;
            acrescimo = (acrescimo / 100) * precoModel.SubTotal;
            var premio = subTotal + acrescimo;

            Assert.Equal(premio, precoModel.Premio);
        }

        [Fact]
        public void Validar_Calculo_Premio_Desconto()
        {
            var segurado = _seguradoMock.SeguradoEntre31e45Anos();
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            var subTotal = CalcularSubTotal(segurado.Coberturas);
            var desconto = segurado.Idade > IDADE_MIN_DESCONTO && segurado.Idade <= IDADE_MAX_DESCONTO ? (segurado.Idade - IDADE_MIN_DESCONTO) * 2 : 0.00;
            desconto = desconto / 100 * precoModel.SubTotal;
            var premio = subTotal - desconto;

            Assert.Equal(desconto, precoModel.Desconto);
        }

        [Fact]
        public void Validar_Parcela_Uma()
        {
            var segurado = _seguradoMock.SeguradoComPremioEspecificos(x => x.Premio > 100 && x.Premio < 200);
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            Assert.Equal(1, precoModel.Parcelas);
        }

        [Fact]
        public void Validar_Parcela_Duas()
        {
            var segurado = _seguradoMock.SeguradoComPremioEspecificos(x => x.Premio > 400 && x.Premio < 500);
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            Assert.Equal(2, precoModel.Parcelas);
        }

        [Fact]
        public void Validar_Parcela_Tres()
        {
            var segurado = _seguradoMock.SeguradoComPremioEspecificos(x => x.Premio > 500 && x.Premio < 1000);
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            Assert.Equal(3, precoModel.Parcelas);
        }

        [Fact]
        public void Validar_Parcela_Quatro()
        {
            var segurado = _seguradoMock.SeguradoComPremioEspecificos(x => x.Premio > 2000);
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas, DateTime.Now);

            Assert.Equal(4, precoModel.Parcelas);
        }

        [Fact]
        public void Validar_Valor_Calculado_SubTotal()
        {
            var precoModel = PrecoModel.CriarPrecoModel(_seguradoMock.Segurado, _coberturaMock.Coberturas, DateTime.Now);

            var subTotal = CalcularSubTotal(_seguradoMock.Segurado.Coberturas);

            Assert.Equal(subTotal, precoModel.SubTotal);
        }

        [Fact]
        public void Validar_Valor_Parcela()
        {
            var precoModel = PrecoModel.CriarPrecoModel(_seguradoMock.Segurado, _coberturaMock.Coberturas, DateTime.Now);
            var precoParcela = precoModel.Premio / precoModel.Parcelas;

            Assert.Equal(precoParcela, precoModel.ValorParcelas);
        }

        [Theory]
        [InlineData(2019, 6, 1, 2019, 7, 5)]
        [InlineData(2019, 9, 1, 2019, 10, 7)]
        [InlineData(2019, 12, 1, 2020, 1, 7)]
        [InlineData(2019, 7, 1, 2019, 8, 7)]
        [InlineData(2019, 10, 1, 2019, 11, 7)]
        [InlineData(2020, 1, 1, 2020, 2, 7)]
        [InlineData(2020, 2, 1, 2020, 3, 6)]
        public void Validar_Datas_Vencimento(int anoSimulacao, int mesSimulacao, int diaSimulacao, int anoVencimento, int mesVencimento, int diaVencimento)
        {
            var precoModel = PrecoModel.CriarPrecoModel(_seguradoMock.Segurado, _coberturaMock.Coberturas, new DateTime(anoSimulacao, mesSimulacao, diaSimulacao));

            Assert.Equal(anoVencimento, precoModel.PrimeiroVencimento.Date.Year);
            Assert.Equal(mesVencimento, precoModel.PrimeiroVencimento.Date.Month);
            Assert.Equal(diaVencimento, precoModel.PrimeiroVencimento.Date.Day);
        }

        private double CalcularSubTotal(IEnumerable<int> coberturasSegurado) =>
            _coberturaMock.Coberturas.Where(x => coberturasSegurado.Any(id => id == x.Id)).Sum(x => x.Premio);
    }
}
