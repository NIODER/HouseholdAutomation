using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdAutomationDesktop.Model.DbEntityRedactors
{
    public class ClientsRedactor : Redactor<Client>
    {
        public ClientsRedactor(HouseholdDbContext db) : base(db)
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
