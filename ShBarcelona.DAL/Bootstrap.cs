using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ShBarcelona.DAL
{
    public static class Bootstrap
    {
        public static void AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RecruitmentContext>(options =>
            options.UseMySql(configuration.GetConnectionString("ShBarcelona"),
                             ServerVersion.AutoDetect(configuration.GetConnectionString("ShBarcelona")),
                             x => x.MigrationsAssembly("ShBarcelona.DAL")));
        }
    }
}
