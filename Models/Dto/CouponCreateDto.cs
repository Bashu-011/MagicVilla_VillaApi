namespace MagicVilla_VillaApi.Models.Dto
{
    public class CouponCreateDto
    {
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
}
