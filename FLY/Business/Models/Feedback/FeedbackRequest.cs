using System.ComponentModel.DataAnnotations;

namespace FLY.Business.Models.Feedback
{
    public class FeedbackRequest
    {
        [Required]
        public int ShopId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string Content { get; set; } = null!;
    }
}
