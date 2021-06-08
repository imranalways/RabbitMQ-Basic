using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQConsumer
{
    class TopicExchangeConsumer
    {
        public static void TopicConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

                var queuename = channel.QueueDeclare().QueueName;

                //string[] routingkey = {"*.Mr.*","#.Imran"};

                channel.QueueBind(queue: queuename,
                                    exchange: "topic_logs",
                                    routingKey: "#.Imran");


                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    byte[] body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Consumer Received Queue Name: {0} and message: {1}",queuename, message);
                };

                channel.BasicConsume(queue: queuename,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("[x][x][x]=> Topic Exchange <=[x][x][x]");
                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
