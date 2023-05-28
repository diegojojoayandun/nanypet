using Microsoft.EntityFrameworkCore;
using NanyPet;
using NanyPet.Models;
using NanyPet.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.EnableAnnotations(); });

builder.Services.AddDbContext<nanypetContext>(option =>
{
    option.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), ServerVersion.Parse("8.0.30-mysql"));

});

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IHerderRepository, HerderRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();


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
