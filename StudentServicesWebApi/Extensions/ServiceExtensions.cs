using System.Text;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentServicesWebApi.Application.Mappings;
using StudentServicesWebApi.Infrastructure;
using StudentServicesWebApi.Infrastructure.Configuration;
using StudentServicesWebApi.Infrastructure.Interfaces;
using StudentServicesWebApi.Infrastructure.Repositories;
using StudentServicesWebApi.Infrastructure.Services;

namespace StudentServicesWebApi.Extensions;

public static class ServiceExtensions
{

    /// Adds database context with PostgreSQL configuration

    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("StudentServicesWebApi"));
        });

        return services;
    }


    /// Adds AutoMapper configuration

    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(NotificationProfile).Assembly);
        return services;
    }


    /// Adds FluentValidation configuration

    public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<StudentServicesWebApi.Application.Validators.CreateNotificationRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentServicesWebApi.Application.Validators.RegisterDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentServicesWebApi.Application.Validators.ForgotPasswordDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentServicesWebApi.Application.Validators.CreatePaymentDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentServicesWebApi.Application.Validators.ProcessPaymentDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentServicesWebApi.Application.Validators.CreateTextSlideDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<StudentServicesWebApi.Application.Validators.CreatePhotoSlideDtoValidator>();

        return services;
    }



    /// Adds application configuration options

    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramBotConfiguration>(configuration.GetSection(TelegramBotConfiguration.SectionName));
        services.Configure<JwtConfiguration>(configuration.GetSection(JwtConfiguration.SectionName));

        return services;
    }


    /// Adds all application services and repositories

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IAdminActionRepository, AdminActionRepository>();
        services.AddScoped<ITextSlideRepository, TextSlideRepository>();
        services.AddScoped<IPhotoSlideRepository, PhotoSlideRepository>();

        // Services
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVerificationCodeService, VerificationCodeService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IAdminActionService, AdminActionService>();
        services.AddScoped<ITextSlideService, TextSlideService>();
        services.AddScoped<IPhotoSlideService, PhotoSlideService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IDataSeeder, DataSeeder>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IDtoMappingService, DtoMappingService>();

        // Singleton services
        services.AddSingleton<ITelegramBotService, TelegramBotService>();
        
        // Add HttpContextAccessor for URL generation
        services.AddHttpContextAccessor();

        return services;
    }


    /// Adds JWT authentication configuration

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection(JwtConfiguration.SectionName).Get<JwtConfiguration>();
        var key = Encoding.ASCII.GetBytes(jwtConfig!.SecretKey);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        return services;
    }


    /// Adds hosted services

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<TelegramBotHostedService>();
        return services;
    }


    /// Adds CORS configuration

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        
        return services;
    }


    /// Adds Swagger/OpenAPI configuration with API versioning support

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        // Configure Swagger
        services.AddSwaggerGen(c =>
        {
            //c.SwaggerDoc("v1", new OpenApiInfo
            //{
            //    Version = "3.1.0",
            //    Title = "Student Services API",
            //    Description = "API for student services management",
            //    Contact = new OpenApiContact
            //    {
            //        Name = "Student Services Team",
            //        Email = "support@studentservices.com"
            //    },
            //    License = new OpenApiLicense
            //    {
            //        Name = "MIT",
            //        Url = new Uri("https://opensource.org/licenses/MIT")
            //    }
            //});

            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Student Services API",
                Version = "v1",
                Description = "API for student services management",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Student Services Team",
                    Email = "support@studentservices.com"
                },
                License = new Microsoft.OpenApi.Models.OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });


            // Include XML comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Enable annotations for better schema documentation
            c.EnableAnnotations();

            // Add JWT Bearer Authentication
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}