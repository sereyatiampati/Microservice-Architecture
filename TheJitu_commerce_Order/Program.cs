using Microsoft.EntityFrameworkCore;
using TheJitu_commerce_Order.Data;
using TheJitu_commerce_Order.Extensions;
using TheJitu_commerce_Order.Services;
using TheJitu_commerce_Order.Services.Iservice;
using TheJituMessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});
//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IMessageBus, MessageBus>();

//Services
builder.Services.AddScoped<IOrderService,OrderService>();
var app = builder.Build();


//Register for Stripe
Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Key").Get<string>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMigration();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
