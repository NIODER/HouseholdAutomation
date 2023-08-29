using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace AutomationHouseholdDatabase.Data
{
    public interface IHouseholdDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Client> Clients { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrdersToResource> OrdersToResources { get; set; }
        DbSet<Provider> Providers { get; set; }
        DbSet<ProviderToResource> ProviderToResources { get; set; }
        DbSet<Resource> Resources { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void ClearChanges();
    }
}