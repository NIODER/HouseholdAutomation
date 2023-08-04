using System;
using System.Collections.Generic;

namespace AutomationHouseholdDatabase.Models;

public partial class ProviderToResource
{
    public int ProviderId { get; set; }

    public long ResourceId { get; set; }

    public int Cost { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual Resource Resource { get; set; } = null!;
}
