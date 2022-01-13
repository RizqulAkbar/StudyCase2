using Service.Models;
using System;

namespace Service.GraphQL
{
    public class CommentPayload
    {
        public CommentPayload(Comment comment)
        {
            Username = comment.Username;
            Comment = comment.Comment1;
            Created = comment.Created;
        }

        public string Username { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
