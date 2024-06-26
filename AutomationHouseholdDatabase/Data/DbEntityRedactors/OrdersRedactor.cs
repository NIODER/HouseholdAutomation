﻿using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseholdAutomationDesktop.Model.DbEntityRedactors
{
    public class OrdersRedactor : Redactor<Order>
    {
        public OrdersRedactor(HouseholdDbContext db) : base(db)
        {
        }

        public override IEnumerable<Order> GetAll()
        {
            return db.Orders
                .Include(o => o.Client)
                .ToList();
        }

        public override IEnumerable<Order> GetByPredicate(Func<Order, bool> predicate)
        {
            return db.Orders
                .Include(o => o.Client)
                .Where(predicate)
                .ToList();
        }
    }
}
