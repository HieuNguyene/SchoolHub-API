namespace SchoolHub.Application.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public int StatusCode { get; set; } = 200;
        public string? TraceId { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Success") => new() { Success = true, StatusCode = 200, Message = message, Data = data };
        public static ApiResponse<T> Fail(string message, int statusCode = 400) => new() { Success = false, StatusCode = statusCode, Message = message };
        public static ApiResponse<T> NotFound(string message = "Không tìm thấy tài nguyên yêu cầu") => new() { Success = false, StatusCode = 404, Message = message };
        public static ApiResponse<T> BadRequest(string message = "Yêu cầu không hợp lệ") => new() { Success = false, StatusCode = 400, Message = message };
    }
}

