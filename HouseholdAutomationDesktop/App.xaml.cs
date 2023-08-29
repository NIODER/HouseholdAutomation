using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using HouseholdAutomationDesktop.Model;
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
            .ConfigureServices(services =>
            {
                services.AddSingleton<ILogger, DebugLogger>();
                services.AddTransient<HouseholdDbContext>(s => new(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString));
                services.AddSingleton<Locator>();

                services.AddTransient<IWindowPresenter, WindowPresenter>();
                services.AddTransient<IDbEntityRedactor<Client>, DbEntityRedactor<Client>>();
                services.AddTransient<IDbEntityRedactor<Order>, DbEntityRedactor<Order>>();
                services.AddTransient<IDbEntityRedactor<OrdersToResource>, DbEntityRedactor<OrdersToResource>>();
                services.AddTransient<IDbEntityRedactor<ProviderToResource>, DbEntityRedactor<ProviderToResource>>();
                services.AddTransient<IDbEntityRedactor<Provider>, DbEntityRedactor<Provider>>();
                services.AddTransient<IDbEntityRedactor<Resource>, DbEntityRedactor<Resource>>();

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

            }).Build();

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
