using System;
using System.Collections.Generic;

#nullable disable

namespace App.Models
{
    public partial class Tweet
    {
        public Tweet()
        {
            Comments = new HashSet<Comment>();
        }

        public int TweetId { get; set; }
        public string Username { get; set; }
        public string Tweet1 { get; set; }
        public DateTime Created { get; set; }

        public virtual Profile UsernameNavigation { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
