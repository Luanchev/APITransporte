using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Transporte.Core.Interfaces.ICamion;
using Transporte.Infrastructure.Repositories;
using Transporte.Infrastructure.Repositories.CamionRepositories;

namespace APITransporte.Extensions
{
    public static class StartupTrasient
    {
        public static void configureTrasient(this IServiceCollection service) 
        {
            service.AddTransient<ICamion, CamionRepository>();
        }
    }
}
