using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseholdAutomationDesktop.Model.DbEntityRedactors
{
    internal class ProviderDbEntityRedactor : DbEntityRedactor<Provider>
    {
        public ProviderDbEntityRedactor(HouseholdDbContext db) : base(db)
        {
        }

        public override IEnumerable<Provider> GetAll()
        {
            return db.Providers
                .Include(p => p.ProviderToResources)
                .ToList();
        }

        public override IEnumerable<Provider> GetByPredicate(Func<Provider, bool> predicate)
        {
            return db.Providers
                .Include(p => p.ProviderToResources)
                .Where(predicate)
                .ToList();
        }
    }
}
