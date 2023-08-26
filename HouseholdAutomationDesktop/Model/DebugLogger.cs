using System;
using System.Diagnostics;

namespace HouseholdAutomationDesktop.Model
{
    public class DebugLogger : ILogger
    {
        public void Log<T>(LogMessage logMessage)
        {
            Debug.WriteLine($"[{DateTime.Now}] {typeof(T).Name} {logMessage.Severety}: {logMessage.Message}");
            if (logMessage.InnerException != null)
            {
                Debug.WriteLine(logMessage.InnerException.ToString());
            }
        }

        public void Log<T>(LogSeverety severety, string message, Exception? innerException = null)
        {
            Log<T>(new(severety, message, innerException));
        }
    }
}
