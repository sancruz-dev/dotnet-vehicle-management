using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using minimal_api;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Services;
using minimal_api.infrastructure;
using minimal_api.infrastructure.Auth;
using minimal_api.infrastructure.DB;

public class Startup
{
    public IConfiguration Configuration { get; set; } 

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        // Registra JwtSettings para injeção
        services.Configure<JwtSettings>(
            Configuration.GetSection("Jwt")
        );

        // Pega a key direto da config (para o TokenValidationParameters)
        var jwtSection = Configuration.GetSection("Jwt");
        var key = jwtSection.GetValue<string>("Key") ?? "123456";


        if (string.IsNullOrEmpty(key)) key = "123456";

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
          .AddJwtBearer(option =>
          {
              option.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateLifetime = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                  ValidateIssuer = false,
                  ValidateAudience = false
              };
          });

        services.AddAuthorization();

        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IVeiculoService, VeiculoService>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT aqui"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
              {
                {
                  new OpenApiSecurityScheme {
                    Reference = new OpenApiReference{
                      Type = ReferenceType.SecurityScheme,
                      Id = "Bearer"
                    }
                  },
                  new string[] {}
                }
              });
        });

        services.AddDbContext<MinimalApiContext>(options =>
        {
            options.UseNpgsql(
                Configuration.GetConnectionString("Postgresql")
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors("AllowAngular");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}