using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkFlow.Application.Commands.ApproveRequest;
using WorkFlow.Application.Commands.CreateRequest;
using WorkFlow.Application.Commands.RejectRequest;
using WorkFlow.Application.DTOs;
using WorkFlow.Application.Queries;

namespace WorkFlow.API.Controllers
{
    [ApiController]
    [Route("api/request")]
    [Authorize]
    public class RequestsController(IRequestQueriesRepository queries) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRequestDto dto,
            [FromServices] CreateRequestCommandHandler handler)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = new CreateRequestCommand(dto.Title, dto.Description, dto.Category, dto.Priority, userId!);

            await handler.HandleAsync(command);
            return StatusCode(201);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRequests([FromQuery] RequestFilter filter)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(await queries.GetFilteredRequestsAsync(filter, userId!, userRole!));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var request = await queries.GetDetailByIdAsync(id);
            if (request == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Manager" && request.CreatedByUser != userId) return Forbid();

            return Ok(request);
        }

        [HttpPost("{id:guid}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> approve(
            Guid id,
            [FromBody] ApprovalDto dto,
            [FromServices] ApproveRequestCommandHandler handler)
        {
            var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(managerId))
                return Unauthorized(new { message = "Usuário não identificado." });

            var command = new ApproveRequestCommand(
                RequestId: id,
                ManagerId: managerId,
                UserRole: userRole ?? "User",
                Comment: dto.comment
            );

            await handler.Handle(command);

            return Ok(new { message = "Solicitação aprovada com sucesso." });
        }

        [HttpPost("{id:guid}/reject")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Reject(
            Guid id,
            [FromBody] RejectionDto dto,
            [FromServices] RejectionRequestCommandHandler handler)
        {
            var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(managerId))
                return Unauthorized(new { message = "Usuário não identificado." });

            var command = new RejectRequestCommand(
                RequestId: id,
                ManagerId: managerId,
                UserRole: userRole ?? "User",
                Comment: dto.comment
            );

            await handler.Handle(command);

            return Ok(new { message = "Solicitação rejeitada com sucesso." });
        }

        [HttpGet("{id:guid}/history")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            var request = await queries.GetDetailByIdAsync(id);
            if (request == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Manager" && request.CreatedByUser != userId) return Forbid();

            var history = await queries.GetRequestHistoryAsync(id, userId, userRole);
            return Ok(history);
        }
    }
}