using Health.Backend.Domain.Repositories.Interfaces;
using Health.Backend.Domain.Services;
using Health.Backend.Domain.Services.Interfaces;
using Health.Backend.Repository.API.Repositories;
using Health.Backend.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Backend.App.IoC
{
    public static class IoC
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICidadeRepository, CidaddeRepository>();
            services.AddScoped<ICoberturaRepository, CoberturaRepository>();

            services.AddScoped<IPrecoService, PrecoService>();
        }
    }
}
