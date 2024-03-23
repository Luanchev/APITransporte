using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Transporte.Core.Interfaces.ICamion;
using Transporte.Core.Interfaces.IUser;
using Transporte.Infrastructure.Repositories;
using Transporte.Infrastructure.Repositories.CamionRepositories;
using Transporte.Infrastructure.Repositories.UserRepositories;

namespace APITransporte.Extensions
{
    public static class StartupTrasient
    {
        public static void configureTrasient(this IServiceCollection service) 
        {
            service.AddTransient<ICamion, CamionRepository>();
            service.AddTransient<IUser, UserRepository>();
        }
    }
}
