using System.ComponentModel.DataAnnotations;

namespace CasaDoCodigo.Models
{
    public class Categoria : BaseModel
    {
        public Categoria()
        {
        }
        [Required]
        public string Nome { get; set; }
    }
}
