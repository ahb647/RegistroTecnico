using RegistroTecnico.Models;
using System.Linq.Expressions;


namespace RegistroTecnico.Services
{
    public class ClienteServices
    {

        private readonly TecnicoContext _contexto;


        public ClienteServices(TecnicoContext contexto)
        {
            _contexto = contexto;
        }

        // Verifica si existe un cliente con el ID proporcionado
        public async Task<bool> Existe(int ClientesID)
        {
            return await _contexto.Clientes
                .AnyAsync(c => c.ClienteID == ClientesID);
        }

        // Modifica los datos de un cliente existente
        public async Task<bool> Modificar(Clientes cliente)
        {
            _contexto.Update(cliente);
            var modificar = await _contexto.SaveChangesAsync() > 0;
            _contexto.Entry(cliente).State = EntityState.Detached;
            return modificar;
        }

        // Inserta un nuevo cliente y asigna la fecha de entrada actual
        private async Task<bool> Insertar(Clientes clientes)
        {
            clientes.FechaIngreso = DateTime.Now; // Asignar la fecha actual
            _contexto.Clientes.Add(clientes);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Guarda un cliente (inserta o modifica según corresponda)
        public async Task<bool> Guardar(Clientes clientes)
        {
            if (!await Existe(clientes.ClienteID))
                return await Insertar(clientes);
            else
                return await Modificar(clientes);
        }

        // Elimina un cliente por su ID
        public async Task<bool> Eliminar(Clientes clientes)
        {
            var cantidad = await _contexto.Clientes
                .Where(c => c.ClienteID == clientes.ClienteID)
                .ExecuteDeleteAsync();

            return cantidad > 0;
        }

        // Busca un cliente por su nombre
        public async Task<Clientes?> BuscarClientes(string nombres)
        {
            return await _contexto.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Nombres.ToLower() == nombres.ToLower());
        }

        // Busca un cliente por su ID
        public async Task<Clientes?> Buscar(int ClientesID)
        {
            return await _contexto.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClienteID == ClientesID);
        }

        // Lista de clientes que cumplen con un criterio
        public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
        {
            return await _contexto.Clientes.AsNoTracking().Where(criterio).ToListAsync();
        }

        // Verifica si ya existe un cliente con el mismo nombre o RNC
        public bool ExisteD(Clientes clientes)
        {
            var mismosDatos = _contexto.Clientes.Any(c => c.Nombres == clientes.Nombres || c.Rnc == clientes.Rnc);
            return !mismosDatos;
        }

        // Asigna un técnico a un cliente
        public async Task<bool> AsignarTecnico(int clienteID, int tecnicoID)
        {
            var cliente = await _contexto.Clientes.FirstOrDefaultAsync(c => c.ClienteID == clienteID);

            if (cliente == null)
                return false;

            cliente.TecnicoID = tecnicoID; // Asignar el técnico al cliente
            _contexto.Update(cliente);

            return await _contexto.SaveChangesAsync() > 0;
        }

        // Método para editar datos clave de un cliente
        public async Task<bool> EditarCliente(int clienteID, string? nombre, string? direccion, int? rnc, decimal? limiteCredito)
        {
            var cliente = await _contexto.Clientes.FirstOrDefaultAsync(c => c.ClienteID == clienteID);

            if (cliente == null)
                return false;

            if (!string.IsNullOrWhiteSpace(nombre))
                cliente.Nombres = nombre;

            if (!string.IsNullOrWhiteSpace(direccion))
                cliente.Direccion = direccion;

            if (rnc.HasValue)
                cliente.Rnc = rnc.Value;

            if (limiteCredito.HasValue)
                cliente.LimiteCredito = limiteCredito.Value;

            _contexto.Update(cliente);

            return await _contexto.SaveChangesAsync() > 0;
        }

    }
}
