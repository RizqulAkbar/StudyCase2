using HotChocolate;
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
            return context.Tweets;
        }

        public IQueryable<Tweet> GetTweetById(
            [Service] TwittorContext context,
            int id)
        {
            return context.Tweets.Where(p => p.TweetId == id);
        }

        public IQueryable<Tweet> GetTweetByUsername(
            [Service] TwittorContext context,
            string username)
        {
            return context.Tweets.Where(p => p.Username == username);
        }

        public IQueryable<Comment> GetComments(
            [Service] TwittorContext context)
        {
            return context.Comments;
        }

        public IQueryable<Comment> GetCommentById(
            [Service] TwittorContext context,
            int id)
        {
            return context.Comments.Where(p => p.CommentId == id);
        }

        public IQueryable<Comment> GetCommentByUsername(
            [Service] TwittorContext context,
            string username)
        {
            return context.Comments.Where(p => p.Username == username);
        }
    }
}
