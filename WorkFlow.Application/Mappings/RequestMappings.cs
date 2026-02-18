using WorkFlow.Application.Commands.ApproveRequest;
using WorkFlow.Application.Commands.CreateRequest;
using WorkFlow.Application.Commands.RejectRequest;
using WorkFlow.Application.DTOs;

namespace WorkFlow.Application.Mappings
{
    public static class RequestMappings
    {
        public static CreateRequestCommand ToCommand(this CreateRequestDto dto, string userId) 
        {
            return new CreateRequestCommand(dto.Title, dto.Description, dto.Category, dto.Priority, userId);
        }

        public static ApproveRequestCommand ToApproveCommand(this ApprovalDto dto, Guid requestId,string managerId,string? userRole)
        {
            return new ApproveRequestCommand(requestId,managerId,userRole ?? "User",dto.comment);
        }

        public static RejectRequestCommand ToRejectCommand(this RejectionDto dto, Guid requestId, string managerId, string? userRole) 
        {
            return new RejectRequestCommand(requestId, managerId, userRole ?? "User", dto.comment);
        }
    }
}
