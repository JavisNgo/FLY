using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FLY.Business.Models.Account;

namespace FLY.Business.Models.Blog
{
    public class BlogResponse
    {
        public int BlogId { get; set; }

        public int AccountId { get; set; }

        public string BlogName { get; set; }

        public DateTime BlogDate { get; set; }

        public string BlogContent { get; set; }

        public string BlogImage { get; set; }
        public AccountResponse? Account { get; set; }
        public int Status { get; set; }

    }
}
