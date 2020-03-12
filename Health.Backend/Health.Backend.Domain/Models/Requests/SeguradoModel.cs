using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Models.Requests
{
    public class SeguradoModel : PessoaModel
    {
        public IEnumerable<int> Coberturas { get; set; }
    }
}
