using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class Cart
{
    public string UserId { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
