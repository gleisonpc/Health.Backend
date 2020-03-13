using Health.Backend.Domain.Entities;
using System.Threading.Tasks;

namespace Health.Backend.Domain.Repositories.Interfaces
{
    public interface ICidadeRepository
    {
        Task<CidadesEntity> ObterCidades();
    }
}
