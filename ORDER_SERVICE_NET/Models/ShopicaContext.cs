using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ORDER_SERVICE_NET.Models
{
    public partial class ShopicaContext : DbContext
    {
        public ShopicaContext()
        {
        }

        public ShopicaContext(DbContextOptions<ShopicaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartDetail> CartDetail { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Notify> Notify { get; set; }
        public virtual DbSet<CustomerPromo> CustomerPromo { get; set; }
                                                                       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.ToTable("cart_detail");

                entity.HasIndex(e => e.CartId)
                    .HasName("FK_CartDetail_Store");

                entity.HasIndex(e => e.ProductDetailId)
                    .HasName("fk_cart_detail_product_detail");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CartId)
                    .HasColumnName("cart_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductDetailId)
                    .HasColumnName("product_detail_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Created_at)
                  .HasColumnName("created_at")
                  .HasColumnType("datetime");

                entity.Property(e => e.Updated_at)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartDetail)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartDetail_Store");
            });

            modelBuilder.Entity<Carts>(entity =>
            {
                entity.ToTable("carts");

                entity.HasIndex(e => e.AccountId)
                    .HasName("FK_Order_Account");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("decimal(10,0)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Created_at)
                  .HasColumnName("created_at")
                  .HasColumnType("datetime");

                entity.Property(e => e.Updated_at)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY");

                entity.ToTable("order_detail");

                entity.HasIndex(e => e.OrderId)
                    .HasName("FK_orderDetail_Order");

                entity.HasIndex(e => e.ProductDetailId)
                    .HasName("fk_order_detail_product_detail1_idx");


                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductDetailId)
                    .HasColumnName("product_detail_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");


                entity.Property(e => e.TotalPriceProduct)
                    .HasColumnName("total_price_product")
                    .HasColumnType("decimal(10,0)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Created_at)
                  .HasColumnName("created_at")
                  .HasColumnType("datetime");

                entity.Property(e => e.Updated_at)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orderDetail_Order");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("order");

                entity.HasKey(e => new { e.Id })
                   .HasName("PRIMARY");

                entity.HasIndex(e => e.PromotionId)
                    .HasName("FK_Order_Promotion");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.CustomerName)
                    .HasColumnName("customer_name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(45);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(12);

                entity.Property(e => e.PromotionId)
                    .HasColumnName("promotion_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.StoreId)
                   .HasColumnName("store_id")
                   .HasColumnType("int(11)")
                   .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.QrCode)
                    .HasColumnName("qr_code")
                    .HasColumnType("longtext")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("enum('PENDING','DELIVER','COMPLETE','CANCLE')");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("decimal(10,0)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ShippingCost)
                    .HasColumnName("shipping_cost")
                    .HasColumnType("decimal(10,0)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Discount)
                .HasColumnName("discount")
                .HasColumnType("decimal(10,0)")
                .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PaymentMethod)
               .HasColumnName("payment_method")
               .HasColumnType("enum('CASH','PAYPAL')");

                entity.Property(e => e.TransactionId)
                   .HasColumnName("transaction_id")
                   .HasMaxLength(45)
                   .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Created_at)
                 .HasColumnName("created_at")
                 .HasColumnType("datetime");

                entity.Property(e => e.Updated_at)
                .HasColumnName("updated_at")
                .HasColumnType("datetime");
            });

            modelBuilder.Entity<CustomerPromo>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.PromotionId })
                    .HasName("PRIMARY");

                entity.ToTable("customer_promo");

                entity.HasIndex(e => e.PromotionId)
                    .HasName("fk_customer_promo_promotion1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PromotionId)
                    .HasColumnName("promotion_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Used_at)
                  .HasColumnName("used_at")
                  .HasColumnType("datetime");

                entity.Property(e => e.Created_at)
                  .HasColumnName("created_at")
                  .HasColumnType("datetime");

                entity.Property(e => e.Updated_at)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.CustomerPhone)
                    .IsRequired()
                    .HasColumnName("customer_phone")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<Notify>(entity =>
            {
                entity.ToTable("notify");

                entity.HasIndex(e => e.StoreId)
                    .HasName("FK_Notify_Store");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(200);

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(20);

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created_at)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Updated_at)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
