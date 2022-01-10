using HotChocolate;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Kafka;
using Service.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.GraphQL
{
    public class Query
    {
        public async Task<IQueryable<Tweet>> GetTweets(
            [Service] TwittorContext context,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetTweets-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetTweets" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);
            return context.Tweets;
        }

        public IQueryable<Tweet> GetTweetById(
            [Service] TwittorContext context,
            int id,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetTweetById-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetTweetById" }).ToString(Formatting.None);

            _ = KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var tweets = context.Tweets.Where(p => p.TweetId == id);

            return tweets;
        }

        public IQueryable<Tweet> GetTweetByUsername(
            [Service] TwittorContext context,
            string username,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetTweetByUsername-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetTweetByUsername" }).ToString(Formatting.None);

            _ = KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var tweets = context.Tweets.Where(p => p.Username == username);

            return tweets;
        }

        public IQueryable<Comment> GetComments(
            [Service] TwittorContext context,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetComments-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetComments" }).ToString(Formatting.None);

            _ = KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Comments;
        }

        public IQueryable<Comment> GetCommentById(
            [Service] TwittorContext context,
            int id,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetCommentById-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetCommentById" }).ToString(Formatting.None);

            _ = KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var comments = context.Comments.Where(p => p.CommentId == id);

            return comments;
        }

        public IQueryable<Comment> GetCommentByUsername(
            [Service] TwittorContext context,
            string username,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetCommentByUsername-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetCommentByUsername" }).ToString(Formatting.None);

            _ = KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var comments = context.Comments.Where(p => p.Username == username);

            return comments;
        }
    }
}
