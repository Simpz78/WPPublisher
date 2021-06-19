using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPPublisher.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private static string _queueServer = "localhost";
        private static string _queueExchange = "testWPPost";
        private static string _queueName = "testWPPost";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                ConsumeMessage();
                _logger.LogInformation("End Consume Message");
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void ConsumeMessage()
        {
            var factory = new ConnectionFactory() { HostName = _queueServer };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclarePassive(_queueExchange);
                    channel.QueueDeclarePassive(_queueName);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (bc, ea) =>
                    {
                        var t = DateTimeOffset.FromUnixTimeMilliseconds(ea.BasicProperties.Timestamp.UnixTime);
                                               
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        PostMessage(message);
                        try
                        {
                            Thread.Sleep((new Random().Next(1, 3)) * 1000);
                            var model = ((IBasicConsumer)bc).Model;
                            model.BasicAck(ea.DeliveryTag, false);
                        }
                        catch (AlreadyClosedException)
                        {
                            //Console.WriteLine("RabbitMQ is closed!");
                        }
                    };
                    channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
                }

                connection.Close();
            }
        }

        private void PostMessage(string message)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(message);
            string title = jo["Title"].ToString();
            string content = jo["Content"].ToString();

            var client = new RestClient("http://localhost:8080");
            client.Authenticator = new HttpBasicAuthenticator("test", "test");

            var request = new RestRequest("?rest_route=/wp/v2/posts", Method.POST);
            request.AddParameter("title", title);
            request.AddParameter("content", content);
            IRestResponse response = client.Execute(request);
        }
    }
}
