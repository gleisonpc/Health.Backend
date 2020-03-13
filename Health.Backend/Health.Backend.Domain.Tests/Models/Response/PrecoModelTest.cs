using Health.Backend.Domain.Models.Responses;
using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Tests.Mock;
using Moq;
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
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas);

            double acrescimo = (IDADE_MAX_ACRESCIMO - segurado.Idade) * PORCENTAGEM_ACRESCIMO;
            acrescimo = (acrescimo / 100) * precoModel.SubTotal;

            Assert.Equal(acrescimo, precoModel.Acrescimo);
        }

        [Fact]
        public void Validar_Calculo_Desconto()
        {
            var segurado = _seguradoMock.SeguradoEntre31e45Anos();
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas);

            var desconto = segurado.Idade > IDADE_MIN_DESCONTO && segurado.Idade <= IDADE_MAX_DESCONTO ? (segurado.Idade - IDADE_MIN_DESCONTO) * 2 : 0.00;
            desconto = desconto / 100 * precoModel.SubTotal;

            Assert.Equal(desconto, precoModel.Desconto);
        }

        [Fact]
        public void Validar_Calculo_Premio_Acrescimo()
        {
            var segurado = _seguradoMock.SeguradoEntre18e30Anos();
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas);

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
            var precoModel = PrecoModel.CriarPrecoModel(segurado, _coberturaMock.Coberturas);

            var subTotal = CalcularSubTotal(segurado.Coberturas);
            var desconto = segurado.Idade > IDADE_MIN_DESCONTO && segurado.Idade <= IDADE_MAX_DESCONTO ? (segurado.Idade - IDADE_MIN_DESCONTO) * 2 : 0.00;
            desconto = desconto / 100 * precoModel.SubTotal;
            var premio = subTotal - desconto;

            Assert.Equal(desconto, precoModel.Desconto);
        }

        [Fact]
        public void Validar_Valor_Calculado_SubTotal()
        {
            var precoModel = PrecoModel.CriarPrecoModel(_seguradoMock.Segurado, _coberturaMock.Coberturas);

            var subTotal = CalcularSubTotal(_seguradoMock.Segurado.Coberturas);

            Assert.Equal(subTotal, precoModel.SubTotal);
        }

        private double CalcularSubTotal(IEnumerable<int> coberturasSegurado) =>
            _coberturaMock.Coberturas.Where(x => coberturasSegurado.Any(id => id == x.Id)).Sum(x => x.Valor);
    }
}
