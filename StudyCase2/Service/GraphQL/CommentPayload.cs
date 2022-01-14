using Service.Models;
using System;

namespace Service.GraphQL
{
    public class CommentPayload
    {
        public CommentPayload(Comment comment)
        {
            CommentID = comment.CommentId;
            TweetID = comment.TweetId;
            Username = comment.Username;
            Comment = comment.Comment1;
            Created = comment.Created;
        }

        public int CommentID { get; set; }
        public int TweetID { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
