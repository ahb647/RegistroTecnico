using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico.Models
{
    public class Clientes 
    {

        [Key]
        public int ClienteID { get; set; }
        public int TecnicoID { get; set; }

        public string? Nombres {  get; set; }    

        public string? Direccion {  get; set; }
        public int Rnc {  get; set; }

        public Decimal LimiteCredito { get; set; }
        public DateTime FechaIngreso { get; set; }  



    }
}
