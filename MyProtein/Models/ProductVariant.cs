using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class ProductVariant
{
    public int VariantId { get; set; }

    public int ProductId { get; set; }

    public int? WeightId { get; set; }

    public int? FlavourId { get; set; }

    public int? Price { get; set; }

    public string? ImageUrl { get; set; }

    public int StockQuantity { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Flavour? Flavour { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product Product { get; set; } = null!;

    public virtual Weight? Weight { get; set; }
}
