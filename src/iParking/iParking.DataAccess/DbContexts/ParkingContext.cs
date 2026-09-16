using iParking.Domain.Common;
using iParking.Domain.Entities.MultiTenant;
using iParking.Domain.Entities.Parking;
using iParking.Domain.Entities.Subscription;
using iParking.Domain.Entities.Usuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iParking.DataAccess.DbContexts;

public partial class ParkingContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ParkingContext()
    {
    }

    public ParkingContext(DbContextOptions<ParkingContext> options)
        : base(options)
    {
    }

    // Multi-Tenant
    public virtual DbSet<Company> Companies { get; set; }

    // Parking
    public virtual DbSet<ParkingLot> ParkingLots { get; set; }
    public virtual DbSet<ParkingSpot> ParkingSpots { get; set; }
    public virtual DbSet<ParkingRate> ParkingRates { get; set; }
    public virtual DbSet<ParkingSession> ParkingSessions { get; set; }

    // Subscription
    public virtual DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar User (IdentityUser)
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(e => e.Rut).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Dv).HasMaxLength(1).IsUnicode(false);
            entity.Property(e => e.Nombres).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Apellidos).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Telefono).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Mail).HasMaxLength(256);
            entity.Property(e => e.ImeiCelular).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.SerieCelular).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.VersionApp).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Ciudad).HasMaxLength(100).IsUnicode(false);

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configurar Company
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BusinessName).HasMaxLength(200).IsUnicode(false).IsRequired();
            entity.Property(e => e.TaxId).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.Property(e => e.LogoUrl).HasMaxLength(500).IsUnicode(false);
            entity.Property(e => e.ContactEmail).HasMaxLength(256);
            entity.Property(e => e.ContactPhone).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Address).HasMaxLength(500).IsUnicode(false);
            entity.Property(e => e.City).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaxParkingLots).HasDefaultValue(5);
            entity.Property(e => e.MaxUsers).HasDefaultValue(20);
        });

        // Configurar ParkingLot
        modelBuilder.Entity<ParkingLot>(entity =>
        {
            entity.ToTable("ParkingLots");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsUnicode(false).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500).IsUnicode(false).IsRequired();
            entity.Property(e => e.City).HasMaxLength(100).IsUnicode(false).IsRequired();
            entity.Property(e => e.Latitude).HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasPrecision(9, 6);
            entity.Property(e => e.Is24Hours).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TotalCapacity).HasDefaultValue(0);

            entity.HasOne(d => d.Company)
                .WithMany(p => p.ParkingLots)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configurar ParkingSpot
        modelBuilder.Entity<ParkingSpot>(entity =>
        {
            entity.ToTable("ParkingSpots");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Identifier).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.Property(e => e.Sector).HasMaxLength(50).IsUnicode(false).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(Enums.ParkingSpotStatus.Available);
            entity.Property(e => e.IsDisabled).HasDefaultValue(false);

            entity.HasOne(d => d.ParkingLot)
                .WithMany(p => p.ParkingSpots)
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configurar ParkingRate
        modelBuilder.Entity<ParkingRate>(entity =>
        {
            entity.ToTable("ParkingRates");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsUnicode(false).IsRequired();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.FreeMinutes).HasDefaultValue(0);

            entity.HasOne(d => d.ParkingLot)
                .WithMany(p => p.Rates)
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configurar ParkingSession
        modelBuilder.Entity<ParkingSession>(entity =>
        {
            entity.ToTable("ParkingSessions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LicensePlate).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(Enums.ParkingSessionStatus.Active);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.TicketCode).HasMaxLength(50).IsUnicode(false).IsRequired();
            entity.Property(e => e.IsPaid).HasDefaultValue(false);

            entity.HasOne(d => d.ParkingLot)
                .WithMany(p => p.Sessions)
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.ParkingSpot)
                .WithMany()
                .HasForeignKey(d => d.ParkingSpotId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.OperatorUser)
                .WithMany(p => p.OperatedSessions)
                .HasForeignKey(d => d.OperatorUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Rate)
                .WithMany()
                .HasForeignKey(d => d.RateId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configurar Subscription
        modelBuilder.Entity<Domain.Entities.Subscription.Subscription>(entity =>
        {
            entity.ToTable("Subscriptions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LicensePlate).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.AutoRenew).HasDefaultValue(false);
            entity.Property(e => e.GracePeriodDays).HasDefaultValue(0);

            entity.HasOne(d => d.Company)
                .WithMany()
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.ParkingLot)
                .WithMany()
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.User)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
