using Microsoft.AspNetCore.Mvc;
using SchoolHub.Application.DTOs;

namespace SchoolHub.API.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleResult<T>(ApiResponse<T> result)
        {
            if (result.Success)
            {
                return Ok(result);
            }

            return result.StatusCode switch
            {
                StatusCodes.Status404NotFound => NotFound(result),
                StatusCodes.Status401Unauthorized => Unauthorized(result),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status409Conflict => Conflict(result),
                _ => BadRequest(result)
            };
        }
    }
}
