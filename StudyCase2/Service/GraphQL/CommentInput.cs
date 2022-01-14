namespace Service.GraphQL
{
    public record CommentInput
        (
            int? CommentID,
            int TweetID,
            string Username,
            string Comment
        );
}
