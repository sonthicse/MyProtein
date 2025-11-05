using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class DeliveryType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Fee { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
