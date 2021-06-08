using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducer
{
    class TopicExchange
    {
        public static void ExchangeTopic()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
                string routingkey;
                for (int i = 1; i <= 10; i++)
                {
                    var message = GetMessage(i);
                    var body = Encoding.UTF8.GetBytes(message);
                    routingkey = (i % 2 == 0) ? "hello.Mr.Imran" : "Demo-queue.New.Queue";
                    channel.BasicPublish(exchange: "topic_logs",
                                            routingKey: routingkey,
                                            basicProperties: null,
                                            body: body);
                    Console.WriteLine("Producer Sent routingKey: {0} & message: {1}",routingkey, message);
                }
            }
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();
        }
        private static string GetMessage(int args)
        {
            return ((args % 2 == 0) ? "Hello Imran!" : "Demo queue message!");
        }
    }
}
