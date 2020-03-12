using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Models
{
    public class PessoaModel
    {
        public string Nome { get; set; }

        public DateTime Nascimento { get; set; }

        public EnderecoModel Endereco { get; set; }
    }
}
