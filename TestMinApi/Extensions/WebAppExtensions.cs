using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TestMinApi.Data;
using TestMinApi.Dto;
using TestMinApi.Middleware;
using TestMinApi.Services;

namespace TestMinApi.Extensions
{
    public static class WebAppExtensions
    {
        public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            var assembly = Assembly.GetExecutingAssembly();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            builder.Services.AddValidatorsFromAssemblies(new List<Assembly>() { assembly });
            builder.Services.AddAutoMapper(new List<Assembly>() { assembly });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SampleAuth0",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //add memory db
            var optionsBuilder = new DbContextOptionsBuilder<DemoContext>()
                .UseInMemoryDatabase("test");
            var _options = optionsBuilder.Options;

            builder.Services.AddSingleton<DbContextOptions<DemoContext>>(_options);
            builder.Services.AddSingleton<DemoContext>(new DemoContext(_options));

            builder.Services.AddScoped<ArticleService>();

            return builder;
        }

        public static WebApplicationBuilder AddJWTAuthentication(this WebApplicationBuilder builder)
        {

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true
                    };
                });

            var policy = new AuthorizationPolicyBuilder()
                                 .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                 .RequireAuthenticatedUser()
                                 .RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                                 .Build();

            var adminPolicy = new AuthorizationPolicyBuilder()
                              .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                              .RequireAuthenticatedUser()
                              .RequireRole("admin")
                              .RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                              .Build();

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", adminPolicy);
                opt.AddPolicy("default", policy);
                opt.FallbackPolicy = policy;
            });

            return builder;
        }

        public static WebApplication ConfigureServices(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            var ctx = app.Services.GetService<DemoContext>();
            ctx.Seed();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }


    }
}
