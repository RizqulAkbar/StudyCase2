namespace Service.GraphQL
{
    public record CommentInput
        (
            int TweetID,
            string Username,
            string Comment
        );
}
