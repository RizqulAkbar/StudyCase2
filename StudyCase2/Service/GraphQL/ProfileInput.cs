namespace Service.GraphQL
{
    public record ProfileInput
    (
        int? UserId,
        string Username,
        string Email
    );
}
