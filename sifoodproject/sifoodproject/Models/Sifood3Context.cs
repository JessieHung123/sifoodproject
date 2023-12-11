//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace sifoodproject.Models;

//public partial class Sifood3Context : DbContext
//{
//    public Sifood3Context()
//    {
//    }

//    public Sifood3Context(DbContextOptions<Sifood3Context> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Admin> Admins { get; set; }

//    public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; }

//    public virtual DbSet<Cart> Carts { get; set; }

//    public virtual DbSet<Category> Categories { get; set; }

//    public virtual DbSet<Comment> Comments { get; set; }

//    public virtual DbSet<Counter> Counters { get; set; }

//    public virtual DbSet<Driver> Drivers { get; set; }

//    public virtual DbSet<Favorite> Favorites { get; set; }

//    public virtual DbSet<Hash> Hashes { get; set; }

//    public virtual DbSet<Job> Jobs { get; set; }

//    public virtual DbSet<JobParameter> JobParameters { get; set; }

//    public virtual DbSet<JobQueue> JobQueues { get; set; }

//    public virtual DbSet<List> Lists { get; set; }

//    public virtual DbSet<Order> Orders { get; set; }

//    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

//    public virtual DbSet<Payment> Payments { get; set; }

//    public virtual DbSet<Product> Products { get; set; }

//    public virtual DbSet<Schema> Schemas { get; set; }

//    public virtual DbSet<Server> Servers { get; set; }

//    public virtual DbSet<Set> Sets { get; set; }

//    public virtual DbSet<State> States { get; set; }

//    public virtual DbSet<Status> Statuses { get; set; }

//    public virtual DbSet<Store> Stores { get; set; }

//    public virtual DbSet<User> Users { get; set; }

//    public virtual DbSet<UserAddress> UserAddresses { get; set; }


//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.UseCollation("Chinese_Taiwan_Stroke_CI_AS");

//        modelBuilder.Entity<Admin>(entity =>
//        {
//            entity
//                .HasNoKey()
//                .ToTable("Admin");

//            entity.Property(e => e.Account).HasMaxLength(30);
//            entity.Property(e => e.Id).ValueGeneratedOnAdd();
//            entity.Property(e => e.Name).HasMaxLength(30);
//            entity.Property(e => e.Password).HasMaxLength(64);
//            entity.Property(e => e.PasswordSalt).HasMaxLength(64);
//        });

//        modelBuilder.Entity<AggregatedCounter>(entity =>
//        {
//            entity.HasKey(e => e.Key).HasName("PK_HangFire_CounterAggregated");

//            entity.ToTable("AggregatedCounter", "HangFire");

//            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

//            entity.Property(e => e.Key).HasMaxLength(100);
//            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
//        });

//        modelBuilder.Entity<Cart>(entity =>
//        {
//            entity.HasKey(e => new { e.UserId, e.ProductId }).HasName("PK_Cart_1");

//            entity.ToTable("Cart");

//            entity.HasIndex(e => new { e.UserId, e.ProductId }, "Cart_Store_MustOne").IsUnique();

//            entity.Property(e => e.UserId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("UserID");
//            entity.Property(e => e.ProductId).HasColumnName("ProductID");

//            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
//                .HasForeignKey(d => d.ProductId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Cart_Products");

//            entity.HasOne(d => d.User).WithMany(p => p.Carts)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Cart_Users");
//        });

//        modelBuilder.Entity<Category>(entity =>
//        {
//            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B1A31B7A5");

//            entity.ToTable("Category");

//            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
//            entity.Property(e => e.CategoryName).HasMaxLength(6);
//        });

//        modelBuilder.Entity<Comment>(entity =>
//        {
//            entity.HasKey(e => e.OrderId);

//            entity.Property(e => e.OrderId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("OrderID");
//            entity.Property(e => e.CommentTime).HasColumnType("datetime");
//            entity.Property(e => e.Contents).HasMaxLength(255);
//            entity.Property(e => e.StoreId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("StoreID");

//            entity.HasOne(d => d.Order).WithOne(p => p.Comment)
//                .HasForeignKey<Comment>(d => d.OrderId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Comments_Orders");

//            entity.HasOne(d => d.Store).WithMany(p => p.Comments)
//                .HasForeignKey(d => d.StoreId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Comments_Stores");
//        });

//        modelBuilder.Entity<Counter>(entity =>
//        {
//            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_Counter");

//            entity.ToTable("Counter", "HangFire");

//            entity.Property(e => e.Key).HasMaxLength(100);
//            entity.Property(e => e.Id).ValueGeneratedOnAdd();
//            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
//        });

//        modelBuilder.Entity<Driver>(entity =>
//        {
//            entity.HasKey(e => e.DriveId).HasName("PK__Drivers__9610CA387C354DCE");

//            entity.Property(e => e.DriveId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasDefaultValueSql("('D'+format(NEXT VALUE FOR [SifoodDriverIdSeq],'000'))")
//                .HasColumnName("DriveID");
//            entity.Property(e => e.Email)
//                .HasMaxLength(64)
//                .IsUnicode(false);
//            entity.Property(e => e.FullName).HasMaxLength(10);
//            entity.Property(e => e.Phone)
//                .HasMaxLength(10)
//                .IsUnicode(false);
//            entity.Property(e => e.PlateNumber)
//                .HasMaxLength(10)
//                .IsUnicode(false);
//        });

//        modelBuilder.Entity<Favorite>(entity =>
//        {
//            entity.HasKey(e => new { e.UserId, e.StoreId });

//            entity.Property(e => e.UserId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("UserID");
//            entity.Property(e => e.StoreId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("StoreID");
//            entity.Property(e => e.FavoriteId)
//                .ValueGeneratedOnAdd()
//                .HasColumnName("FavoriteID");

//            entity.HasOne(d => d.Store).WithMany(p => p.Favorites)
//                .HasForeignKey(d => d.StoreId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Favorites_Stores1");

//            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Favorites_Users");
//        });

//        modelBuilder.Entity<Hash>(entity =>
//        {
//            entity.HasKey(e => new { e.Key, e.Field }).HasName("PK_HangFire_Hash");

//            entity.ToTable("Hash", "HangFire");

//            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

//            entity.Property(e => e.Key).HasMaxLength(100);
//            entity.Property(e => e.Field).HasMaxLength(100);
//        });

//        modelBuilder.Entity<Job>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK_HangFire_Job");

//            entity.ToTable("Job", "HangFire");

//            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

//            entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName").HasFilter("([StateName] IS NOT NULL)");

//            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
//            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
//            entity.Property(e => e.StateName).HasMaxLength(20);
//        });

//        modelBuilder.Entity<JobParameter>(entity =>
//        {
//            entity.HasKey(e => new { e.JobId, e.Name }).HasName("PK_HangFire_JobParameter");

//            entity.ToTable("JobParameter", "HangFire");

//            entity.Property(e => e.Name).HasMaxLength(40);

//            entity.HasOne(d => d.Job).WithMany(p => p.JobParameters)
//                .HasForeignKey(d => d.JobId)
//                .HasConstraintName("FK_HangFire_JobParameter_Job");
//        });

//        modelBuilder.Entity<JobQueue>(entity =>
//        {
//            entity.HasKey(e => new { e.Queue, e.Id }).HasName("PK_HangFire_JobQueue");

//            entity.ToTable("JobQueue", "HangFire");

//            entity.Property(e => e.Queue).HasMaxLength(50);
//            entity.Property(e => e.Id).ValueGeneratedOnAdd();
//            entity.Property(e => e.FetchedAt).HasColumnType("datetime");
//        });

//        modelBuilder.Entity<List>(entity =>
//        {
//            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_List");

//            entity.ToTable("List", "HangFire");

//            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

//            entity.Property(e => e.Key).HasMaxLength(100);
//            entity.Property(e => e.Id).ValueGeneratedOnAdd();
//            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
//        });

//        modelBuilder.Entity<Order>(entity =>
//        {
//            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFD4236324");

//            entity.Property(e => e.OrderId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasDefaultValueSql("('O'+format(NEXT VALUE FOR [SifoodOrderIdSeq],'000'))")
//                .HasColumnName("OrderID");
//            entity.Property(e => e.Address).HasMaxLength(255);
//            entity.Property(e => e.DeliveryMethod).HasMaxLength(10);
//            entity.Property(e => e.DriverId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("DriverID");
//            entity.Property(e => e.OrderDate).HasColumnType("datetime");
//            entity.Property(e => e.ShippingFee).HasColumnType("money");
//            entity.Property(e => e.StatusId).HasColumnName("StatusID");
//            entity.Property(e => e.StoreId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("StoreID");
//            entity.Property(e => e.UserId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("UserID");

//            entity.HasOne(d => d.Driver).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.DriverId)
//                .HasConstraintName("FK_Orders_Drivers");

//            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.StatusId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Orders_Status");

//            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.StoreId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Orders_Stores");

//            entity.HasOne(d => d.User).WithMany(p => p.Orders)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Orders_Users");
//        });

//        modelBuilder.Entity<OrderDetail>(entity =>
//        {
//            entity.HasKey(e => new { e.OrderId, e.ProductId });

//            entity.Property(e => e.OrderId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("OrderID");
//            entity.Property(e => e.ProductId).HasColumnName("ProductID");
//            entity.Property(e => e.OrderDetailId)
//                .ValueGeneratedOnAdd()
//                .HasColumnName("OrderDetailID");

//            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
//                .HasForeignKey(d => d.OrderId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_OrderDetails_Orders");

//            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
//                .HasForeignKey(d => d.ProductId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_OrderDetails_Products");
//        });

//        modelBuilder.Entity<Payment>(entity =>
//        {
//            entity.HasKey(e => e.OrderId);

//            entity.ToTable("Payment");

//            entity.Property(e => e.OrderId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("OrderID");
//            entity.Property(e => e.PaymentMethodＮame).HasMaxLength(10);
//            entity.Property(e => e.PaymentStatusＮame).HasMaxLength(10);
//            entity.Property(e => e.PaymentTime).HasColumnType("datetime");

//            entity.HasOne(d => d.Order).WithOne(p => p.Payment)
//                .HasForeignKey<Payment>(d => d.OrderId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Payment_Orders");
//        });

//        modelBuilder.Entity<Product>(entity =>
//        {
//            entity.HasKey(e => e.ProductId).HasName("PK_Products_1");

//            entity.Property(e => e.ProductId).HasColumnName("ProductID");
//            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
//            entity.Property(e => e.Description).HasMaxLength(15);
//            entity.Property(e => e.IsDelete).HasDefaultValueSql("((1))");
//            entity.Property(e => e.PhotoPath).HasMaxLength(50);
//            entity.Property(e => e.ProductName).HasMaxLength(15);
//            entity.Property(e => e.RealeasedTime).HasColumnType("datetime");
//            entity.Property(e => e.StoreId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("StoreID");
//            entity.Property(e => e.UnitPrice).HasColumnType("money");

//            entity.HasOne(d => d.Category).WithMany(p => p.Products)
//                .HasForeignKey(d => d.CategoryId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Products_Category1");

//            entity.HasOne(d => d.Store).WithMany(p => p.Products)
//                .HasForeignKey(d => d.StoreId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Products_Stores");
//        });

//        modelBuilder.Entity<Schema>(entity =>
//        {
//            entity.HasKey(e => e.Version).HasName("PK_HangFire_Schema");

//            entity.ToTable("Schema", "HangFire");

//            entity.Property(e => e.Version).ValueGeneratedNever();
//        });

//        modelBuilder.Entity<Server>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK_HangFire_Server");

//            entity.ToTable("Server", "HangFire");

//            entity.HasIndex(e => e.LastHeartbeat, "IX_HangFire_Server_LastHeartbeat");

//            entity.Property(e => e.Id).HasMaxLength(200);
//            entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
//        });

//        modelBuilder.Entity<Set>(entity =>
//        {
//            entity.HasKey(e => new { e.Key, e.Value }).HasName("PK_HangFire_Set");

//            entity.ToTable("Set", "HangFire");

//            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

//            entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

//            entity.Property(e => e.Key).HasMaxLength(100);
//            entity.Property(e => e.Value).HasMaxLength(256);
//            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
//        });

//        modelBuilder.Entity<State>(entity =>
//        {
//            entity.HasKey(e => new { e.JobId, e.Id }).HasName("PK_HangFire_State");

//            entity.ToTable("State", "HangFire");

//            entity.HasIndex(e => e.CreatedAt, "IX_HangFire_State_CreatedAt");

//            entity.Property(e => e.Id).ValueGeneratedOnAdd();
//            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
//            entity.Property(e => e.Name).HasMaxLength(20);
//            entity.Property(e => e.Reason).HasMaxLength(100);

//            entity.HasOne(d => d.Job).WithMany(p => p.States)
//                .HasForeignKey(d => d.JobId)
//                .HasConstraintName("FK_HangFire_State_Job");
//        });

//        modelBuilder.Entity<Status>(entity =>
//        {
//            entity.ToTable("Status");

//            entity.Property(e => e.StatusId).HasColumnName("StatusID");
//            entity.Property(e => e.StatusName).HasMaxLength(8);
//        });

//        modelBuilder.Entity<Store>(entity =>
//        {
//            entity.Property(e => e.StoreId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasDefaultValueSql("('S'+format(NEXT VALUE FOR [SifoodStoreIdSeq],'000'))")
//                .HasColumnName("StoreID");
//            entity.Property(e => e.Address).HasMaxLength(50);
//            entity.Property(e => e.City).HasMaxLength(10);
//            entity.Property(e => e.ClosingDay).HasMaxLength(15);
//            entity.Property(e => e.ContactName).HasMaxLength(10);
//            entity.Property(e => e.Description).HasMaxLength(25);
//            entity.Property(e => e.Email)
//                .HasMaxLength(64)
//                .IsUnicode(false);
//            entity.Property(e => e.EnrollDate).HasColumnType("date");
//            entity.Property(e => e.Latitude).HasColumnType("decimal(10, 8)");
//            entity.Property(e => e.LogoPath).HasMaxLength(70);
//            entity.Property(e => e.Longitude).HasColumnType("decimal(11, 8)");
//            entity.Property(e => e.OpeningTime).HasMaxLength(50);
//            entity.Property(e => e.PasswordHash).HasMaxLength(64);
//            entity.Property(e => e.PasswordSalt).HasMaxLength(64);
//            entity.Property(e => e.Phone).HasMaxLength(10);
//            entity.Property(e => e.PhotosPath).HasMaxLength(70);
//            entity.Property(e => e.PhotosPath2).HasMaxLength(70);
//            entity.Property(e => e.PhotosPath3).HasMaxLength(70);
//            entity.Property(e => e.Region).HasMaxLength(10);
//            entity.Property(e => e.StoreName).HasMaxLength(15);
//            entity.Property(e => e.TaxId)
//                .HasMaxLength(8)
//                .IsUnicode(false)
//                .IsFixedLength()
//                .HasColumnName("TaxID");
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC7F13BEFB");

//            entity.Property(e => e.UserId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasDefaultValueSql("('U'+format(NEXT VALUE FOR [SifoodUserIdSeq],'000'))")
//                .HasColumnName("UserID");
//            entity.Property(e => e.ForgotPasswordRandom).HasMaxLength(20);
//            entity.Property(e => e.TotalOrderAmount).HasColumnType("money");
//            entity.Property(e => e.UserBirthDate).HasColumnType("date");
//            entity.Property(e => e.UserEmail)
//                .HasMaxLength(64)
//                .IsUnicode(false);
//            entity.Property(e => e.UserEnrollDate).HasColumnType("date");
//            entity.Property(e => e.UserLatestLogInDate).HasColumnType("datetime");
//            entity.Property(e => e.UserName).HasMaxLength(15);
//            entity.Property(e => e.UserPasswordHash).HasMaxLength(64);
//            entity.Property(e => e.UserPasswordSalt).HasMaxLength(64);
//            entity.Property(e => e.UserPhone)
//                .HasMaxLength(10)
//                .IsUnicode(false);
//            entity.Property(e => e.UserVerificationCode).HasMaxLength(20);
//        });

//        modelBuilder.Entity<UserAddress>(entity =>
//        {
//            entity.HasKey(e => e.UserAddressId).HasName("PK__UserAddr__5961BB97573B1078");

//            entity.ToTable("UserAddress");

//            entity.Property(e => e.UserAddressId).HasColumnName("UserAddressID");
//            entity.Property(e => e.UserCity).HasMaxLength(50);
//            entity.Property(e => e.UserDetailAddress).HasMaxLength(50);
//            entity.Property(e => e.UserId)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("UserID");
//            entity.Property(e => e.UserLatitude).HasColumnType("decimal(10, 8)");
//            entity.Property(e => e.UserLongitude).HasColumnType("decimal(11, 8)");
//            entity.Property(e => e.UserRegion).HasMaxLength(50);

//            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_UserAddresses_Users");
//        });
//        modelBuilder.HasSequence<int>("SifoodDriverIdSeq").StartsAt(3L);
//        modelBuilder.HasSequence<int>("SifoodOrderIdSeq").StartsAt(10L);
//        modelBuilder.HasSequence<int>("SifoodStoreIdSeq").StartsAt(6L);
//        modelBuilder.HasSequence<int>("SifoodUserIdSeq").StartsAt(5L);

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
