using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Profile
    {
        public Profile()
        {
            Comments = new HashSet<Comment>();
            Tweets = new HashSet<Tweet>();
        }

        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tweet> Tweets { get; set; }
    }
}
