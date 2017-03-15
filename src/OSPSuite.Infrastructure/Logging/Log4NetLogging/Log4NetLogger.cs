using System;
using log4net;
using OSPSuite.Utility.Logging;

namespace OSPSuite.Infrastructure.Logging.Log4NetLogging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(ILog log)
        {
            _log = log;
        }

        public void Informational(string infoToLog)
        {
            _log.Info(infoToLog);
        }

        public void Error(Exception e)
        {
            _log.Error(e.ToString());
        }

        public void Debug(string debugInfo)
        {
            if (_log.IsDebugEnabled == false) return;
            _log.Debug(debugInfo);
        }
    }
}