using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NlogForCore
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;
        //private readonly ILoggerFactory loggerFactory;
        public Runner(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Runner>();
        }

        public void DoAction(string name)
        {
            _logger.LogDebug(20, $"Doing hard work! ${name} : {_logger.GetHashCode()}");
        }

    }
}
