using Health.Backend.Domain.Tests.Mock;
using System;
using Xunit;

namespace Health.Backend.Domain.Tests.Models.Requests
{
    public class SeguradoModelTest
    {
        private readonly CoberturasMock _coberturaMock;
        private readonly SeguradoMock _seguradoMock;

        public SeguradoModelTest()
        {
            _coberturaMock = new CoberturasMock();
            _seguradoMock = new SeguradoMock(_coberturaMock.Coberturas);
        }

        [Fact]
        public void Validar_Segurado_Com_Menos_18_anos()
        {
            var segurado = _seguradoMock.SeguradoEntre0e17Anos();

            Assert.False(segurado.Valido);
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
