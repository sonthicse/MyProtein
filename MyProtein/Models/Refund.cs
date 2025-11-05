using System;
using System.Collections.Generic;

namespace MyProtein.Models;

public partial class Refund
{
    public int RefundId { get; set; }

    public int OrderId { get; set; }

    public string Reason { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
