using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
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
            
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclarePassive(_queueExchange);

            channel.QueueDeclarePassive(_queueName);
            Console.WriteLine($"Queue [{_queueName}] is waiting for messages.");

            var messageCount = channel.MessageCount(_queueName);
            if (messageCount > 0)
            {
                Console.WriteLine($"\tDetected {messageCount} message(s).");
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (bc, ea) =>
            {
                var t = DateTimeOffset.FromUnixTimeMilliseconds(ea.BasicProperties.Timestamp.UnixTime);
                Console.WriteLine($"{t.LocalDateTime:O} ID=[{ea.BasicProperties.MessageId}]");
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"\tProcessing message: '{message}'.");

                if (ea.BasicProperties.UserId != "ops0")
                {
                    Console.WriteLine($"\tIgnored a message sent by [{ea.BasicProperties.UserId}].");
                    return;
                }

                try
                {
                    Thread.Sleep((new Random().Next(1, 3)) * 1000);
                    var model = ((IBasicConsumer)bc).Model;
                    model.BasicAck(ea.DeliveryTag, false);
                }
                catch (AlreadyClosedException)
                {
                    Console.WriteLine("RabbitMQ is closed!");
                }
            };
            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
            connection.Close();
        }
    }
}
