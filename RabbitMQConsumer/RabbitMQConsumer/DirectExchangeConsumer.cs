using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQConsumer
{
    class DirectExchangeConsumer
    {
        public static void DirectConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                var queuename = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queuename,
                                    exchange: "direct_logs",
                                    routingKey: "Demo-queue");


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

                Console.WriteLine("[x][x][x]=> Direct Exchange <=[x][x][x]");
                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
