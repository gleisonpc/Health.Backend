﻿using Health.Backend.Domain.Entities;
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

        public double Premio
        {
            get
            {
                return SubTotal + Acrescimo - Desconto;
            }
        }
        public int Parcelas { get; set; }
        public double ValorParcelas { get; set; }
        public DateTime PrimeiroVencimento { get; set; }
        public double CoberturaTotal { get; set; }

        [JsonIgnore]
        public double SubTotal { get; private set; }

        [JsonIgnore]
        public double Acrescimo { get; private set; }

        [JsonIgnore]
        public double Desconto { get; private set; }

        public static PrecoModel CriarPrecoModel(SeguradoModel segurado, IEnumerable<CoberturaEntity> coberturas)
        {
            var subTotal = coberturas.Where(x => segurado.Coberturas.Any(id => id == x.Id)).Sum(x => x.Valor);

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


    }
}
