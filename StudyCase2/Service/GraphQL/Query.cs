using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Service.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Service.GraphQL
{
    public class Query
    {
        public IQueryable<Tweet> GetTweets(
            [Service] TwittorContext context)
        {
            return context.Tweets.Include("Comments");
        }

        public IQueryable<Tweet> GetTweetById(
            [Service] TwittorContext context,
            int id)
        {
            return context.Tweets.Include("Comments").Where(p => p.TweetId == id);
        }

        public IQueryable<Tweet> GetTweetByUsername(
            [Service] TwittorContext context,
            string username)
        {
            return context.Tweets.Include("Comments").Where(p => p.Username == username);
        }

        public IQueryable<Comment> GetComments(
            [Service] TwittorContext context)
        {
            return context.Comments.Include("Tweet");
        }

        public IQueryable<Comment> GetCommentById(
            [Service] TwittorContext context,
            int id)
        {
            return context.Comments.Include("Tweet").Where(p => p.CommentId == id);
        }

        public IQueryable<Comment> GetCommentByUsername(
            [Service] TwittorContext context,
            string username)
        {
            return context.Comments.Include("Tweet").Where(p => p.Username == username);
        }

        public IQueryable<Profile> GetProfiles(
            [Service] TwittorContext context)
        {
            return context.Profiles;
        }

        public IQueryable<Profile> GetProfileByUsername(
            [Service] TwittorContext context,
            string username)
        {
            return context.Profiles.Where(p => p.Username == username);
        }
    }
}
