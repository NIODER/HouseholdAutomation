using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseholdAutomationDesktop.Model.DbEntityRedactors
{
    internal class ProviderToResourceDbEntityRedactor : DbEntityRedactor<ProviderToResource>
    {
        public ProviderToResourceDbEntityRedactor(HouseholdDbContext db) : base(db)
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
