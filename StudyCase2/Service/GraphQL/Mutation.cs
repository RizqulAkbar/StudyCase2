using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        [Authorize]
        public async Task<TweetPayload> AddTweetAsync(
            TweetInput input,
            [Service] TwittorContext context)
        {
            var tweet = new Tweet
            {
                Username = input.Username,
                Tweet1 = input.Tweet,
                Created = DateTime.Now
            };

            context.Tweets.Add(tweet);
            await context.SaveChangesAsync();

            return new TweetPayload(tweet);
        }

        [Authorize]
        public async Task<TweetPayload> DeleteTweetByIdAsync(
            int id,
            [Service] TwittorContext context)
        {
            var tweet = context.Tweets.Where(o => o.TweetId == id).FirstOrDefault();
            if (tweet != null)
            {
                context.Tweets.Remove(tweet);
                await context.SaveChangesAsync();
            }


            return new TweetPayload(tweet);
        }

        [Authorize]
        public async Task<CommentPayload> AddCommentAsync(
            CommentInput input,
            [Service] TwittorContext context)
        {
            var comment = new Comment
            {
                TweetId = input.TweetID,
                Username = input.Username,
                Comment1 = input.Comment,
                Created = DateTime.Now
            };

            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            return new CommentPayload(comment);
        }

        [Authorize]
        public async Task<CommentPayload> DeleteCommentByIdAsync(
            int id,
            [Service] TwittorContext context)
        {
            var comment = context.Comments.Where(o => o.CommentId == id).FirstOrDefault();
            if (comment != null)
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
            }


            return new CommentPayload(comment);
        }

        //Profile

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
        public async Task<Profile> DeleteUserByUsernameAsync(
            string username,
            [Service] TwittorContext context)
        {
            var user = context.Profiles.Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                context.Profiles.Remove(user);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(user);
        }

    }
}