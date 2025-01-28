using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Models
{
    public class Clientes
    {
        [Key]
        public int ClienteID { get; set; }

      
        

        
       

        [Required]
        [StringLength(100)]
        public string? Nombres { get; set; }

        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El RNC es obligatorio.")]
        [Range(100000000, 999999999, ErrorMessage = "El RNC debe tener 9 dígitos.")]
        public int Rnc { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El límite de crédito debe ser positivo.")]
        public decimal LimiteCredito { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }


        [ForeignKey("Tecnicos")]

        public int TecnicoID { get; set; }
        public Tecnicos? Tecnico { get; set; }

        
    }
}