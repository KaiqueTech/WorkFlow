using WorkFlow.Domain.Enuns;

namespace WorkFlow.Domain.Models
{
    public class RequestHistoryModel
    {
        public Guid Id { get; private set; }
        public Guid RequestId { get; private set; }

        public RequestStatusEnum? FromStatus { get; private set; }
        public RequestStatusEnum ToStatus { get; private set; }

        public string ChangedBy { get; private set; }
        public DateTime ChangedAt { get; private set; }

        public string? Comment { get; private set; }

        protected RequestHistoryModel() { }

        public RequestHistoryModel(
            Guid requestId,
            RequestStatusEnum? fromStatus,
            RequestStatusEnum toStatus,
            string changedBy,
            string? comment)
        {
            //Id = Guid.NewGuid();
            RequestId = requestId;
            FromStatus = fromStatus;
            ToStatus = toStatus;
            ChangedBy = changedBy;
            ChangedAt = DateTime.UtcNow;
            Comment = comment;
        }
    }
}
