using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VotingSystem.Application.Interfaces;
using VotingSystem.Application.Services;
using VotingSystem.Data;
using VotingSystem.Data.Repositories;
using VotingSystem.Domain.Interfaces;
using VotingSystem.Infra.Data.Repositories;

namespace VotingSystem.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração PostgreSQL
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });


            //Repositories
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();


            //Services
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService, ResultService>();

            //Cache
            services.AddMemoryCache();

            return services;
        }
    }
}
