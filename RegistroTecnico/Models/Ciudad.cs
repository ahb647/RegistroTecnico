using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico.Models
{
    public class Ciudad
    {

        [Key]
        public int CiudadID { get; set; }
        public string? CiudadNombre { get; set; }

    }
}
