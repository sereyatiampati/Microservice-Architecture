using Microsoft.EntityFrameworkCore;
using TheJitu_commerce_EmailService.Extensions;
using TheJitu_commerce_Rewards.Data;
using TheJitu_commerce_Rewards.Extensions;
using TheJitu_commerce_Rewards.Messaging;
using TheJitu_commerce_Rewards.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});


var dbContextBuilder = new DbContextOptionsBuilder<AppDBContext>();
dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
builder.Services.AddSingleton(new RewardsService(dbContextBuilder.Options));
//services
builder.Services.AddSingleton<IAzureMessaageBusConsumer, AzureMessageBusConsumer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMigration();
app.useAzure();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
