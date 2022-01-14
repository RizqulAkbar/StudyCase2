namespace Service.GraphQL
{
    public record ProfileInput
    (
        string Firstname,
        string Lastname,
        string Username,
        string Email
    );
}
