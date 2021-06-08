using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RabbitMQConsumer
{
    class PublishExchangeConsumer
    {
        public static void ExchangeConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var queuename = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queuename,
                                    exchange: "logs",
                                    routingKey: "hello");


                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    byte[] body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Consumer Received Queue Name: " + queuename + " and {0}", message);
                };

                channel.BasicConsume(queue: queuename,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("[x][x][x]=> Fanout Exchange <=[x][x][x]");
                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
