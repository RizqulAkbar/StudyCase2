namespace Service.GraphQL
{
    public record RegisterUser
    (
        string Email,
        string Firstname,
        string Lastname,
        string UserName,
        string Password
    );
}
