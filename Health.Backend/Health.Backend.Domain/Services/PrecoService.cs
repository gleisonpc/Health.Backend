using Health.Backend.Domain.Models.Requests;
using Health.Backend.Domain.Models.Responses;
using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Health.Backend.Domain.Services
{
    public class PrecoService : IPrecoService
    {
        private readonly ICoberturaRepository _coberturasRepository;

        public PrecoService(ICoberturaRepository coberturaRepository) =>
            _coberturasRepository = coberturaRepository;

        public PrecoModel ObterPrecoParaSegurado(SeguradoModel segurado)
        {
            var coberturas = _coberturasRepository.ObterCoberturas();
            var subTotal = coberturas.Where(x => segurado.Coberturas.Any(id => id == x.Id)).Sum(x => x.Valor);
            throw new NotImplementedException();
        }
    }
}
