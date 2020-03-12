namespace Health.Backend.Domain.Models
{
    public class EnderecoModel : ModelBase
    {
        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }
    }
}
