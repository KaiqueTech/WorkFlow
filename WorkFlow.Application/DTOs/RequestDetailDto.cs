namespace WorkFlow.Application.DTOs
{
    public record RequestDetailDto(Guid Id, string Title, string Description, string Status, string Priority, string Category,
        string CreatedByUser, DateTime CreatedAt, DateTime? UpdatedAt)
    {
        public List<RequestHistoryDto> History { get; set; } = new();
    };
}
