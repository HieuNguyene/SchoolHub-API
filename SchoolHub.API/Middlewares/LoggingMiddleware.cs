using SchoolHub.Application.DTOs;
using SchoolHub.Application.Validations;
using SchoolHub.Application.Interfaces;
using System.Diagnostics;

namespace SchoolHub.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("----RESQUEST----");
            _logger.LogInformation($"Method : {context.Request.Method}");
            _logger.LogInformation($"Path : {context.Request.Path}");
            _logger.LogInformation($"Time : {DateTime.Now}");

            await _next(context);

            _logger.LogInformation("----RESPONSE----");
            _logger.LogInformation($"Status: {context.Response.StatusCode}");
            _logger.LogInformation($"Excution time: {stopwatch.ElapsedMilliseconds}ms");

        }
    }
}








