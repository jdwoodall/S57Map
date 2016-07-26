using Common.Logging;
using System;

namespace SharpMap_OGR_Test
{
    partial class LogMessages
    {
        internal bool isInitialized;
        private static ILog log;

        public LogMessages()
        {
            log = LogManager.GetLogger<SharpMapOGRTest>();
            isInitialized = true;
        }

        protected static void LogTraceMessage(String message)
        {
            log.Trace(message);
        }

        protected static void LogDebugMessage(String message)
        {
            log.Debug(message);
        }

        public void LogInfoMessage(String message)
        {
            log.Info(message);
        }

        protected static void LogWarningMessage(String message)
        {
            log.Warn(message);
        }

        private static void LogErrorMessage(String message)
        {
            log.Error(message);
        }

        private static void LogFatalErrorMessage(String message)
        {
            log.Fatal(message);
        }
    }
}