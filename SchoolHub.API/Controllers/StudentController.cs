using SchoolHub.Application.DTOs;
using SchoolHub.Application.Validations;
using SchoolHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

using SchoolHub.Domain.Entities;



using SchoolHub.Application.Features.Students.Commands;
using SchoolHub.Application.Features.Students.Queries;
using Microsoft.AspNetCore.Authorization;
namespace SchoolHub.API.Controllers
{
    [Authorize]
    [Route("api/student")]
    [ApiController]
    public class StudentController : ApiControllerBase
    {
        private readonly MediatR.IMediator _mediator;
        public StudentController(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("search")]
        [Authorize(Policy = "CanManageStudents")]
        public async Task<IActionResult> GetByKeyWordAsync([FromQuery] GetStudentByKeyWordQuery query)
        {
            var response = await _mediator.Send(query);
            return HandleResult(response);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStudentCommand command)
        {
            var student = await _mediator.Send(command);
            return HandleResult(student);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "CanManageStudents")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var query = new GetStudentByIdQuery(id);
            var result = await _mediator.Send(query);
            return HandleResult(result);

        }
        [HttpGet("class/{classId}")]
        [Authorize(Policy = "CanManageStudents")]
        public async Task<IActionResult> GetStudentsByClassIdAsync(string classId)
        {
            var query = new GetStudentsByClassIdQuery(classId);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "CanManageStudents")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateStudentCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var command = new DeleteStudentCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}










