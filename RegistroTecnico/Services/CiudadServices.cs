using RegistroTecnico.Models;
using System.Linq.Expressions;


namespace RegistroTecnico.Services
{
    public class CiudadServices
    {

        public class CiudadService
        {
            private readonly TecnicoContext _contexto;

            public CiudadService(TecnicoContext contexto)
            {
                _contexto = contexto;
            }


            public async Task<bool> Existe(int ciudadID)
            {
                return await _contexto.Ciudades.AnyAsync(c => c.CiudadID == ciudadID);
            }


            private async Task<bool> Insertar(Ciudad ciudad)
            {
              

                _contexto.Ciudades.Add(ciudad);
                return await _contexto.SaveChangesAsync() > 0;
            }



            private async Task<bool> Modificar(Ciudad ciudad)
            {
            

                _contexto.Ciudades.Update(ciudad);
                return await _contexto.SaveChangesAsync() > 0;
            }


            public async Task<bool> Guardar(Ciudad ciudad)
            {
                if (!await Existe(ciudad.CiudadID))
                {
                    return await Insertar(ciudad);
                }
                else
                {
                    return await Modificar(ciudad);
                }
            }


            public async Task<Ciudad> Buscar(int ciudadID)
            {
                return await _contexto.Ciudades.FirstOrDefaultAsync(c => c.CiudadID == ciudadID);
            }

            public async Task<bool> Eliminar(int ciudadID)
            {
                var ciudad = await _contexto.Ciudades.FirstOrDefaultAsync(c => c.CiudadID == ciudadID);

                if (ciudad == null) return false;

                _contexto.Ciudades.Remove(ciudad);
                return await _contexto.SaveChangesAsync() > 0;
            }


            public async Task<List<Ciudad>> Listar(Expression<Func<Ciudad, bool>> criterio)
            {
                return await _contexto.Ciudades
                    .Where(criterio)
                    .AsNoTracking()
                    .ToListAsync();
            }


        }
    }

}
