using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseholdAutomationDesktop.Model.DbEntityRedactors
{
    internal class ClientDbEntityRedactor : DbEntityRedactor<Client>
    {
        public ClientDbEntityRedactor(HouseholdDbContext db) : base(db)
        {
        }

        public override IEnumerable<Client> GetAll()
        {
            return db.Clients
                .Include(c => c.Orders)
                .ToList();
        }

        public override IEnumerable<Client> GetByPredicate(Func<Client, bool> predicate)
        {
            return db.Clients
                .Include(c => c.Orders)
                .Where(predicate)
                .ToList();
        }
    }
}
