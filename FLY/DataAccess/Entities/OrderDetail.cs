using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FLY.DataAccess.Entities;

public partial class OrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int OrderQuantity { get; set; }

    [Required]
    public float ProductPrice { get; set; }

    [Required]
    public int Status { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}
