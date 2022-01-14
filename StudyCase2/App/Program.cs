using App.Models;
using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace App
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();


            var Serverconfig = new ConsumerConfig
            {
                BootstrapServers = config["Settings:KafkaServer"],
                GroupId = "tester",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };
            Console.WriteLine("--------------.NET Application--------------");
            using (var consumer = new ConsumerBuilder<string, string>(Serverconfig).Build())
            {
                Console.WriteLine("Connected");
                var topics = new string[] { "tweet", "comment", "profile" };
                consumer.Subscribe(topics);

                Console.WriteLine("Waiting messages....");
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);
                        Console.WriteLine($"Consumed record with Topic: {cr.Topic} key: {cr.Message.Key} and value: {cr.Message.Value}");

                        using (var dbcontext = new TwittorContext())
                        {
                            if (cr.Topic == "tweet")
                            {
                                Tweet tweet = JsonConvert.DeserializeObject<Tweet>(cr.Message.Value);
                                dbcontext.Tweets.Add(tweet);
                            }
                            if (cr.Topic == "comment")
                            {
                                Comment comment = JsonConvert.DeserializeObject<Comment>(cr.Message.Value);
                                dbcontext.Comments.Add(comment);
                            }
                            if (cr.Topic == "profile")
                            {
                                Profile profile = JsonConvert.DeserializeObject<Profile>(cr.Message.Value);
                                dbcontext.Profiles.Add(profile);
                            }
                            await dbcontext.SaveChangesAsync();
                            Console.WriteLine("Data was saved into database");
                        }


                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                }
                finally
                {
                    consumer.Close();
                }

            }

            return 1;
        }
    }
}
