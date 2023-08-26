namespace HouseholdAutomationLogic
{
    public interface IRedactorFactory
    {
        public IRedactor<T> Create<T>() where T : class;
    }
}
