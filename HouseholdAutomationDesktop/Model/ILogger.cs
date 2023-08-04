namespace HouseholdAutomationDesktop.Model
{
    public interface ILogger
    {
        void Log<T>(LogMessage logMessage);
    }
}
