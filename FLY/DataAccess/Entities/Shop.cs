using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FLY.DataAccess.Entities;

public partial class Shop
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShopId { get; set; }

    [Required]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ShopName { get; set; }

    [Required]
    [MaxLength(250)]
    public string ShopDetail { get; set; }

    [Required]
    [MaxLength(250)]
    public string ShopAddress { get; set; }

    [Required]
    public DateTime ShopStartTime { get; set; }

    [Required]
    public DateTime ShopEndTime { get; set; }

    [Required]
    public int Status { get; set; }

    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    public ICollection<Product> Products { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<VoucherOfshop> VoucherOfshops { get; set; }

}
