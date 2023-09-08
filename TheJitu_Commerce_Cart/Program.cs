using Microsoft.EntityFrameworkCore;
using TheJitu_Commerce_Cart.Data;
using TheJitu_Commerce_Cart.Extensions;
using TheJitu_Commerce_Cart.Services;
using TheJitu_Commerce_Cart.Services.Iservice;
using TheJitu_Commerce_Cart.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection to DB

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});
//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor();
//Services
builder.Services.AddScoped<ICartservice, CartService>();
builder.Services.AddScoped<IProductInterface, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
//Registering the Base Url for the services
builder.Services.AddHttpClient("Product", c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrl:ProductApi"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Coupon", c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrl:CouponApi"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
    
//custom builders

builder.AddSwaggenGenExtension();
builder.AddAppAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//migration
app.UseMigration();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
