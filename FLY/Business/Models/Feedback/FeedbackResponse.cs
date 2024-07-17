namespace FLY.Business.Models.Feedback
{
    public class FeedbackResponse
    {
        public int FeedbackId { get; set; }

        public string Email { get; set; }

        public string ShopName { get; set; }

        public string Content { get; set; } = null!;

        public int Status { get; set; }
    }
}
