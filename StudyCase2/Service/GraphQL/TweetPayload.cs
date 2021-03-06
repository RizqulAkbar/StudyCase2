using Service.Models;
using System;

namespace Service.GraphQL
{
    public class TweetPayload
    {
        public TweetPayload(Tweet tweet)
        {
           TweetID = tweet.TweetId;
           Username = tweet.Username;
           Tweet = tweet.Tweet1;
           Created = tweet.Created;
        }

        public int TweetID { get; set; }
        public string Username { get; set; }
        public string Tweet { get; set; }
        public DateTime Created { get; set; }
    }
}
