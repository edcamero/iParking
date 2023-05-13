using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace iParking.DataAccess.DbContexts;

public partial class ParkingContext : DbContext
{
    public ParkingContext()
    {
    }

    public ParkingContext(DbContextOptions<ParkingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Parking> Parkings { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Parking__3213E83F389FD563");

            entity.ToTable("Parking");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicle__3213E83F72015460");

            entity.ToTable("Vehicle");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Parking)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("parking");
            entity.Property(e => e.ParkingId).HasColumnName("parkingId");
            entity.Property(e => e.Plate)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("plate");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.ParkingNavigation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ParkingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarParking_Parking");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
