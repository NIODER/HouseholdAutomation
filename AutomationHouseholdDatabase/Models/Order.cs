﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomationHouseholdDatabase.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public int ClientId { get; set; }

    public DateOnly OrderDate { get; set; }

    public virtual Client Client { get; set; } = null!;
}
