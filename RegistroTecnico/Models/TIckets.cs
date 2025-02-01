
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Models
{
    public class Tickets
    {
        [Key]
        public int TicketID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(50)] // Limita el tamaño del campo
        public string? Prioridad { get; set; }

       

        [Required]
        [StringLength(100)] // Máximo 100 caracteres
        public string? Asunto { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        [Range(0, 24)]
        public decimal TiempoInvertido { get; set; } // En horas

       
       
        [ForeignKey("Tecnicos")]

        public int TecnicoID { get; set; }
        public Tecnicos? Tecnico { get; set; } // Relación con la tabla Técnicos


        [ForeignKey("Clientes")]

        public int ClienteID { get; set; }

        public Clientes? Cliente { get; set; }


    }
}