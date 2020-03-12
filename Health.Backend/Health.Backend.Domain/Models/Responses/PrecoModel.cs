using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Models.Responses
{
    public class PrecoModel
    {
        public double Premio { get; set; }
        public int Parcelas { get; set; }
        public double ValorParcelas { get; set; }
        public DateTime PrimeiroVencimento { get; set; }
        public double CoberturaTotal { get; set; }
    }
}
