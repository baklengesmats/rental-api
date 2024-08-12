using RentalApp.Entities;
using RentalApp.Factory;
using RentalApp.Repositories;
using RentalApp.Repositories.Mock;
using RentalApp.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<MockDataService>();

builder.Services.AddScoped<IPriceFactory, PriceFactory>();

builder.Services.AddScoped<IRentalRepository, MockRentalRepository>();
builder.Services.AddScoped<IReturnRepository, MockReturnRepository>();
builder.Services.AddScoped<IRentalObjectRepository, MockRentalObjectRepository>();

builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IReturnService, ReturnService>();
builder.Services.AddScoped<IRentalObjectService, RentalObjectService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
