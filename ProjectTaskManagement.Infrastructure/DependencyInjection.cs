using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Infrastructure.Data;
using ProjectTaskManagement.Infrastructure.Repository.Base;
using ProjectTaskManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectTaskManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastrctureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProjectTaskManagementConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IunitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IPasswordHasher, PasswordHasherService>();

        return services;
    }
}
