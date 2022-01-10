namespace Service.GraphQL
{
    public record ProfileToken
    (
        string? Token,
        string? Expired,
        string? Message
    );
}
