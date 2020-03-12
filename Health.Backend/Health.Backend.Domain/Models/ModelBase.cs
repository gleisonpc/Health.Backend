using System.Collections.Generic;

namespace Health.Backend.Domain.Models
{
    public class ModelBase
    {
        public ModelBase() => Erros = new List<string>();

        public virtual bool Valido
        {
            get
            {
                return !(Erros.Count > 0);
            }
        }
        public IList<string> Erros { get; private set; }

        public void AdicionarErro(string erro) => Erros.Add(erro);
    }
}
