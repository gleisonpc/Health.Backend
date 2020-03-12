using System;

namespace Health.Backend.Domain.Models
{
    public class PessoaModel : ModelBase
    {
        public string Nome { get; set; }

        public DateTime Nascimento { get; set; }

        public EnderecoModel Endereco { get; set; }
    }
}
