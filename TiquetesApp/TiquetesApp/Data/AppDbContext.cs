using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TiquetesApp.Models;

namespace TiquetesApp.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Compra> Compras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evento -> Compra (1 a muchos)
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Evento)
                .WithMany(e => e.Compras)
                .HasForeignKey(c => c.EventoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario -> Compra (1 a muchos)
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Compras)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}