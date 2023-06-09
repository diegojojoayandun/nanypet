using DotEnv.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NanyPet;
using NanyPet.Api.Models;
using NanyPet.Api.Repositories.IRepository;
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

// Swagger, Custom for Bearer Authorization  
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresar Bearer [space] tuToken \r\n\r\n " +
                      "Ejemplo: Bearer 123456abcder",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Scheme = "oauth2",
                Name="Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddResponseCaching();

// DbContext
builder.Services.AddDbContext<nanypetContext>(option =>
{
    option.UseSqlServer(Configuration["CONNECTION_STRING"]);
});

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<nanypetContext>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IHerderRepository, HerderRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

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

// External Authentication(Google, Facebook, ...)
builder.Services.AddAuthentication(options =>
{

    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
{
    options.LoginPath = "/api/signin-google";
    // options.LoginPath = "/account/facebook-login";
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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
