using System;

namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicConsumer.ConsumerBasic();
            //WorkQueueConsumer.QueueWorkConsumer();
            //PublishExchangeConsumer.ExchangeConsumer();
            //DirectExchangeConsumer.DirectConsumer();
            //TopicExchangeConsumer.TopicConsumer();
        }
    }
}
