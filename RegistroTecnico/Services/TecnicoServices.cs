using RegistroTecnico.Models;
using System.Linq.Expressions;

namespace RegistroTecnico.Services
{
    public class TecnicoServices
    {
        private readonly TecnicoContext _contexto;

        public TecnicoServices(TecnicoContext contexto)
        {
            _contexto = contexto;
        }

        // Verificar si un técnico existe por ID
        public async Task<bool> Existe(int tecnicoID)
        {
            return await _contexto.Tecnicos.AnyAsync(t => t.TecnicoID == tecnicoID);
        }

        // Verificar si un técnico ya existe por nombre (evitar duplicados)
        public async Task<bool> ExistePorNombre(string nombre)
        {
            return await _contexto.Tecnicos.AnyAsync(t => t.Nombre == nombre);
        }

        private async Task<bool> Insertar(Tecnicos tecnico)
        {
            // Verificar si el nombre ya existe antes de insertar
            if (await ExistePorNombre(tecnico.Nombre))
            {
                throw new InvalidOperationException("Ya existe un técnico con el mismo nombre.");
            }

            _contexto.Tecnicos.Add(tecnico);
            return await _contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Tecnicos tecnico)
        {
            // Verificar si el nombre ya existe antes de modificar, excluyendo el técnico actual
            if (await _contexto.Tecnicos.AnyAsync(t => t.Nombre == tecnico.Nombre && t.TecnicoID != tecnico.TecnicoID))
            {
                throw new InvalidOperationException("Ya existe un técnico con el mismo nombre.");
            }

            _contexto.Tecnicos.Update(tecnico);
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(Tecnicos tecnico)
        {
            if (!await Existe(tecnico.TecnicoID))
            {
                return await Insertar(tecnico);
            }
            else
            {
                return await Modificar(tecnico);
            }
        }

        public async Task<Tecnicos> Buscar(int tecnicoID)
        {
            return await _contexto.Tecnicos.FirstOrDefaultAsync(t => t.TecnicoID == tecnicoID);
        }

        public async Task<bool> Eliminar(int tecnicoID)
        {
            var tecnico = await _contexto.Tecnicos.FirstOrDefaultAsync(t => t.TecnicoID == tecnicoID);

            if (tecnico == null) return false;

            _contexto.Tecnicos.Remove(tecnico);
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
        {
            return await _contexto.Tecnicos
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
