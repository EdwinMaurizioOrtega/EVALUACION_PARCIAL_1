using Microsoft.EntityFrameworkCore;
using SistemaVentas.Models;

namespace SistemaVentas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProductoModel> Productos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<VentaModel> Ventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductoModel>().ToTable("Productos");
            modelBuilder.Entity<ClienteModel>().ToTable("Clientes");
            modelBuilder.Entity<VentaModel>().ToTable("Ventas");
        }
    }
}
