using CsvHelper;
using Health.Backend.Domain.Entities;
using Health.Backend.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Health.Backend.Repository.Repositories
{
    public class CoberturaRepository : ICoberturaRepository
    {
        public IEnumerable<CoberturaEntity> ObterCoberturas()
        {
            using (var reader = new StreamReader("coberturas_dataset.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CoberturaEntity>();
                return records.ToList();
            }
        }
    }
}
