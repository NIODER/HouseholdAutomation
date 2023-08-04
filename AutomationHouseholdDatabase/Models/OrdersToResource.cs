using System;
using System.Collections.Generic;

namespace AutomationHouseholdDatabase.Models;

public partial class OrdersToResource
{
    public long OrderId { get; set; }

    public long ResourceId { get; set; }

    public int Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Resource Resource { get; set; } = null!;
}
