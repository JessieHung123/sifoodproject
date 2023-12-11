using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class Comment
{
    public string OrderId { get; set; } = null!;

    public short CommentRank { get; set; }

    public DateTime CommentTime { get; set; }

    public string? Contents { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
