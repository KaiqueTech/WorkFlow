namespace WorkFlow.Application.DTOs
{
    public record RequestHistoryDto(string? FromStatus, string ToStatus, string ChangedBy, DateTime ChangedAt, string? Comment);
}
