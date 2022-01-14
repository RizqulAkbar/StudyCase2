using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Kafka;
using Service.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.GraphQL
{
    public class MutationKafka
    {
        [Authorize]
        public async Task<TransactionStatus> AddTweetAsync(
            TweetInput input,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var tweet = new Tweet
            {
                Username = input.Username,
                Tweet1 = input.Tweet,
                Created = DateTime.Now
            };
            var key = "tweet-add-" + DateTime.Now.ToString();
            var val = JObject.FromObject(tweet).ToString(Formatting.None);
            var result = await KafkaHelper.SendMessage(kafkaSettings.Value, "tweet", key, val);
            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var ret = new TransactionStatus(result, "");
            if (!result)
                ret = new TransactionStatus(result, "Failed to submit data");


            return await Task.FromResult(ret);
        }
        
        [Authorize]
        public async Task<TransactionStatus> DeleteTweetByIdAsync(
            int id,
            [Service] IOptions<KafkaSettings> kafkaSettings,
            [Service] TwittorContext context)
        {
            var tweet = context.Tweets.Where(o => o.TweetId == id).FirstOrDefault();
            if (tweet != null)
            {
                context.Tweets.Remove(tweet);
                await context.SaveChangesAsync();
            }
            var key = "tweet-delete-" + DateTime.Now.ToString();
            var val = JObject.FromObject(tweet).ToString(Formatting.None);
            var result = await KafkaHelper.SendMessage(kafkaSettings.Value, "tweet", key, val);
            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var ret = new TransactionStatus(result, "");
            if (!result)
                ret = new TransactionStatus(result, "Failed to submit data");


            return await Task.FromResult(ret);
        }
        [Authorize]
        public async Task<TransactionStatus> AddCommentAsync(
            CommentInput input,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var comment = new Comment
            {
                TweetId = input.TweetID,
                Username = input.Username,
                Comment1 = input.Comment,
                Created = DateTime.Now
            };
            var key = "comment-add-" + DateTime.Now.ToString();
            var val = JObject.FromObject(comment).ToString(Formatting.None);
            var result = await KafkaHelper.SendMessage(kafkaSettings.Value, "comment", key, val);
            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var ret = new TransactionStatus(result, "");
            if (!result)
                ret = new TransactionStatus(result, "Failed to submit data");


            return await Task.FromResult(ret);

        }
        [Authorize]
        public async Task<TransactionStatus> DeleteCommentByIdAsync(
           int id,
           [Service] IOptions<KafkaSettings> kafkaSettings,
           [Service] TwittorContext context)
        {
            var comment = context.Comments.Where(o => o.CommentId == id).FirstOrDefault();
            if (comment != null)
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
            }
            var key = "comment-delete-" + DateTime.Now.ToString();
            var val = JObject.FromObject(comment).ToString(Formatting.None);
            var result = await KafkaHelper.SendMessage(kafkaSettings.Value, "comment", key, val);
            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var ret = new TransactionStatus(result, "");
            if (!result)
                ret = new TransactionStatus(result, "Failed to submit data");


            return await Task.FromResult(ret);
        }

        //User

        public async Task<ProfileData> RegisterUserAsync(
            RegisterUser input,
            [Service] TwittorContext context)
            {
                var user = context.Profiles.Where(o => o.Username == input.UserName).FirstOrDefault();
                if (user != null)
                {
                    return await Task.FromResult(new ProfileData());
                }
                var newUser = new Profile
                {
                    Email = input.Email,
                    Username = input.UserName,
                    Firstname = input.Firstname,
                    Lastname = input.Lastname,
                    Password = BCrypt.Net.BCrypt.HashPassword(input.Password)
                };

                var ret = context.Profiles.Add(newUser);
                await context.SaveChangesAsync();

                return await Task.FromResult(new ProfileData
                {
                     Username = newUser.Username,
                     Email = newUser.Email,
                     Firstname = newUser.Firstname,
                     Lastname = newUser.Lastname
                });
            }

            public async Task<ProfileToken> LoginAsync(
                LoginUser input,
                [Service] IOptions<TokenSettings> tokenSettings,
                [Service] TwittorContext context)
            {
                var user = context.Profiles.Where(o => o.Username == input.Username).FirstOrDefault();
                if (user == null)
                {
                    return await Task.FromResult(new ProfileToken(null, null, "Username or password was invalid"));
                }
                bool valid = BCrypt.Net.BCrypt.Verify(input.Password, user.Password);
                if (valid)
                {
                    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
                    var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                    var expired = DateTime.Now.AddHours(3);
                    var jwtToken = new JwtSecurityToken(
                        issuer: tokenSettings.Value.Issuer,
                        audience: tokenSettings.Value.Audience,
                        expires: expired,
                        signingCredentials: credentials
                    );

                    return await Task.FromResult(
                        new ProfileToken(new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expired.ToString(), null));
                    //return new JwtSecurityTokenHandler().WriteToken(jwtToken);
                }

                return await Task.FromResult(new ProfileToken(null, null, Message: "Username or password was invalid"));
            }

            [Authorize]
            public async Task<TransactionStatus> DeleteUserByUsernameAsync(
               string username,
               [Service] IOptions<KafkaSettings> kafkaSettings,
               [Service] TwittorContext context)
            {
                var user = context.Profiles.Where(o => o.Username == username).FirstOrDefault();
                if (user != null)
                {
                    context.Profiles.Remove(user);
                    await context.SaveChangesAsync();
                }
                var key = "user-delete-" + DateTime.Now.ToString();
                var val = JObject.FromObject(user).ToString(Formatting.None);
                var result = await KafkaHelper.SendMessage(kafkaSettings.Value, "user", key, val);
                await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

                var ret = new TransactionStatus(result, "");
                if (!result)
                    ret = new TransactionStatus(result, "Failed to submit data");


                return await Task.FromResult(ret);
            }

    }
}