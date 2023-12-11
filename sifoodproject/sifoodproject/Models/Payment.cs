using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class Payment
{
    public string OrderId { get; set; } = null!;

    public string PaymentMethodＮame { get; set; } = null!;

    public string PaymentStatusＮame { get; set; } = null!;

    public DateTime PaymentTime { get; set; }

    public virtual Order Order { get; set; } = null!;
}
