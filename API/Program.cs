using API.Data;
using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

// Register the BackgroundTaskService
builder.Services.AddHostedService<BackgroundTaskService>();

// Register the BackgroundTaskService as a singleton for dependency injection
builder.Services.AddSingleton<BackgroundTaskService>();
builder.Services.AddHostedService<BackgroundTaskService>(provider => provider.GetService<BackgroundTaskService>());

//builder.Services.AddHostedService<API.Services.BackgroundTaskService>();


var app = builder.Build();

// app.UseAuthorization();

//Configre the HTTP 
// Add this in ConfigureServices method (for .NET 5 and below) or in the builder (for .NET 6 and above)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularOrigin",
//        policy =>
//        {
//            policy.WithOrigins("http://192.168.1.169:4200") // Replace with your Angular frontend IP and port
//                .AllowAnyHeader()
//                .AllowAnyMethod();
//        });
//});

// Use the CORS policy in Configure method (for .NET 5 and below) or after app creation (for .NET 6 and above)
app.UseCors("AllowAngularOrigin");

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod());

//middleware-
//asks if the token is valid
app.UseAuthentication();
//checks what you are allowed to do with that token
app.UseAuthorization();

app.MapControllers();

app.Run();

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddApplicationServices(builder.Configuration);

//builder.Services.AddDbContext<DataContext>(opt => 
//{
//    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

////CORS
//builder.Services.AddCors();

////Jason Web Tokens-
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});

//var app = builder.Build();

//// app.UseAuthorization();

////Configre the HTTP 
//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

////middleware-
////asks if the token is valid
//app.UseAuthentication();
////checks what you are allowed to do with that token
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
