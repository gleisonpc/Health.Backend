using Health.Backend.Domain.Entities;
using Health.Backend.Domain.Models.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Health.Backend.Domain.Models.Responses
{
    public class PrecoModel
    {
        private const int IDADE_MAX_ACRESCIMO = 30;
        private const int PORCENTAGEM_ACRESCIMO = 8;
        private const int IDADE_MIN_DESCONTO = 30;
        private const int IDADE_MAX_DESCONTO = 45;

        private static DateTime _dataBase;

        public double Premio
        {
            get
            {
                return SubTotal + Acrescimo - Desconto;
            }
        }
        public int Parcelas
        {
            get
            {
                return CalcularParcela();
            }
        }
        public double ValorParcelas
        {
            get
            {
                return Premio / Parcelas;
            }
        }
        public DateTime PrimeiroVencimento
        {
            get
            {
                return CalcularPrimeiroVencimento();
            }
        }

        public double CoberturaTotal { get; set; }

        [JsonIgnore]
        public double SubTotal { get; private set; }

        [JsonIgnore]
        public double Acrescimo { get; private set; }

        [JsonIgnore]
        public double Desconto { get; private set; }

        public static PrecoModel CriarPrecoModel(SeguradoModel segurado, IEnumerable<CoberturaEntity> coberturas, DateTime dataSimulacao)
        {
            _dataBase = dataSimulacao;
            var subTotal = coberturas.Where(x => segurado.Coberturas.Any(id => id == x.Id)).Sum(x => x.Premio);

            return new PrecoModel
            {
                SubTotal = subTotal,
                Acrescimo = (CalcularAcrescimoPorcentagem(segurado.Idade) / 100) * subTotal,
                Desconto = (CalcularDescontoPorcentagem(segurado.Idade) / 100) * subTotal
            };
        }

        private static double CalcularAcrescimoPorcentagem(int idade) => (IDADE_MAX_ACRESCIMO - idade) * PORCENTAGEM_ACRESCIMO;

        private static double CalcularDescontoPorcentagem(int idade) =>
            idade > IDADE_MIN_DESCONTO && idade <= IDADE_MAX_DESCONTO ? (idade - IDADE_MIN_DESCONTO) * 2 : 0.00;

        private int CalcularParcela()
        {
            if (Premio > 0 && Premio <= 500)
                return 1;

            if (Premio > 500 && Premio <= 1000)
                return 2;

            if (Premio > 1000 && Premio <= 2000)
                return 3;

            if (Premio > 2000)
                return 4;

            return 0;
        }

        private static DateTime CalcularPrimeiroVencimento()
        {
            var data = _dataBase.AddMonths(1);
            var dataVencimento = new DateTime(data.Year, data.Month, 1);

            switch (dataVencimento.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dataVencimento = dataVencimento.AddDays(4);
                    break;

                case DayOfWeek.Tuesday:
                    dataVencimento = dataVencimento.AddDays(6);
                    break;

                case DayOfWeek.Wednesday:
                    dataVencimento = dataVencimento.AddDays(6);
                    break;

                case DayOfWeek.Thursday:
                    dataVencimento = dataVencimento.AddDays(6);
                    break;

                case DayOfWeek.Friday:
                    dataVencimento = dataVencimento.AddDays(6);
                    break;

                case DayOfWeek.Saturday:
                    dataVencimento = dataVencimento.AddDays(6);
                    break;

                case DayOfWeek.Sunday:
                    dataVencimento = dataVencimento.AddDays(5);
                    break;
            }

            return dataVencimento;
        }
    }
}
