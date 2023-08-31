
using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdAutomationDesktop.Model.DbEntityRedactors
{
    public class ProviderToResourcesRedactor : Redactor<ProviderToResource>
    {
        public ProviderToResourcesRedactor(HouseholdDbContext db) : base(db)
        {
        }

        public override IEnumerable<ProviderToResource> GetAll()
        {
            return db.ProviderToResources
                .Include(pr => pr.Provider)
                .Include(pr => pr.Resource)
                .ToList();
        }

        public override IEnumerable<ProviderToResource> GetByPredicate(Func<ProviderToResource, bool> predicate)
        {
            return db.ProviderToResources
                .Include(pr => pr.Provider)
                .Include(pr => pr.Resource)
                .Where(predicate)
                .ToList();
        }
    }
}
