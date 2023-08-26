using AutomationHouseholdDatabase.Data;
using HouseholdAutomationDesktop.Model;
using HouseholdAutomationDesktop.View;
using HouseholdAutomationDesktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Configuration;
using HouseholdAutomationLogic;
using HouseholdAutomationDesktop.Utils;
using HouseholdAutomationDesktop.View.Dialogs;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;

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
                services.AddTransient<IRedactorFactory, RedactorFactory>();
                services.AddTransient<IWindowPresenter, WindowPresenter>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<ClientsView>();
                services.AddSingleton<ResourcesView>();
                services.AddSingleton<OrdersView>();
                services.AddSingleton<ProvidersView>();

                services.AddTransient<CreateResourceViewModel>();
                services.AddTransient<CreateResourceWindow>();
                services.AddTransient<AddProviderViewModel>();
                services.AddTransient<AddProviderWindow>();
                services.AddTransient<AddResourceToProviderViewModel>();
                services.AddTransient<AddResourceToProviderWindow>();
                services.AddTransient<SelectResourceViewModel>();
                services.AddTransient<SelectResourceWindow>();
                services.AddTransient<SaveChangesWarningViewModel>();
                services.AddTransient<ChangesSavingWarningWindow>();

                services.AddSingleton<Locator>();

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<ClientsViewModel>();
                services.AddSingleton<ResourcesViewModel>();
                services.AddSingleton<OrdersViewModel>();
                services.AddSingleton<ProvidersViewModel>();

            }).Build();

        protected override void OnStartup(StartupEventArgs e)
        {
            host.Start();
            MainWindow = host.Services.GetRequiredService<MainWindow>();
            RegisterDialogs();
            base.OnStartup(e);
        }

        private void RegisterDialogs()
        {
            WindowPresenter.Register<CreateResourceWindow, CreateResourceViewModel>();
            WindowPresenter.Register<SelectResourceWindow, SelectResourceViewModel>();
            WindowPresenter.Register<ChangesSavingWarningWindow, SaveChangesWarningViewModel>();
        }
    }
}
