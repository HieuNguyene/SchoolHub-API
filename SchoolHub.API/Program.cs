using SchoolHub.Application.DTOs;
using SchoolHub.Application.Validations;
using SchoolHub.Application.Interfaces;
using SchoolHub.API.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;



using SchoolHub.API.Middlewares;

using SchoolHub.Infrastructure.Repositories.Implementations;
using SchoolHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Infrastructure.Services;
using SchoolHub.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.OpenApi.Models;

namespace SchoolHub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Cấu hình CORS cho phép Frontend kết nối
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            // Cấu hình JWT
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
            builder.Services.AddAuthentication(options =>
            {
                 // Báo cho Server: "Mặc định hãy tìm và kiểm tra thẻ JWT Bearer"
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Khi có API đến tìm thẻ JWT 
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;// Nếu ko có hay hết hạn thì đẩy lỗi 401 
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key)),// Tạo một khóa đối xứng rồi truyền key đã chuyển sang dạng byte

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer, // Kiểm tra xem có đúng nơi phát hành không

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience, // Kiểm tra xem có đúng người sử dụng không

                    // Kiểm tra hết hạn dùng: Quá 10p từ chối lập tức 
                    ValidateLifetime =true,
                    ClockSkew = TimeSpan.Zero  //Hết hạn đúng từng giây, không cho trễ   
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("CanManageStudents", policy =>
                    policy.RequireRole("Admin","Teacher"));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SchoolHub API",
                    Version = "v1",
                    Description = "Enterprise School & Student Management RESTful API built with Clean Architecture, CQRS, and .NET 8"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập Access Token của bạn vào đây (Swagger sẽ tự thêm tiền tố Bearer)"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddMemoryCache();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Đăng ký IDbConnection cho Dapper
            builder.Services.AddScoped<System.Data.IDbConnection>(sp =>
                new Microsoft.Data.SqlClient.SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}










