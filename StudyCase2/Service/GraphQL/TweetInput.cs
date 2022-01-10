namespace Service.GraphQL
{
    public record TweetInput
    (
        int? TweetId,
        int? UserId,
        string Username,
        string Tweet
    );
}
