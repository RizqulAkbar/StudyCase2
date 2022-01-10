namespace Service.GraphQL
{
    public record TransactionStatus
    (
        bool IsSucceed,
        string? Message
    );
}
