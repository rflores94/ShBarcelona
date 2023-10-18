using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using ShBarcelona.Services.Area;

namespace ShBarcelona.Services
{
    public static class Bootstrap
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            services.AddTransient<IAreaService, AreaService>();

            services.AddTransient<HttpClient>();
            services.AddLogging();
            services.AddSingleton(typeof(ILogger), _logger);

            services.AddMemoryCache();

        }
    }
}

