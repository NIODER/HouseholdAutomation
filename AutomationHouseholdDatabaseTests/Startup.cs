using AutomationHouseholdDatabase.Data.DbEntityRedactors;
using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using HouseholdAutomationDesktop.Model.DbEntityRedactors;
using HouseholdAutomationLogic.BLL;
using HouseholdAutomationLogic;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AutomationHouseholdDatabaseTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<HouseholdDbContext>(s => new("Host=localhost;Port=5432;Database=household_db_tests;Username=householder;Password=householder123;IncludeErrorDetail=true"));

            services.AddTransient<IRedactor<Client>, ClientsRedactor>();
            services.AddTransient<IRedactor<Order>, OrdersRedactor>();
            services.AddTransient<IRedactor<OrdersToResource>, OrderToResourcesRedactor>();
            services.AddTransient<IRedactor<ProviderToResource>, ProviderToResourcesRedactor>();
            services.AddTransient<IRedactor<Provider>, ProvidersRedactor>();
            services.AddTransient<IRedactor<Resource>, ResourcesRedactor>();

        }
    }
}
