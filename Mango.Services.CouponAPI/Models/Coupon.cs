using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]

        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmmount { get; set; }
        public int MinAmmount { get; set; }
    }
}
