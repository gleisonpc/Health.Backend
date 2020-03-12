using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Models
{
    public class EnderecoModel
    {
        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }
    }
}
