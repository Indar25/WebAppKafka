using Microsoft.EntityFrameworkCore;
using WebAppKafka.Models;

namespace WebAppKafka.Infrastructure;

public partial class KafkaDBContext : DbContext
{
    public KafkaDBContext()
    {
    }

    public KafkaDBContext(DbContextOptions<KafkaDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Rider> Riders { get; set; }

    public virtual DbSet<RiderLocationLog> RiderLocationLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rider__3214EC074A7F40F7");

            entity.ToTable("Rider");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Organization)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RiderName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RiderLocationLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RiderLoc__3214EC07DB852F7D");

            entity.ToTable("RiderLocationLog");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Coordinates)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("coordinates");
            entity.Property(e => e.GroupId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RiderId).HasColumnName("RiderID");

            entity.HasOne(d => d.Rider).WithMany(p => p.RiderLocationLogs)
                .HasForeignKey(d => d.RiderId)
                .HasConstraintName("FK_Rider_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
