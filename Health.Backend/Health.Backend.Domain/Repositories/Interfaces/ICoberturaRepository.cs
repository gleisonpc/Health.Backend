using Health.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Health.Backend.Domain.Repositories.Interfaces
{
    public interface ICoberturaRepository
    {
        IEnumerable<CoberturaEntity> ObterCoberturas();
    }
}
