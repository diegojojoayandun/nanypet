using DotEnv.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NanyPet;
using NanyPet.Api.Models;
using NanyPet.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

var Configuration = builder.Configuration; // Forma de usar IConfiguration Interfaz en net core 6.0 o superior

new EnvLoader().Load();
Configuration.AddEnvironmentVariables();
//builder.Configuration.AddEnvironmentVariables();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.EnableAnnotations(); });

builder.Services.AddDbContext<nanypetContext>(option =>
{
    option.UseMySql(Configuration["CONNECTION_STRING"], ServerVersion.Parse("8.0.30-mysql"));

});

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IHerderRepository, HerderRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SECRET_KEY"]))
    };
});


builder.Services.AddAuthentication(options =>
{

    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
{
    options.LoginPath = "/api/signin-google";
    //options.LoginPath = "/account/facebook-login";
})
.AddGoogle(options =>
{
    options.ClientId = Configuration["CLIENT_ID"];
    options.ClientSecret = Configuration["CLIENT_SECRET"];
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseCors();
app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
