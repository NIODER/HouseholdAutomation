using HouseholdAutomationDesktop.ViewModel;
using HouseholdAutomationDesktop.ViewModel.DialogsViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;

namespace HouseholdAutomationDesktop
{
    public class Locator
    {
        public MainWindowViewModel MainWindowViewModel => App.host.Services.GetRequiredService<MainWindowViewModel>();
        public ClientsViewModel ClientsViewModel => App.host.Services.GetRequiredService<ClientsViewModel>();
        public ResourcesViewModel ResourcesViewModel => App.host.Services.GetRequiredService<ResourcesViewModel>();
        public OrdersViewModel OrdersViewModel => App.host.Services.GetRequiredService<OrdersViewModel>();
        public ProvidersViewModel ProvidersViewModel => App.host.Services.GetRequiredService<ProvidersViewModel>();
        public CreateResourceViewModel CreateResourceViewModel => App.host.Services.GetRequiredService<CreateResourceViewModel>();
        public AddProviderViewModel AddProviderViewModel => App.host.Services.GetRequiredService<AddProviderViewModel>();
        public AddResourceToProviderViewModel AddResourceToProviderViewModel => App.host.Services.GetRequiredService<AddResourceToProviderViewModel>();
        public SelectResourceViewModel SelectResourceViewModel => App.host.Services.GetRequiredService<SelectResourceViewModel>();
        public SaveChangesWarningViewModel SaveChangesWarningViewModel => App.host.Services.GetRequiredService<SaveChangesWarningViewModel>();
    }
}
