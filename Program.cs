using AutoMapper;
using FluentValidation;
using MagicVilla_VillaApi;
using MagicVilla_VillaApi.DataFolder;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Inject automapper into the pipleine
builder.Services.AddAutoMapper(typeof(MappingConfig));
//inject validations
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program> _logger) => 
{
    _logger.Log(LogLevel.Information, "Getting all coupons");
    return Results.Ok(CouponStore.couponList);
}
);

app.MapGet("/api/coupon/{id:int}", (int id) =>
{
    return Results.Ok(CouponStore.couponList.FirstOrDefault(user => user.Id == id));
});

app.MapPost("/api/coupon", (IMapper _mapper,
    IValidator <CouponCreateDto> _validation, [FromBody] CouponCreateDto couponCDto) =>
{
    //check for validations
    var validationResult = _validation.ValidateAsync(couponCDto).GetAwaiter().GetResult();

    if (!validationResult.IsValid)
    {

        return Results.BadRequest(validationResult.Errors.FirstOrDefault().ToString());

    }
    if(CouponStore.couponList.FirstOrDefault(cpn => cpn.Name.ToLower() == couponCDto.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon alreday exists");
    }

    //convert the dto into a coupon before adding to the db
    Coupon coupon = _mapper.Map<Coupon>(couponCDto);

    coupon.Id = CouponStore.couponList.OrderByDescending(cpn => cpn.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);

    
    //update the coupondto so that it returns specified data to the client
    CouponDto couponDto = _mapper.Map<CouponDto>(coupon);

    return Results.Ok(couponDto);
});


app.UseHttpsRedirection();


app.Run();

