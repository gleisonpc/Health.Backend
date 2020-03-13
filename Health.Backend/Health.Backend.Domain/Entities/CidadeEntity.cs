using System.Collections.Generic;

namespace Health.Backend.Domain.Entities
{
    public class CidadesEntity
    {
        public IEnumerable<CidadeEntity> Cities { get; set; }
    }

    public class CidadeEntity
    {
        public string Name { get; set; }

        public string Uf { get; set; }

        public string Pais { get; set; }
    }
}
