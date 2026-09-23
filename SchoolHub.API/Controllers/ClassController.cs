using Microsoft.AspNetCore.Mvc;
using MediatR;
using SchoolHub.Application.Features.Classes.Commands;
using SchoolHub.Application.Features.Classes.Queries;
using Microsoft.AspNetCore.Authorization;

namespace SchoolHub.API.Controllers
{
    [Authorize]
    [Route("api/class")]
    [ApiController]
    public class ClassController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public ClassController(IMediator mediator) => _mediator = mediator;

        // Bất kỳ ai đã đăng nhập đều có thể xem danh sách lớp
        [HttpGet]
        public async Task<IActionResult> GetAll() => HandleResult(await _mediator.Send(new GetAllClassesQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) => HandleResult(await _mediator.Send(new GetClassByIdQuery(id)));

        // Chỉ Admin mới được Thêm, Sửa, Xóa lớp học
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateClassCommand command) => HandleResult(await _mediator.Send(command));

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateClassCommand command)
        {
            command.ClassId = id;
            return HandleResult(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id) => HandleResult(await _mediator.Send(new DeleteClassCommand(id)));
    }
}

