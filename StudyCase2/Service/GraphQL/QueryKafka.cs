using HotChocolate;
using Microsoft.EntityFrameworkCore;
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
    public class QueryKafka
    {
        public async Task<IQueryable<Tweet>> GetTweets(
            [Service] TwittorContext context,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetTweets-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetTweets" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);
            return context.Tweets.Include("Comments");
        }

        public async Task<IQueryable<Tweet>> GetTweetById(
            [Service] TwittorContext context,
            int id,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetTweetById-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetTweetById" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Tweets.Include("Comments").Where(p => p.TweetId == id); ;
        }

        public async Task<IQueryable<Tweet>> GetTweetByUsername(
            [Service] TwittorContext context,
            string username,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetTweetByUsername-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetTweetByUsername" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Tweets.Include("Comments").Where(p => p.Username == username); ;
        }

        public async Task<IQueryable<Comment>> GetComments(
            [Service] TwittorContext context,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetComments-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetComments" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Comments.Include("Tweet");
        }

        public async Task<IQueryable<Comment>> GetCommentById(
            [Service] TwittorContext context,
            int id,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetCommentById-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetCommentById" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Comments.Include("Tweet").Where(p => p.CommentId == id); ;
        }

        public async Task<IQueryable<Comment>> GetCommentByUsername(
            [Service] TwittorContext context,
            string username,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetCommentByUsername-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetCommentByUsername" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Comments.Include("Tweet").Where(p => p.Username == username); ;
        }

        public async Task<IQueryable<Profile>> GetProfiles(
            [Service] TwittorContext context,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var key = "GetProfile-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetProfile" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Profiles;
        }

        public async Task<IQueryable<Profile>> GetProfileByUsername(
            [Service] TwittorContext context,
            [Service] IOptions<KafkaSettings> kafkaSettings,
            string username)
        {
            var key = "GetProfileByUsername-" + DateTime.Now.ToString();
            var val = JObject.FromObject(new { Message = "GraphQL Query GetProfileByUsername" }).ToString(Formatting.None);

            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            return context.Profiles.Where(p => p.Username == username);
        }
    }
}
