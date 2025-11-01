using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class Weight
{
    public int WeightId { get; set; }

    public double WeightValue { get; set; }

    public int? Servings { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
