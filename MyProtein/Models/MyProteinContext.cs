using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyProtein.Models;

public partial class MyProteinContext : DbContext
{
    public MyProteinContext()
    {
    }

    public MyProteinContext(DbContextOptions<MyProteinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Flavour> Flavours { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Weight> Weights { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    public virtual DbSet<WishlistItem> WishlistItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=MyProtein;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__addresse__CAA247C88D2D56A0");

            entity.ToTable("addresses");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.AddressLine)
                .HasMaxLength(255)
                .HasColumnName("address_line");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("is_default");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("postal_code");
            entity.Property(e => e.StateProvince)
                .HasMaxLength(100)
                .HasColumnName("state_province");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__addresses__user___1A54DAB7");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__admins__43AA414185B2C024");

            entity.ToTable("admins");

            entity.HasIndex(e => e.Username, "UQ__admins__F3DBC572FC569B92").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__carts__2EF52A273F853AB6");

            entity.ToTable("carts");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__carts__user_id__2101D846");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__cart_ite__52020FDDA5049375");

            entity.ToTable("cart_items");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
            entity.Property(e => e.VariantId).HasColumnName("variant_id");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__cart_item__cart___21F5FC7F");

            entity.HasOne(d => d.Variant).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cart_item__varia__22EA20B8");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__categori__D54EE9B4590D00F3");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__colors__1143CECB72D6FD7B");

            entity.ToTable("colors");

            entity.HasIndex(e => e.ColorName, "UQ__colors__2F423AB8BBCA18CF").IsUnique();

            entity.Property(e => e.ColorId).HasColumnName("color_id");
            entity.Property(e => e.ColorName)
                .HasMaxLength(50)
                .HasColumnName("color_name");
        });

        modelBuilder.Entity<Flavour>(entity =>
        {
            entity.HasKey(e => e.FlavourId).HasName("PK__flavours__3BE665E38A6EA66B");

            entity.ToTable("flavours");

            entity.HasIndex(e => e.FlavourName, "UQ__flavours__E6DD5CD28DD43BDF").IsUnique();

            entity.Property(e => e.FlavourId).HasColumnName("flavour_id");
            entity.Property(e => e.FlavourName)
                .HasMaxLength(50)
                .HasColumnName("flavour_name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PK__manufact__163F69B012A2F194");

            entity.ToTable("manufacturers");

            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__4659622929D033C6");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.ShippingAddressId).HasColumnName("shipping_address_id");
            entity.Property(e => e.ShippingMethodId).HasColumnName("shipping_method_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__payment___25C68D63");

            entity.HasOne(d => d.ShippingAddress).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__shipping__24D2692A");

            entity.HasOne(d => d.ShippingMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__shipping__26BAB19C");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__user_id__23DE44F1");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__order_it__3764B6BCF2C86308");

            entity.ToTable("order_items");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
            entity.Property(e => e.VariantId).HasColumnName("variant_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__order_ite__order__27AED5D5");

            entity.HasOne(d => d.Variant).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.VariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_ite__varia__28A2FA0E");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__payment___8A3EA9EB957AFEE5");

            entity.ToTable("payment_methods");

            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__47027DF54029C9B1");

            entity.ToTable("products");

            entity.HasIndex(e => e.Sku, "UQ__products__DDDF4BE7CBE98D64").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.SalePrice).HasColumnName("sale_price");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sku");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__products__catego__1B48FEF0");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__products__manufa__1C3D2329");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__product___DC9AC955DB125E2D");

            entity.ToTable("product_images");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.AltText)
                .HasMaxLength(255)
                .HasColumnName("alt_text");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__product_i__produ__1D314762");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("PK__product___EACC68B7BE8D9896");

            entity.ToTable("product_variants");

            entity.Property(e => e.VariantId).HasColumnName("variant_id");
            entity.Property(e => e.FlavourId).HasColumnName("flavour_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.WeightId).HasColumnName("weight_id");

            entity.HasOne(d => d.Flavour).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.FlavourId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__product_v__flavo__200DB40D");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__product_v__produ__1E256B9B");

            entity.HasOne(d => d.Weight).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.WeightId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__product_v__weigh__1F198FD4");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.RefundId).HasName("PK__refunds__897E9EA354861796");

            entity.ToTable("refunds");

            entity.Property(e => e.RefundId).HasColumnName("refund_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProcessedAt)
                .HasColumnType("datetime")
                .HasColumnName("processed_at");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending")
                .HasColumnName("status");

            entity.HasOne(d => d.Order).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__refunds__order_i__2E5BD364");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__reviews__60883D902B68385E");

            entity.ToTable("reviews");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__reviews__product__29971E47");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__reviews__user_id__2A8B4280");
        });

        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.HasKey(e => e.ShippingMethodId).HasName("PK__shipping__DCF5023B5C4A8D83");

            entity.ToTable("shipping_methods");

            entity.Property(e => e.ShippingMethodId).HasColumnName("shipping_method_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Fee)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("fee");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__sizes__0DCACE31C11E5093");

            entity.ToTable("sizes");

            entity.HasIndex(e => e.SizeName, "UQ__sizes__75FCE556C07A2B8A").IsUnique();

            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.SizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("size_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FC991F958");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616422B3E50E").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC57209C11E47").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Weight>(entity =>
        {
            entity.HasKey(e => e.WeightId).HasName("PK__weights__453932ACCFF76F6A");

            entity.ToTable("weights");

            entity.HasIndex(e => e.WeightValue, "UQ__weights__4D069A65DA6C5F39").IsUnique();

            entity.Property(e => e.WeightId).HasColumnName("weight_id");
            entity.Property(e => e.Servings).HasColumnName("servings");
            entity.Property(e => e.WeightValue).HasColumnName("weight_value");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__wishlist__6151514E1FC872DA");

            entity.ToTable("wishlists");

            entity.HasIndex(e => e.UserId, "UQ__wishlist__B9BE370E575C9E17").IsUnique();

            entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Wishlist)
                .HasForeignKey<Wishlist>(d => d.UserId)
                .HasConstraintName("FK__wishlists__user___2B7F66B9");
        });

        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__wishlist__3213E83F13FFA3B0");

            entity.ToTable("wishlist_items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");

            entity.HasOne(d => d.Product).WithMany(p => p.WishlistItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__wishlist___produ__2D67AF2B");

            entity.HasOne(d => d.Wishlist).WithMany(p => p.WishlistItems)
                .HasForeignKey(d => d.WishlistId)
                .HasConstraintName("FK__wishlist___wishl__2C738AF2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
