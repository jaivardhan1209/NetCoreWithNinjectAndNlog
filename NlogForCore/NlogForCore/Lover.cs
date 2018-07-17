using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NlogForCore
{
    public class Lover
    {
        private readonly ILogger<Lover> _logger;

        public Lover(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Lover>();
        }

        public void DoAction()
        {
            _logger.LogDebug(20, $"Lover class {_logger.GetHashCode()}");
        }
    }
}
