using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public int ShippingAddressId { get; set; }

    public int PaymentMethodId { get; set; }

    public int ShippingMethodId { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual Address ShippingAddress { get; set; } = null!;

    public virtual ShippingMethod ShippingMethod { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
