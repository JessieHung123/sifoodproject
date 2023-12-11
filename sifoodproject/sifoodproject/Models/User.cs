using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? UserName { get; set; }

    public string UserEmail { get; set; } = null!;

    public byte[] UserPasswordHash { get; set; } = null!;

    public string? UserPhone { get; set; }

    public DateTime? UserBirthDate { get; set; }

    public decimal? TotalOrderAmount { get; set; }

    public DateTime? UserEnrollDate { get; set; }

    public DateTime? UserLatestLogInDate { get; set; }

    public byte[]? UserPasswordSalt { get; set; }

    public int UserAuthenticated { get; set; }

    public string? UserVerificationCode { get; set; }

    public string? ForgotPasswordRandom { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
}
