

using Microsoft.EntityFrameworkCore;
using SistemaErp.Dominio.Entidades;

namespace SistemaErp.Infrastructura.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movimiento> Clientes => Set<Movimiento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.ToTable("MOV_INVENTARIO");          
                entity.HasKey(e => e.CodCia);    

                entity.Property(e => e.CompaniaVenta3)
                      .HasMaxLength(80)
                      .IsRequired();

                entity.Property(e => e.AlmacenVenta)
                      .HasMaxLength(80)
                      .IsRequired();

                entity.Property(e => e.TipoMovimiento)
                      .HasMaxLength(30)
                      .IsFixedLength()
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
