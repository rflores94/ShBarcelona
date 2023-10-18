using AutoMapper;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using Microsoft.Extensions.Caching.Memory;

namespace TTecno.TalentHR.Services
{
    public static class Bootstrap
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            //services.AddTransient<IWorkCalendarService, WorkCalendarService>();
            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            services.AddSignalR();

            services.AddMemoryCache();

        }
    }
}

