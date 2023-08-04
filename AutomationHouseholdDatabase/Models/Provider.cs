using System;
using System.Collections.Generic;

namespace AutomationHouseholdDatabase.Models;

public partial class Provider
{
    public int ProviderId { get; set; }

    public string? ProviderName { get; set; }

    public string Website { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<ProviderToResource> ProviderToResources { get; set; } = new List<ProviderToResource>();
}
