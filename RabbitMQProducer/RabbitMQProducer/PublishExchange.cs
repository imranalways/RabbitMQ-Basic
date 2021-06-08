using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducer
{
    class PublishExchange
    {
        public static void Exchange()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
                
                for (int i = 1; i <= 10; i++)
                {
                    var message = GetMessage(i);
                    var body = Encoding.UTF8.GetBytes(message);
                    
                    channel.BasicPublish(exchange: "logs",
                                            routingKey: "",
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
