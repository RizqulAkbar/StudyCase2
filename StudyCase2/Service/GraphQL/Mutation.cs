using HotChocolate;
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
    public class Mutation
    {
        public async Task<TransactionStatus> AddTweetAsync(
            TweetInput input,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var product = new Tweet
            {
                Username = input.Username,
                Tweet1 = input.Tweet,
                Created = DateTime.Now
            };
            var key = "product-add-" + DateTime.Now.ToString();
            var val = JObject.FromObject(product).ToString(Formatting.None);
            var result = await KafkaHelper.SendMessage(kafkaSettings.Value, "product", key, val);
            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var ret = new TransactionStatus(result, "");
            if (!result)
                ret = new TransactionStatus(result, "Failed to submit data");


            return await Task.FromResult(ret);
        }
        public async Task<TransactionStatus> AddCommentAsync(
            CommentInput input,
            [Service] IOptions<KafkaSettings> kafkaSettings)
        {
            var cart = new Comment
            {
                Username = input.Username,
                Comment1 = input.Comment
            };
            var key = "comment-add-" + DateTime.Now.ToString();
            var val = JObject.FromObject(cart).ToString(Formatting.None);
            var result = await KafkaHelper.SendMessage(kafkaSettings.Value, "comment", key, val);
            await KafkaHelper.SendMessage(kafkaSettings.Value, "logging", key, val);

            var ret = new TransactionStatus(result, "");
            if (!result)
                ret = new TransactionStatus(result, "Failed to submit data");


            return await Task.FromResult(ret);

        }

            //----------------------------------------

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
                    Password = BCrypt.Net.BCrypt.HashPassword(input.Password)
                };

                var ret = context.Profiles.Add(newUser);
                await context.SaveChangesAsync();

                return await Task.FromResult(new ProfileData
                {
                    Id = newUser.UserId,
                    Username = newUser.Username,
                    Email = newUser.Email,
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

    }
}