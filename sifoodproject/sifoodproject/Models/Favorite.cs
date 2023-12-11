using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class Favorite
{
    public string UserId { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public int FavoriteId { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
