using Health.Backend.Domain.Models.Requests;
using Health.Backend.Domain.Models.Responses;

namespace Health.Backend.Domain.Services.Interfaces
{
    public interface IPrecoService
    {
        PrecoModel ObterPrecoParaSegurado(SeguradoModel segurado);
    }
}
