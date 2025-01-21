using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RegistroTecnico.Context
{
    public class ContextoFactory : IDesignTimeDbContextFactory<TecnicoContext>
    {
        public TecnicoContext CreateDbContext(string[] args)
        {
            // Configura el DbContext usando una cadena de conexión estática o predeterminada.
            var optionsBuilder = new DbContextOptionsBuilder<TecnicoContext>();
            optionsBuilder.UseSqlServer("Server=ADRIAN\\MSSQLSERVER01;Database=Tecnicos;Trusted_Connection=True;TrustServerCertificate=True;");

            return new TecnicoContext(optionsBuilder.Options);
        }
    }
}

