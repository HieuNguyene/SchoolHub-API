using SchoolHub.Application.DTOs;
using SchoolHub.Application.Validations;
using SchoolHub.Application.Interfaces;
using System.Text.Json;
using FluentValidation;

namespace SchoolHub.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra trong quá trình xử lý Request");
                context.Response.ContentType = "application/json";
                var response = new ApiResponse<object>
                {
                    Success = false,
                    Data = null
                };

                switch (ex)
                {
                    case ValidationException validationEx:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        response.Message = "Dữ liệu không hợp lệ";
                        var errors = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                        response.Data = errors;
                        break;
                    case KeyNotFoundException:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        response.Message = "Không tìm thấy tài nguyên yêu cầu.";
                        break;
                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.Message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.";
                        break;
                }
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}









