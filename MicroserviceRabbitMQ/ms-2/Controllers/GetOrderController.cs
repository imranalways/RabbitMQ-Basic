using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ViewModel;

namespace ms_2.Controllers
{
    [ApiController]
    public class GetOrderController : ControllerBase
    {
        private readonly IBus _bus;
        Order or = new Order();
        public GetOrderController(IBus bus)
        {
            _bus = bus;
        }
        //[Route("api/Sorder")]
        //[HttpPost]
        //public async Task<IActionResult> setorder(Order order)
        //{

        //    //var option = new CookieOptions();
        //    //option.HttpOnly = false;
        //    //option.Expires = DateTime.Now.AddMinutes(10);
        //    //Response.Cookies.Append("data", "true", option);
        //    return Ok(order); 
        //}
        [Route("api/Gorder")]
        [HttpGet]
        public async Task<IActionResult> Getorder()
        {
            //var services = new ServiceCollection();

            //services.AddMassTransit(x =>
            //{
            //    //x.AddConsumer<OrderConsumer>();
            //    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        cfg.UseHealthCheck(provider);
            //        cfg.Host(new Uri("rabbitmq://localhost"), h =>
            //        {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });
            //        cfg.ReceiveEndpoint("orderSaveQueue", ep =>
            //        {
            //            ep.PrefetchCount = 1;
            //            ep.UseMessageRetry(r => r.Interval(2, 100));
            //            //ep.ConfigureConsumer<OrderConsumer>(provider);

            //        });

            //    }));
            //    var y= x;
            //});

            //services.AddMassTransitHostedService();
            //services.AddControllers();

            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "orderNewSaveQueue", type: ExchangeType.Fanout);

                var queuename = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queuename,
                                    exchange: "orderNewSaveQueue",
                                    routingKey: "");


                var consumer = new EventingBasicConsumer(channel);
                var message="";
                consumer.Received += (model, ea) =>
                {
                    byte[] body = ea.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                    
                };
                string y = message;
                channel.BasicConsume(queue: queuename,
                                     autoAck: true,
                                     consumer: consumer);

                
            }
            return Ok();

        }

    }
}
