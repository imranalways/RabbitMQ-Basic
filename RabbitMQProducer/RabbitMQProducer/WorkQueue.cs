using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducer
{
    class WorkQueue
    {
        public static void QueueWorke()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Demo-queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                for (int i = 1; i <= 10; i++)
                {


                    var message = GetMessage(i);
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: "Demo-queue",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine("Producer Sent {0}", message);
                }
            }
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }
        private static string GetMessage(int args)
        {
            return ((args % 2 == 0) ? "Imran..." : "Hello Imran!");
        }
    
    }
}
