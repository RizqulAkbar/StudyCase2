namespace Service.GraphQL
{
    public record RegisterUser
    (
        string Email,
        string UserName,
        string Password
    );
}
