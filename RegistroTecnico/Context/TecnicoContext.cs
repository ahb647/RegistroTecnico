
using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Models.RegistroTecnicos.Models;


namespace RegistroTecnicos.Context
{
    public class TecnicoContext : DbContext
    {

        public TecnicoContext(DbContextOptions<TecnicoContext> options) : base(options) { }

        public DbSet<Tecnicos> Tecnicos { get; set; }
    }


}
