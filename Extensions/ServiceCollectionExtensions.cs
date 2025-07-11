using System.Buffers;
using System.Reflection;
using System.Text;
using System.Text.Unicode;
using Api.Config;
using Api.Extensions;
using Api.Interfaces;
using Api.Models;
using Api.Repositories;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSwaggerGen(c =>
       {
           c.SwaggerDoc("v1", new OpenApiInfo
           {
               Title = "Stockify API",
               Version = "v1",
               Description = "Stockify is a platform to manage stocks and portfolios. This API provides access to user accounts, trading data, and admin operations.",
               Contact = new OpenApiContact
               {
                   Name = "Stockify Team",
                   Email = "support@stockify.com"
               },
               License = new OpenApiLicense
               {
                   Name = "MIT License",
                   Url = new Uri("https://opensource.org/licenses/MIT")
               }
           });

           var jwtSecurityScheme = new OpenApiSecurityScheme
           {
               BearerFormat = "JWT",
               Name = "Authorization",
               In = ParameterLocation.Header,
               Type = SecuritySchemeType.Http,
               Scheme = "bearer",
               Description = "Enter your JWT token in the format: Bearer {your token}",

               Reference = new OpenApiReference
               {
                   Id = JwtBearerDefaults.AuthenticationScheme,
                   Type = ReferenceType.SecurityScheme
               }
           };

           c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

           c.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
        { jwtSecurityScheme, Array.Empty<string>() }
           });

           var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
           var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
           c.IncludeXmlComments(xmlPath);
       });




        services.AddDbContext<AppDbContext>(options =>
         options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IstockRepository, StockRepository>();
        services.AddScoped<IcommentRepository, CommentRepository>();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.Configure<JwtSettings>(configuration.GetSection("JWT"));
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))


            };
        });

        services.Configure<EmailSettings>(
        configuration.GetSection("EmailSettings"));

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
