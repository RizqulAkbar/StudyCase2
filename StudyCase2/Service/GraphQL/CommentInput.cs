namespace Service.GraphQL
{
    public record CommentInput
        (
            int? CommentId,
            int TweetId,
            int? UserId,
            string Username,
            string Comment
        );
}
