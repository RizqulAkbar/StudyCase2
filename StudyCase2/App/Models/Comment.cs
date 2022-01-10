using System;
using System.Collections.Generic;

#nullable disable

namespace App.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Comment1 { get; set; }
        public DateTime Created { get; set; }

        public virtual Tweet Tweet { get; set; }
        public virtual Profile User { get; set; }
    }
}
