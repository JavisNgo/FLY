using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FLY.DataAccess.Entities;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    [Required]
    public int AccountId { get; set; }
    [Required]
    public int ShopId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public float TotalPrice { get; set; }

    [Required]
    public int Status { get; set; }

    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    [ForeignKey("ShopId")]
    public Shop Shop { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}
