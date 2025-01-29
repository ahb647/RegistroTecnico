using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Models;
using System.Linq.Expressions;

namespace RegistroTecnico.Services
{
    public class TicketsServices
    {
        private readonly TecnicoContext _contexto;

        public TicketsServices(TecnicoContext contexto)
        {
            _contexto = contexto;
        }

        // Crear un nuevo ticket
        public async Task<bool> Crear(Tickets ticket)
        {
            // Verifica si el cliente y el técnico existen
            if (!await _contexto.Clientes.AnyAsync(c => c.ClienteID == ticket.ClienteID))
                throw new InvalidOperationException("El cliente especificado no existe.");

            if (!await _contexto.Tecnicos.AnyAsync(t => t.TecnicoID == ticket.TecnicoID))
                throw new InvalidOperationException("El técnico especificado no existe.");

            ticket.Fecha = DateTime.Now; // Asigna la fecha actual al ticket
            _contexto.Tickets.Add(ticket);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Editar un ticket existente
        public async Task<bool> Editar(Tickets ticket)
        {
            var ticketExistente = await _contexto.Tickets.FindAsync(ticket.TicketID);

            if (ticketExistente == null)
                throw new InvalidOperationException("El ticket no existe.");

            ticketExistente.Prioridad = ticket.Prioridad;
            ticketExistente.Asunto = ticket.Asunto;
            ticketExistente.Descripcion = ticket.Descripcion;
            ticketExistente.TiempoInvertido = ticket.TiempoInvertido;

            if (ticket.TecnicoID != ticketExistente.TecnicoID)
            {
                // Cambiar el técnico asignado si es necesario
                if (!await _contexto.Tecnicos.AnyAsync(t => t.TecnicoID == ticket.TecnicoID))
                    throw new InvalidOperationException("El nuevo técnico no existe.");

                ticketExistente.TecnicoID = ticket.TecnicoID;
            }

            if (ticket.ClienteID != ticketExistente.ClienteID)
            {
                // Cambiar el cliente asignado si es necesario
                if (!await _contexto.Clientes.AnyAsync(c => c.ClienteID == ticket.ClienteID))
                    throw new InvalidOperationException("El nuevo cliente no existe.");

                ticketExistente.ClienteID = ticket.ClienteID;
            }

            _contexto.Tickets.Update(ticketExistente);
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(Tickets ticket)
        {
            var ticketExistente = await _contexto.Tickets.FindAsync(ticket.TicketID);

            if (ticketExistente == null)
            {
                // Validar que el cliente y técnico existan
                if (ticket.ClienteID > 0 && !await _contexto.Clientes.AnyAsync(c => c.ClienteID == ticket.ClienteID))
                    ticket.ClienteID = 0; // Asignar como "sin cliente"

                if (ticket.TecnicoID > 0 && !await _contexto.Tecnicos.AnyAsync(t => t.TecnicoID == ticket.TecnicoID))
                    ticket.TecnicoID = 0; // Asignar como "sin técnico"

                ticket.Fecha = DateTime.Now; // Fecha actual
                _contexto.Tickets.Add(ticket);
            }
            else
            {
                // Editar ticket existente
                ticketExistente.Prioridad = ticket.Prioridad;
                ticketExistente.Asunto = ticket.Asunto;
                ticketExistente.Descripcion = ticket.Descripcion;
                ticketExistente.TiempoInvertido = ticket.TiempoInvertido;

                if (ticket.TecnicoID != ticketExistente.TecnicoID)
                {
                    ticketExistente.TecnicoID = (await _contexto.Tecnicos.AnyAsync(t => t.TecnicoID == ticket.TecnicoID))
                        ? ticket.TecnicoID
                        : 0; // Si no existe, marcar como "sin técnico"
                }

                if (ticket.ClienteID != ticketExistente.ClienteID)
                {
                    ticketExistente.ClienteID = (await _contexto.Clientes.AnyAsync(c => c.ClienteID == ticket.ClienteID))
                        ? ticket.ClienteID
                        : 0; // Si no existe, marcar como "sin cliente"
                }

                _contexto.Tickets.Update(ticketExistente);
            }

            return await _contexto.SaveChangesAsync() > 0;
        }

        // Buscar un ticket por ID
        public async Task<Tickets> Buscar(int ticketID)
        {
            var ticket = await _contexto.Tickets
                .Include(t => t.Tecnico) // Incluir técnico relacionado
                .Include(t => t.Cliente) // Incluir cliente relacionado
                .FirstOrDefaultAsync(t => t.TicketID == ticketID);

            if (ticket == null)
                throw new InvalidOperationException("El ticket no existe.");

            return ticket;
        }

        // Listar tickets que cumplen con un criterio
        public async Task<List<Tickets>> Listar(Expression<Func<Tickets, bool>> criterio)
        {
            return await _contexto.Tickets
                .Include(t => t.Tecnico) // Incluir técnico relacionado
                .Include(t => t.Cliente) // Incluir cliente relacionado
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        // Eliminar un ticket por ID
        public async Task<bool> Eliminar(int ticketID)
        {
            var ticket = await _contexto.Tickets.FindAsync(ticketID);

            if (ticket == null) return false;

            _contexto.Tickets.Remove(ticket);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Asignar un técnico a un ticket
        public async Task<bool> AsignarTecnico(int ticketID, int tecnicoID)
        {
            var ticket = await _contexto.Tickets.FindAsync(ticketID);

            if (ticket == null)
                throw new InvalidOperationException("El ticket no existe.");

            if (!await _contexto.Tecnicos.AnyAsync(t => t.TecnicoID == tecnicoID))
                throw new InvalidOperationException("El técnico no existe.");

            ticket.TecnicoID = tecnicoID;
            _contexto.Tickets.Update(ticket);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Asignar un cliente a un ticket
        public async Task<bool> AsignarCliente(int ticketID, int clienteID)
        {
            var ticket = await _contexto.Tickets.FindAsync(ticketID);

            if (ticket == null)
                throw new InvalidOperationException("El ticket no existe.");

            if (!await _contexto.Clientes.AnyAsync(c => c.ClienteID == clienteID))
                throw new InvalidOperationException("El cliente no existe.");

            ticket.ClienteID = clienteID;
            _contexto.Tickets.Update(ticket);
            return await _contexto.SaveChangesAsync() > 0;
        }

        // Marcar ticket como "sin técnico" o "sin cliente" si se elimina uno de ellos
        public async Task VerificarTicketsHuérfanos()
        {
            // Actualizar tickets que ya no tienen técnico
            var ticketsSinTecnico = await _contexto.Tickets
                .Where(t => !_contexto.Tecnicos.Any(te => te.TecnicoID == t.TecnicoID))
                .ToListAsync();

            foreach (var ticket in ticketsSinTecnico)
            {
                ticket.TecnicoID = 0; // Valor 0 como indicador de "sin técnico"
                ticket.Tecnico = null;
            }

            // Actualizar tickets que ya no tienen cliente
            var ticketsSinCliente = await _contexto.Tickets
                .Where(t => !_contexto.Clientes.Any(c => c.ClienteID == t.ClienteID))
                .ToListAsync();

            foreach (var ticket in ticketsSinCliente)
            {
                ticket.ClienteID = 0; // Valor 0 como indicador de "sin cliente"
                ticket.Cliente = null;
            }

            await _contexto.SaveChangesAsync();
        }
    }
}