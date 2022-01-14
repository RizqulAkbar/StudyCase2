namespace Service.GraphQL
{
    public record TweetInput
    (
        int? TweetId,
        string Username,
        string Tweet
    );
}
