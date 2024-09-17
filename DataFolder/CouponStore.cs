using MagicVilla_VillaApi.Models;


namespace MagicVilla_VillaApi.DataFolder
{
    public class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon>
        {
            new Coupon{Id = 1, Name = "100FF", IsActive = true},
            new Coupon{Id = 2, Name = "200FF", IsActive=false},
        };
    }
}
