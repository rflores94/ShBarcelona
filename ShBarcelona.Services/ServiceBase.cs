using NLog;

namespace TTecno.TalentHR.Services
{
    public abstract class ServiceBase
    {
        protected readonly ILogger _logger;

        protected ServiceBase(ILogger logger)
        {
            _logger = logger;
        }

    }
}
