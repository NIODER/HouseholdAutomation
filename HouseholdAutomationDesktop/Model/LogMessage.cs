using System;

namespace HouseholdAutomationDesktop.Model
{
    public class LogMessage
    {
        public LogSeverety Severety { get; set; } 
        public string Message { get; set; }
        public Exception? InnerException { get; set; }

        public LogMessage(LogSeverety severety, string message, Exception? innerException = null)
        {
            Severety = severety;
            Message = message;
            InnerException = innerException;
        }
    }
}
