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
        private async Task<bool> Existe(int tecnidoID)
        {

            return await _contexto.Tecnicos.AnyAsync(t => t.TecnicoID == tecnidoID);


        }


        private async Task<bool> Insertar(Tecnicos tecnico)
        {
            _contexto.Tecnicos.Add(tecnico);
            return await _contexto.SaveChangesAsync() > 0;
        }


        private async Task<bool> Modificar(Tecnicos tecnico)
        {
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
