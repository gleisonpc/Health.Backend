using Health.Backend.Domain.Constants;
using Health.Backend.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Health.Backend.Domain.Models.Requests
{
    public class SeguradoModel : PessoaModel
    {
        private readonly ICidadeRepository _cidadeRepository;
        private readonly ICoberturaRepository _coberturaRepository;

        public SeguradoModel(ICidadeRepository cidadeRepository, ICoberturaRepository coberturaRepository)
        {
            _cidadeRepository = cidadeRepository;
            _coberturaRepository = coberturaRepository;
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
                return ValidoIdade && ValidoCidade && ValidoCep;
            }
        }

        public bool ValidoCep
        {
            get
            {
                return ValidarCep();
            }
        }

        public bool ValidoCidade
        {
            get
            {
                return ValidarCidade();
            }
        }

        public bool ValidoCoberturaObrigatoria
        {
            get
            {
                var coberturas = _coberturaRepository.ObterCoberturas();
                var minhasCoberturas = coberturas.Where(x => Coberturas.Equals(x.Id));
                var temObrigatorio = minhasCoberturas.Any(x => x.Principal == "S");
                return temObrigatorio;
            }
        }

        public bool ValidoIdade
        {
            get
            {
                return ValidarIdade();
            }
        }

        private int CacularIdade()
        {
            var anoAtual = DateTime.Today;
            var idade = anoAtual.Year - Nascimento.Year;
            idade = anoAtual.Month < Nascimento.Month ? --idade : anoAtual.Day < Nascimento.Day ? --idade : idade;
            return idade;
        }

        private bool ValidarIdade()
        {
            if (Idade < 18)
            {
                AdicionarErro(MensagensErros.SEGURADO_COM_MENOS_DE_18_ANOS);
                return false;
            }

            return true;
        }

        private bool ValidarCep()
        {
            if (!Regex.IsMatch(Endereco.Cep, ("[0-9]{5}-[0-9]{3}")))
            {
                AdicionarErro(MensagensErros.CEP_FORA_DO_PADRA_PERMITIDO);
                return false;
            }

            return true;
        }

        private bool ValidarCidade()
        {
            var cidades = _cidadeRepository.ObterCidades().Result;
            if (!cidades.Cities.Any(x => x.Name.Equals(Endereco.Cidade)))
            {
                AdicionarErro(MensagensErros.CIDADE_NAO_ENCONTRADA);
                return false;
            }

            return true;
        }
    }
}
