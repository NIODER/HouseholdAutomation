using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationHouseholdDatabase.Data.DbEntityRedactors
{
    public class OrderToResourcesRedactor : Redactor<OrdersToResource>
    {
        public OrderToResourcesRedactor(HouseholdDbContext db) : base(db)
        {
        }

        public override IEnumerable<OrdersToResource> GetAll()
        {
            return db.OrdersToResources
                .Include(or => or.Order)
                .Include(or => or.Resource)
                .ToList();
        }

        public override IEnumerable<OrdersToResource> GetByPredicate(Func<OrdersToResource, bool> predicate)
        {
            return db.OrdersToResources
                .Include(or => or.Order)
                .Include(or => or.Resource)
                .Where(predicate)
                .ToList();
        }
    }
}
