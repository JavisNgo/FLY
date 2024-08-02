namespace FLY.Business.Models.Blog
{
    public class UpdateBlogRequest
    {
         public int BlogId { get; set; }

        public int AccountId { get; set; }

        public string BlogName { get; set; }

        public DateTime BlogDate { get; set; }

        public string BlogContent { get; set; }

        public string BlogImage { get; set; }
    }
}
