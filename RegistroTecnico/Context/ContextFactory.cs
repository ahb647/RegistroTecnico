using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RegistroTecnico.Context
{
    public class ContextoFactory : IDesignTimeDbContextFactory<TecnicoContext>
    {
        public TecnicoContext CreateDbContext(string[] args)
        {
            // Configura el DbContext usando la cadena de conexión de Somee.
            var optionsBuilder = new DbContextOptionsBuilder<TecnicoContext>();
            optionsBuilder.UseSqlServer("workstation id=RegistroTecnicoAdrian.mssql.somee.com;packet size=4096;user id=AHB_SQLLogin_2;pwd=3egw6zk3ao;data source=RegistroTecnicoAdrian.mssql.somee.com;persist security info=False;initial catalog=RegistroTecnicoAdrian;TrustServerCertificate=True");

            return new TecnicoContext(optionsBuilder.Options);
        }
    }
}