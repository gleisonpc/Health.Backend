using Health.Backend.Domain.Models.Requests;
using Health.Backend.Domain.Models.Responses;
using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Services.Interfaces;
using System;

namespace Health.Backend.Domain.Services
{
    public class PrecoService : IPrecoService
    {
        private readonly ICoberturaRepository _coberturasRepository;
        private readonly ICidadeRepository _cidadeRepository;

        public PrecoService(ICoberturaRepository coberturaRepository, ICidadeRepository cidadeRepository)
        {
            _cidadeRepository = cidadeRepository;
            _coberturasRepository = coberturaRepository;
        }

        public PrecoModel ObterPrecoParaSegurado(SeguradoModel segurado)
        {
            try
            {
                if (segurado.Valido(_cidadeRepository, _coberturasRepository))
                    return PrecoModel.CriarPrecoModel(segurado, _coberturasRepository.ObterCoberturas(), DateTime.Now);
                else
                    return PrecoModel.CriarPrecoModelComErros(segurado);
            }
            catch (Exception ex)
            {
                return PrecoModel.CriarPrecoModelComErro(ex.Message);
            }
        }
    }
}
