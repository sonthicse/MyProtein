using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class Flavour
{
    public int FlavourId { get; set; }

    public string FlavourName { get; set; } = null!;

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
