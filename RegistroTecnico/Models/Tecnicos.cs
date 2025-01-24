using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico.Models
{
    public class Tecnicos
    {
        [Key]
        public int TecnicoID { get; set; }

        [Required(ErrorMessage = "este campo es obligatorio")]
        public string Nombre { get; set; } = string.Empty!; // para que nombre no sea nulo
        public double SueldoHora { get; set; }
    }
}