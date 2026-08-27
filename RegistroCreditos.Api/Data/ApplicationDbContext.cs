using Microsoft.EntityFrameworkCore;
using RegistroCreditos.Api.Models;

namespace RegistroCreditos.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Credito> Creditos => Set<Credito>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique(); // Index and unique constraint
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<Credito>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreCliente).IsRequired().HasMaxLength(150);
            entity.Property(e => e.CedulaCliente).IsRequired().HasMaxLength(20);
            entity.Property(e => e.ComercialNombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ValorCredito).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.TasaInteres).HasColumnType("decimal(5,2)").IsRequired();
            entity.Property(e => e.PlazoMeses).IsRequired();
            entity.Property(e => e.FechaRegistro).IsRequired();
            entity.HasIndex(e => e.CedulaCliente);
            entity.HasIndex(e => e.ComercialNombre);
            entity.HasIndex(e => e.FechaRegistro);

            // RelaciÃ³n
            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.Creditos)
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
