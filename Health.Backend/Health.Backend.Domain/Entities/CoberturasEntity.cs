using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Entities
{
    public class CoberturaEntity
    {
        [Index(0)]
        public int Id { get; set; }

        [Index(1)]
        public string Nome { get; set; }

        [Index(2)]
        public double Premio { get; set; }

        [Index(3)]
        public double Valor { get; set; }

        [Index(4)]
        public string Principal { get; set; }
    }
}
