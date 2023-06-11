using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Create an instance of JsonSerializerOptions
var options = new JsonSerializerOptions
{
    ReferenceHandler = ReferenceHandler.Preserve
};

// Serialize your object using the options
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).
                WriteTo.File("C:\\Users\\Islam\\Desktop\\Looger\\Log.txt", rollingInterval: RollingInterval.Day).
                CreateLogger();
builder.Services.AddDbContext<RestaurantContext>(cnn => cnn.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<RestaurantContext>();
builder.Services.AddScoped<IMenuRepo, MenuRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("TokenSecrect"));

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
