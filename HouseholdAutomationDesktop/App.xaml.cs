using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Data.DbEntityRedactors;
using AutomationHouseholdDatabase.Models;
using HouseholdAutomationDesktop.Model;
using HouseholdAutomationDesktop.Model.DbEntityRedactors;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.View;
using HouseholdAutomationDesktop.View.Dialogs;
using HouseholdAutomationDesktop.ViewModel;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using HouseholdAutomationLogic;
using HouseholdAutomationLogic.BLL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Windows;

namespace HouseholdAutomationDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((System.Action<IServiceCollection>)(services =>
            {
                services.AddSingleton<ILogger, DebugLogger>();
                services.AddTransient<HouseholdDbContext>(s => new(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString));
                services.AddSingleton<Locator>();

                services.AddTransient<IWindowPresenter, WindowPresenter>();
                ServiceCollectionServiceExtensions.AddTransient<IRedactor<Client>, ClientsRedactor>(services);
                ServiceCollectionServiceExtensions.AddTransient<IRedactor<Order>, OrdersRedactor>(services);
                ServiceCollectionServiceExtensions.AddTransient<IRedactor<OrdersToResource>, OrderToResourcesRedactor>(services);
                ServiceCollectionServiceExtensions.AddTransient<IRedactor<ProviderToResource>, ProviderToResourcesRedactor>(services);
                ServiceCollectionServiceExtensions.AddTransient<IRedactor<Provider>, ProvidersRedactor>(services);
                ServiceCollectionServiceExtensions.AddTransient<IRedactor<Resource>, ResourcesRedactor>(services);

                services.AddTransient<ClientsBLL>();
                services.AddTransient<OrdersBLL>();
                services.AddTransient<ProviderBLL>();
                services.AddTransient<ResourceBLL>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<ClientsView>();
                services.AddSingleton<ResourcesView>();
                services.AddSingleton<OrdersView>();
                services.AddSingleton<ProvidersView>();
                services.AddTransient<CreateResourceWindow>();
                services.AddTransient<AddProviderWindow>();
                services.AddTransient<AddResourceToProviderWindow>();
                services.AddTransient<SelectResourceWindow>();
                services.AddTransient<ChangesSavingWarningWindow>();
                services.AddTransient<ChoseClientWindow>();

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<ClientsViewModel>();
                services.AddSingleton<ResourcesViewModel>();
                services.AddSingleton<OrdersViewModel>();
                services.AddSingleton<ProvidersViewModel>();
                services.AddTransient<CreateResourceViewModel>();
                services.AddTransient<AddProviderViewModel>();
                services.AddTransient<AddResourceToProviderViewModel>();
                services.AddTransient<SelectResourceViewModel>();
                services.AddTransient<SaveChangesWarningViewModel>();
                services.AddTransient<ChoseClientViewModel>();

            })).Build();

        protected override void OnStartup(StartupEventArgs e)
        {
            host.Start();
            MainWindow = host.Services.GetRequiredService<MainWindow>();
            RegisterDialogs();
            base.OnStartup(e);
        }

        private static void RegisterDialogs()
        {
            WindowPresenter.Register<CreateResourceWindow, CreateResourceViewModel>();
            WindowPresenter.Register<SelectResourceWindow, SelectResourceViewModel>();
            WindowPresenter.Register<ChangesSavingWarningWindow, SaveChangesWarningViewModel>();
            WindowPresenter.Register<ChoseClientWindow, ChoseClientViewModel>();
            WindowPresenter.Register<AddResourceToProviderWindow, AddResourceToProviderViewModel>();
            WindowPresenter.Register<AddProviderWindow, AddProviderViewModel>();
        }
    }
}
