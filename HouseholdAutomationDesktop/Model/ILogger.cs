using System;

namespace HouseholdAutomationDesktop.Model
{
    public interface ILogger
    {
        void Log<T>(LogMessage logMessage);
        void Log<T>(LogSeverety severety, string message, Exception? innerException = null);
    }
}
