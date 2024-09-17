using MagicVilla_VillaApi.DataFolder;
using MagicVilla_VillaApi.Models;
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

app.MapGet("/api/coupon", () => Results.Ok(CouponStore.couponList));

app.MapGet("/api/coupon/{id:int}", (int id) =>
{
    return Results.Ok(CouponStore.couponList.FirstOrDefault(user => user.Id == id));
});

app.MapPost("/api/coupon", ([FromBody] Coupon coupon) =>
{
    if ((coupon.Id != 0) || string.IsNullOrEmpty(coupon.Name))
    {
        return Results.BadRequest("Invalid id or coupon name");

    }
    if(CouponStore.couponList.FirstOrDefault(cpn => cpn.Name.ToLower() == coupon.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon alreday exists");
    }

    coupon.Id = CouponStore.couponList.OrderByDescending(cpn => cpn.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);
    return Results.Ok(coupon);
});


app.UseHttpsRedirection();


app.Run();

