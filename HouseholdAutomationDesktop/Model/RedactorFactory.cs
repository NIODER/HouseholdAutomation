using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using HouseholdAutomationLogic;
using System;

namespace HouseholdAutomationDesktop.Model
{
    class RedactorFactory : IRedactorFactory
    {
        private readonly HouseholdDbContext _context;

        public RedactorFactory(HouseholdDbContext context)
        {
            _context = context;
        }

        public IRedactor<T> Create<T>() where T : class
        {
            return typeof(T).Name switch
            {
                nameof(Client) => new ClientsRedactor(_context) as IRedactor<T>,
                nameof(Resource) => new ResourcesRedactor(_context) as IRedactor<T>,
                nameof(Provider) => new ProviderRedactor(_context) as IRedactor<T>,
                nameof(Order) => new OrdersRedactor(_context) as IRedactor<T>,
                nameof(OrdersToResource) => new OrdersToResourcesRedactor(_context) as IRedactor<T>,
                _ => null
            } ?? throw new TypeUnloadedException($"There is no redactor for {typeof(T).FullName}.") ;
        }
    }
}
