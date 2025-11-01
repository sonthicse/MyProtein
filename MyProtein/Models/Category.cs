using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public bool? Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
