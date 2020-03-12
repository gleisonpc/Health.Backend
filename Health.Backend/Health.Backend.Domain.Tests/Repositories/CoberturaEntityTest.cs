using Health.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace Health.Backend.Domain.Tests.Repositories
{
    
    public class CoberturaEntityTest
    {
        [Fact]
        public void Validar_Propriedades_Entidade() =>
            Assert.True(typeof(CoberturaEntity).GetProperties().Count() == 5);
    }
}
