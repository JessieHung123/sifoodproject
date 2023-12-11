using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public string OrderId { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
