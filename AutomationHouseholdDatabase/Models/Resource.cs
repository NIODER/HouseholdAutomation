using System;
using System.Collections.Generic;

namespace AutomationHouseholdDatabase.Models;

public partial class Resource
{
    public long ResourceId { get; set; }

    public string ResourceName { get; set; } = null!;

    public virtual ICollection<ProviderToResource> ProviderToResources { get; set; } = new List<ProviderToResource>();
}
