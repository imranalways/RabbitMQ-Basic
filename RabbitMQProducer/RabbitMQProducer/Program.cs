using System;

namespace RabbitMQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicProducer.ProducerBasic();
            WorkQueue.QueueWorke();
            //PublishExchange.Exchange();
            //DirectExchange.ExchangeDirect();
            //TopicExchange.ExchangeTopic();
        }
    }
}
