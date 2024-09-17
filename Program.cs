using MagicVilla_VillaApi.DataFolder;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapPost("/api/coupon", ([FromBody] CouponCreateDto couponCDto) =>
{
    if (string.IsNullOrEmpty(couponCDto.Name))
    {
        return Results.BadRequest("Invalid id or coupon name");

    }
    if(CouponStore.couponList.FirstOrDefault(cpn => cpn.Name.ToLower() == couponCDto.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon alreday exists");
    }

    //convert the dto into a coupon before adding to the db
    Coupon coupon = new()
    {
        IsActive = couponCDto.IsActive,
        Name = couponCDto.Name,
        Percent = couponCDto.Percent,
    };

    coupon.Id = CouponStore.couponList.OrderByDescending(cpn => cpn.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);

    
    //update the coupondto so that it returns specified data to the client
    CouponDto couponDto = new()
    {
        Id = coupon.Id,
        Name = coupon.Name,
        Percent = coupon.Percent,
        IsActive = coupon.IsActive,
        Created = coupon.Created
    };

    return Results.Ok(couponDto);
});


app.UseHttpsRedirection();


app.Run();

