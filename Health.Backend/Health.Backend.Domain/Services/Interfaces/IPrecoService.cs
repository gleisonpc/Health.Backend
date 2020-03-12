using Health.Backend.Domain.Models.Requests;
using Health.Backend.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Services.Interfaces
{
    public interface IPrecoService
    {
        public PrecoModel ObterPrecoParaSegurado(SeguradoModel segurado);
    }
}
