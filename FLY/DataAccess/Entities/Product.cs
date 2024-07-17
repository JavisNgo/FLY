using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FLY.DataAccess.Entities;

public partial class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }

    [Required]
    public int ShopId { get; set; }

    public int? VoucherId { get; set; }

    [Required]
    public int ProductCategoryId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProductName { get; set; }

    [Required]
    [MaxLength(250)]
    public string ProductInfor { get; set; }

    [Required]
    public float ProductPrice { get; set; }

    [Required]
    public int ProductQuatity { get; set; }

    [Required]
    [MaxLength(250)]
    public string ImageProduct { get; set; }

    [Required]
    public int Status { get; set; }

    [ForeignKey("ShopId")]
    public Shop Shop { get; set; }

    [ForeignKey("VoucherId")]
    public VoucherOfshop Voucher { get; set; }

    [ForeignKey("ProductCategoryId")]
    public ProductCategory ProductCategory { get; set; }

    public ICollection<Cart> Carts { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
