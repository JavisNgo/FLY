using FLY.Business.Models.Account;

namespace FLY.Business.Models.Blog
{
    public class BlogRequest
    {
        public int AccountId { get; set; }
        public int BlogId { get; set; }

        public string BlogName { get; set; }

        public DateTime BlogDate { get; set; }

        public string BlogContent { get; set; }

        public string BlogImage { get; set; }

        public AuthResponse Account { get; set; }
    }
}
