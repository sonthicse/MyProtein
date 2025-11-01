using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public string? City { get; set; }

    public string? StateProvince { get; set; }

    public string? PostalCode { get; set; }

    public string Phone { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
