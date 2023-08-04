using AutomationHouseholdDatabase.Data;
using HouseholdAutomationDesktop.Model;
using HouseholdAutomationDesktop.View;
using HouseholdAutomationDesktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Configuration;

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
                services.AddSingleton<Locator>();
                services.AddTransient<HouseholdDbContext>(s => new(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString));

                services.AddSingleton<MainWindow>();
                services.AddSingleton<ClientsView>();
                services.AddSingleton<ResourcesView>();
                services.AddSingleton<OrdersView>();
                services.AddSingleton<ProvidersView>();

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
            base.OnStartup(e);
        }
    }
}
