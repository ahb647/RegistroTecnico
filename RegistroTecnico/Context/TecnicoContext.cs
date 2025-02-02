
using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Models;


namespace RegistroTecnico.Context
{
    public class TecnicoContext : DbContext
    {

        public TecnicoContext(DbContextOptions<TecnicoContext> options) : base(options) { }

        public DbSet<Tecnicos> Tecnicos { get; set; }

        public DbSet<Clientes> Clientes { get; set; }

        public DbSet<Ciudad> Ciudades { get; set; }

        public DbSet<Tickets> Tickets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación Tickets -> Tecnicos
            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.Tecnico) // Un ticket tiene un técnico
                .WithMany() // Un técnico puede tener muchos tickets
                .HasForeignKey(t => t.TecnicoID) // Clave foránea en Tickets
                .OnDelete(DeleteBehavior.Restrict); // Evita la eliminación en cascada

            // Configuración de la relación Tickets -> Clientes
            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.Cliente) // Un ticket tiene un cliente
                .WithMany() // Un cliente puede tener muchos tickets
                .HasForeignKey(t => t.ClienteID) // Clave foránea en Tickets
                .OnDelete(DeleteBehavior.Restrict); // Evita la eliminación en cascada
        }

    }


}
