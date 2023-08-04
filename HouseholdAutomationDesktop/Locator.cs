using HouseholdAutomationDesktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace HouseholdAutomationDesktop
{
    public class Locator
    {
        public MainWindowViewModel MainWindowViewModel => App.host.Services.GetRequiredService<MainWindowViewModel>();
        public ClientsViewModel ClientsViewModel => App.host.Services.GetRequiredService<ClientsViewModel>();
        public ResourcesViewModel ResourcesViewModel => App.host.Services.GetRequiredService<ResourcesViewModel>();
        public OrdersViewModel OrdersViewModel => App.host.Services.GetRequiredService<OrdersViewModel>();
        public ProvidersViewModel ProvidersViewModel => App.host.Services.GetRequiredService<ProvidersViewModel>();
    }
}
