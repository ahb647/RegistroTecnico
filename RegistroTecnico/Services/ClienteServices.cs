using RegistroTecnico.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace RegistroTecnico.Services
{
    public class ClienteServices
    {
        private readonly TecnicoContext _contexto;

        public ClienteServices(TecnicoContext contexto)
        {
            _contexto = contexto;
        }

        // Verificar si un cliente existe por ID
        public async Task<bool> Existe(int clienteID)
        {
            return await _contexto.Clientes.AnyAsync(c => c.ClienteID == clienteID);
        }

        // Verificar si ya existe un cliente con el mismo nombre o RNC
        private async Task<bool> ExisteDuplicado(string nombres, int rnc, int? clienteID = null)
        {
            return await _contexto.Clientes.AnyAsync(c =>
                (c.Nombres == nombres || c.Rnc == rnc) &&
                (clienteID == null || c.ClienteID != clienteID));
        }

        // Método para insertar un nuevo cliente
        private async Task<bool> Insertar(Clientes cliente)
        {
            // Validar unicidad de nombre y RNC
            if (await ExisteDuplicado(cliente.Nombres, cliente.Rnc))
            {
                throw new InvalidOperationException("Ya existe un cliente con el mismo nombre o RNC.");
            }

            cliente.FechaIngreso = DateTime.Now; // Asignar fecha de ingreso actual
            _contexto.Clientes.Add(cliente);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Método para modificar un cliente existente
        private async Task<bool> Modificar(Clientes cliente)
        {
            // Validar unicidad de nombre y RNC
            if (await ExisteDuplicado(cliente.Nombres, cliente.Rnc, cliente.ClienteID))
            {
                throw new InvalidOperationException("Ya existe un cliente con el mismo nombre o RNC.");
            }

            _contexto.Clientes.Update(cliente);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Guardar un cliente (insertar o modificar según corresponda)
        public async Task<bool> Guardar(Clientes cliente)
        {
            if (!await Existe(cliente.ClienteID))
            {
                return await Insertar(cliente);
            }
            else
            {
                return await Modificar(cliente);
            }
        }

        // Buscar un cliente por ID
        public async Task<Clientes> Buscar(int clienteID)
        {
            return await _contexto.Clientes.FirstOrDefaultAsync(c => c.ClienteID == clienteID);
        }

        // Eliminar un cliente por ID
        public async Task<bool> Eliminar(int clienteID)
        {
            var cliente = await _contexto.Clientes.FirstOrDefaultAsync(c => c.ClienteID == clienteID);

            if (cliente == null) return false;

            _contexto.Clientes.Remove(cliente);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Listar clientes que cumplen con un criterio
        public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
        {
            return await _contexto.Clientes
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}