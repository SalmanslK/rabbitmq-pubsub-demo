using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ_Project
{
    public interface IRabbitMqProducer<in T>
    {
        bool Publish(T @event);
    }

    public abstract class ProducerBase<T> : RabbitMqClientBase, IRabbitMqProducer<T>
    {

        protected ProducerBase(ConnectionFactory connectionFactory, string exchangeType) : base(connectionFactory, exchangeType, true) { }

        public bool Publish(T @event)
        {
            var result = false;
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

                var properties = Channel.CreateBasicProperties();
                properties.AppId = AppId;
                properties.ContentType = "application/json";
                properties.DeliveryMode = 2; //persist mode
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                Channel.BasicPublish(exchange: RefinedExchangeName, routingKey: RefinedRoutingKey, basicProperties: properties, body: body);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }

}
