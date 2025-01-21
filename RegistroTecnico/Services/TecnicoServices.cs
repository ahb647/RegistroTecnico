using System;
using RegistroTecnico.Models;
using System.Threading.Tasks;
using RegistroTecnico.Context;
using System.Diagnostics.Eventing.Reader;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;

namespace RegistroTecnico.Services
{
    public class TecnicoServices
    {
        private readonly IDbContextFactory<TecnicoContext> DbFactory;
        private async Task<bool> Existe(int tecnidoID)
        {

            await using var Contexto = await DbFactory.CreateDbContextAsync();
            return await Contexto.Tecnicos.AnyAsync(t => t.TecnicoID == tecnidoID);


        }


        private async Task<bool> Insertar(Tecnicos tecnico)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Tecnicos.Add(tecnico);
            return await contexto.SaveChangesAsync() > 0;
        }


        private async Task<bool> Modificar(Tecnicos tecnico)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Tecnicos.Update(tecnico);
            return await contexto.SaveChangesAsync() > 0;
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
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos.FirstOrDefaultAsync(t => t.TecnicoID == tecnicoID);
        }


        public async Task<bool> Eliminar(int tecnicoID)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var tecnico = await contexto.Tecnicos.FirstOrDefaultAsync(t => t.TecnicoID == tecnicoID);

            if (tecnico == null) return false;

            contexto.Tecnicos.Remove(tecnico);
            return await contexto.SaveChangesAsync() > 0;
        }


        public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Tecnicos
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

    }




}