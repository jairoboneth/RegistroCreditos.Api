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

            // Semilla de Usuario
            entity.HasData(new Usuario
            {
                Id = 1,
                Nombre = "Usuario de Pruebas",
                Email = "test@empresa.com",
                PasswordHash = "$2a$11$GCi/1BMMsbfUyEANY2xaNu5t.5j0Vw4bg1HBHr0ojat0BITZxZNeG"
            });
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

            // Relación
            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.Creditos)
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Semilla de Créditos
            entity.HasData(
                new Credito { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), NombreCliente = "Pepito Perez", CedulaCliente = "1000000001", ValorCredito = 7800000, PlazoMeses = 10, TasaInteres = 2, ComercialNombre = "Sede Norte", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), NombreCliente = "Maria Perez", CedulaCliente = "1000000002", ValorCredito = 12500000, PlazoMeses = 5, TasaInteres = 2, ComercialNombre = "Sede Sur", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), NombreCliente = "Antonio Rodriguez", CedulaCliente = "1000000003", ValorCredito = 10312673, PlazoMeses = 5, TasaInteres = 2, ComercialNombre = "Sede Centro", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), NombreCliente = "Giselle López", CedulaCliente = "1000000004", ValorCredito = 8628510, PlazoMeses = 12, TasaInteres = 2, ComercialNombre = "Sede Este", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), NombreCliente = "Martha Perez", CedulaCliente = "1000000005", ValorCredito = 5889085, PlazoMeses = 24, TasaInteres = 2, ComercialNombre = "Sede Oeste", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), NombreCliente = "Isaac llanos", CedulaCliente = "1000000006", ValorCredito = 14793565, PlazoMeses = 48, TasaInteres = 2, ComercialNombre = "Sede Norte", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), NombreCliente = "Teresa Gutierrez", CedulaCliente = "1000000007", ValorCredito = 8072348, PlazoMeses = 50, TasaInteres = 2, ComercialNombre = "Sede Sur", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), NombreCliente = "Isabel Llanos", CedulaCliente = "1000000008", ValorCredito = 5143860, PlazoMeses = 60, TasaInteres = 2, ComercialNombre = "Sede Centro", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), NombreCliente = "Paola Tao", CedulaCliente = "1000000009", ValorCredito = 12881963, PlazoMeses = 24, TasaInteres = 2, ComercialNombre = "Sede Este", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 },
                new Credito { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), NombreCliente = "Wendy Moscoso", CedulaCliente = "1000000010", ValorCredito = 13484682, PlazoMeses = 40, TasaInteres = 2, ComercialNombre = "Sede Oeste", FechaRegistro = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), UsuarioId = 1 }
            );
        });
    }
}
