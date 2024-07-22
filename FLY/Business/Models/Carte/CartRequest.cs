using System.ComponentModel.DataAnnotations;

namespace FLY.Business.Models.Carte
{
    public class CartRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int CartQuantity { get; set; }
    }
}
