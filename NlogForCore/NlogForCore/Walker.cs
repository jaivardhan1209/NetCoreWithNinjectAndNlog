using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace NlogForCore
{
    public class Walker
    {
        public readonly ILogger _logger = ServiceLocator.GetServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger("walkerlog");

        public void DoAction()
        {
           // var serviceProvider = 
            _logger.LogTrace(20, $"I love walking baby {_logger.GetHashCode()}");
        }
    }
}
