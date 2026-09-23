using SchoolHub.Application.DTOs;
using SchoolHub.Application.Validations;
using SchoolHub.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SchoolHub.Infrastructure.Repositories.Implementations;
using SchoolHub.Application.Behaviors;
using FluentValidation;
using MediatR;
using AutoMapper;
using SchoolHub.Infrastructure.Services;

namespace SchoolHub.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(SchoolHub.Application.Features.Students.Commands.CreateStudentCommand).Assembly;

            services.AddValidatorsFromAssembly(applicationAssembly);

            // Kích hoạt Trạm gác Pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

            services.AddAutoMapper(cfg => cfg.AddMaps(applicationAssembly));
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IScoreRepository, ScoreRepository>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}







