using System;
using System.Collections.Generic;

namespace sifoodproject.Models;

public partial class Store
{
    public string StoreId { get; set; } = null!;

    public string StoreName { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string TaxId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Description { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public DateTime? EnrollDate { get; set; }

    public string OpeningTime { get; set; } = null!;

    public string ClosingDay { get; set; } = null!;

    public string? PhotosPath { get; set; }

    public string? LogoPath { get; set; }

    public string? PhotosPath2 { get; set; }

    public string? PhotosPath3 { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public int StoreIsAuthenticated { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
