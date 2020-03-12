using Health.Backend.Domain.Constants;
using System;
using System.Collections.Generic;

namespace Health.Backend.Domain.Models.Requests
{
    public class SeguradoModel : PessoaModel
    {
        public SeguradoModel()
        {
            ValidarIdade();
        }

        public IEnumerable<int> Coberturas { get; set; }

        public int Idade
        {
            get
            {
                return CacularIdade();
            }
        }

        public override bool Valido
        {
            get
            {
                ValidarIdade();
                return !(Erros.Count > 0);
            }
        }

        private int CacularIdade()
        {
            var anoAtual = DateTime.Today;
            var idade = anoAtual.Year - Nascimento.Year;
            idade = anoAtual.Month < Nascimento.Month ? --idade : anoAtual.Day < Nascimento.Day ? --idade : idade;
            return idade;
        }

        private void ValidarIdade()
        {
            if (Idade < 18)
                AdicionarErro(MensagensErros.SEGURADO_COM_MENOS_DE_18_ANOS);
        }
    }
}
