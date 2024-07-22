using System.ComponentModel.DataAnnotations;

namespace FLY.Business.Models.Carte
{
    public class SubCartRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int AccountId { get; set; }
    }
}
