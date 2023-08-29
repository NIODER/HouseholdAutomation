using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace AutomationHouseholdDatabase.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PassHash { get; set; } = null!;
}
