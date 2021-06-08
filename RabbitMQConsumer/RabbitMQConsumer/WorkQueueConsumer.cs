using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RabbitMQConsumer
{
    class WorkQueueConsumer
    {
        public static void QueueWorkConsumer()
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

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                Console.WriteLine("[*] Waiting for messages.");
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Consumer Received {0}", message);

                    // string[] a = message.Split('.');
                    int dot = message.Split('.').Length - 1;
                    Thread.Sleep(dot * 1000);
                    Console.WriteLine("[x] Work is done");
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: "Demo-queue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine("[x][x][x]=> Work Queues <=[x][x][x]");
                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
