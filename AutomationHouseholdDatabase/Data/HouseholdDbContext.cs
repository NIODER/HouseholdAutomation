using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutomationHouseholdDatabase.Data;

public partial class HouseholdDbContext : DbContext, IHouseholdDbContext
{
    private readonly string _connectionString;

    public HouseholdDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public HouseholdDbContext(string connectionString, DbContextOptions<HouseholdDbContext> options)
        : base(options)
    {
        _connectionString = connectionString;
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrdersToResource> OrdersToResources { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<ProviderToResource> ProviderToResources { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public void ClearChanges()
    {
        ChangeTracker.Clear();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("accounts_pkey");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.AccountName, "accounts_account_name_key").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(250)
                .HasColumnName("account_name");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.PassHash)
                .HasColumnType("character varying")
                .HasColumnName("pass_hash");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.ClientName)
                .HasMaxLength(250)
                .HasColumnName("client_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("'2023-08-04'::date")
                .HasColumnName("order_date");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("order_client_fk");
        });

        modelBuilder.Entity<OrdersToResource>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ResourceId }).HasName("orders_to_resources_pkey");
            entity.ToTable("orders_to_resources");

            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ResourceId).HasColumnName("resource_id");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("orders_to_resources_order_fk");

            entity.HasOne(d => d.Resource).WithMany()
                .HasForeignKey(d => d.ResourceId)
                .HasConstraintName("orders_to_resources_resource_fk");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("providers_pkey");

            entity.ToTable("providers");

            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.ProviderName)
                .HasMaxLength(250)
                .HasColumnName("provider_name");
            entity.Property(e => e.Website)
                .HasMaxLength(512)
                .HasColumnName("website");
        });

        modelBuilder.Entity<ProviderToResource>(entity =>
        {
            entity.HasKey(e => new { e.ProviderId, e.ResourceId }).HasName("provider_to_resources_pkey");

            entity.ToTable("provider_to_resources");

            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.Cost).HasColumnName("cost");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderToResources)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("provider_to_resources_providers_fk");

            entity.HasOne(d => d.Resource).WithMany(p => p.ProviderToResources)
                .HasForeignKey(d => d.ResourceId)
                .HasConstraintName("provider_to_resources_resources_fk");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("resources_pkey");

            entity.ToTable("resources");

            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.ResourceName)
                .HasMaxLength(250)
                .HasColumnName("resource_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
