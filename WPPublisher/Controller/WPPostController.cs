using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPPublisher.Model;

namespace WPPublisher.Controller
{
    internal static class WPPostController
    {
        private static string _queueServer = "localhost";
        private static string _queueExchange = "testWPPost";

        public static int EnqueuePost(WPPost post)
        {
            int response = 0;

            try
            {
                // TO DO
                // controllo configurazione

                var factory = new ConnectionFactory() { HostName = _queueServer };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: _queueExchange, type: ExchangeType.Fanout, durable: true);

                        JObject obj =(JObject)JToken.FromObject(post);

                        var body = Encoding.UTF8.GetBytes(obj.ToString());
                        channel.BasicPublish(exchange: _queueExchange,
                            routingKey: "",
                            basicProperties: null,
                            body: body);
                    }
                }

                response = 1;
            }
            catch (System.Exception ex)
            {
                response = -1;
            }

            return response;
        }

        public static void PublishPost()
        { }
    }
}
