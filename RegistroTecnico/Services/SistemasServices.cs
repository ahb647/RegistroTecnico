using RegistroTecnico.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RegistroTecnico.Services
{
    public class SistemasServices
    {
        private readonly TecnicoContext _contexto;

        public SistemasServices(TecnicoContext contexto)
        {
            _contexto = contexto;
        }

        // Verificar si el sistema existe por su ID
        public async Task<bool> Existe(int sistemaId)
        {
            return await _contexto.Sistemas.AnyAsync(s => s.SistemaId == sistemaId);
        }

        // Insertar un nuevo sistema
        private async Task<bool> Insertar(Sistemas sistema)
        {
            _contexto.Sistemas.Add(sistema);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Modificar un sistema existente
        private async Task<bool> Modificar(Sistemas sistema)
        {
            _contexto.Sistemas.Update(sistema);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Guardar: Insertar o Modificar según corresponda
        public async Task<bool> Guardar(Sistemas sistema)
        {
            if (!await Existe(sistema.SistemaId))
            {
                return await Insertar(sistema);
            }
            else
            {
                return await Modificar(sistema);
            }
        }

        // Buscar un sistema por su ID
        public async Task<Sistemas> Buscar(int sistemaId)
        {
            return await _contexto.Sistemas.FirstOrDefaultAsync(s => s.SistemaId == sistemaId);
        }

        // Eliminar un sistema por su ID
        public async Task<bool> Eliminar(int sistemaId)
        {
            var sistema = await _contexto.Sistemas.FirstOrDefaultAsync(s => s.SistemaId == sistemaId);

            if (sistema == null) return false;

            _contexto.Sistemas.Remove(sistema);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Listar sistemas aplicando un criterio de filtro
        public async Task<List<Sistemas>> Listar(Expression<Func<Sistemas, bool>> criterio)
        {
            return await _contexto.Sistemas
                .Where(criterio)
                .AsNoTracking() // Mejora el rendimiento para solo lectura
                .ToListAsync();
        }
    }
}