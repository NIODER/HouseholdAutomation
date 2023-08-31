using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseholdAutomationDesktop.Model.DbEntityRedactors
{
    public class ResourcesRedactor : Redactor<Resource>
    {
        public ResourcesRedactor(HouseholdDbContext db) : base(db)
        {
        }

        public override IEnumerable<Resource> GetAll()
        {
            return db.Resources
                .Include(r => r.ProviderToResources)
                .ToList();
        }

        public override IEnumerable<Resource> GetByPredicate(Func<Resource, bool> predicate)
        {
            return db.Resources
                .Include(r => r.ProviderToResources)
                .Where(predicate)
                .ToList();
        }
    }
}
