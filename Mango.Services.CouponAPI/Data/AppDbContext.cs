using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {   
        //pass what we get in ctor to base class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        //coupons is table name 
        public DbSet<Coupon> Coupons { get; set; }
        //seeding
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon 
            {
                CouponId = 1,
                CouponCode = "10FFx",
                DiscountAmmount = 10,
                MinAmmount = 10,

            }, new Coupon
            {
                CouponId = 2,
                CouponCode = "F011x",
                DiscountAmmount = 5,
                MinAmmount = 20,

            }

            );

        }
    }
}
