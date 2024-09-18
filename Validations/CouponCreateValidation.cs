using FluentValidation;
using MagicVilla_VillaApi.Models.Dto;

namespace MagicVilla_VillaApi.Validations
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDto>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
