using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class Driver
{
    public string DriveId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PlateNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
