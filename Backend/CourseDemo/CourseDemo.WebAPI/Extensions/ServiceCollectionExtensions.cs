using CourseDemo.Application.Services;
using CourseDemo.Domain.Interfaces;
using CourseDemo.Infrastructure.Data;
using CourseDemo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseDemo.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<ICourseService, CourseService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<ICourseRepository, CourseRepository>();

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Controllers and Swagger
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Course Management API",
                    Version = "v1",
                    Description = "API for managing courses - create, edit, publish, and archive courses"
                });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    builder
                        .WithOrigins(
                            "http://localhost:3000", 
                            "https://localhost:3000",
                            "http://coursedemo-alb-1005453574.us-east-1.elb.amazonaws.com",
                            "https://coursedemo-alb-1005453574.us-east-1.elb.amazonaws.com"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}