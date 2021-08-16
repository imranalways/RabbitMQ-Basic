using MassTransit;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ViewModel;

namespace ms_2
{
    public class OrderConsumer : IConsumer<Order>
    {
        private readonly IBus _bus;
        public OrderConsumer(IBus bus)
        {
            _bus = bus;
        }
        public async Task Consume(ConsumeContext<Order> context)
        {
            var data = context.Message;

            //Uri uri = new Uri("rabbitmq://localhost/orderSaveQueue");
            //var endPoint = await _bus. GetSendEndpoint(uri);
            //await endPoint.Send(data);

            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "orderNewSaveQueue", type: ExchangeType.Fanout);

               var message = data;
               var body = Encoding.UTF8.GetBytes("abcdhjskks");
               

                channel.BasicPublish(exchange: "orderNewSaveQueue",
                                        routingKey: "",
                                        basicProperties: null,
                                        body: body);
               
                }
            }
           
        

        // message received.
        //string jsonString = JsonSerializer.Serialize(data);
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44380/api/Sorder");
        //request.Method = "POST";
        //request.ContentType = "application/json";
        //request.ContentLength = jsonString.Length;
        //using (Stream webStream = request.GetRequestStream())
        //using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
        //{
        //    requestWriter.Write(jsonString);
        //}
        //try
        //{
        //    WebResponse webResponse = request.GetResponse();
        //    using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
        //    using (StreamReader responseReader = new StreamReader(webStream))
        //    {
        //        string response = responseReader.ReadToEnd();

        //    }
        //}
        //catch (Exception e)
        //{
        //    throw e;
        //}


    //}


    }
}
