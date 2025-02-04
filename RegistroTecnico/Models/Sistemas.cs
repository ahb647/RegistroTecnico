using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Models
{
    public class Sistemas
    {


        [Key] 
        public int SistemaId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")] // Campo obligatorio
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres.")] // Longitud máxima
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La complejidad es obligatoria.")] // Campo obligatorio
        [Range(0, 100, ErrorMessage = "La complejidad debe estar entre 0 y 100.")] // Rango válido
        public double Complejidad { get; set; }





    }
}
