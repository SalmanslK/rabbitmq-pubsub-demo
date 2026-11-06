using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Project
{
    public interface IRabbitMqProducer<in T>
    {
        bool Publish(T @event, string routingKey);
    }

    public abstract class ProducerBase<T> : RabbitMqClientBase, IRabbitMqProducer<T>
    {
        protected ProducerBase(ConnectionFactory connectionFactory, string exchangeType)
            : base(connectionFactory, exchangeType, true)
        {
            Channel.ConfirmSelect();
        }

        public bool Publish(T @event, string routingKey)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

                var properties = Channel.CreateBasicProperties();
                properties.AppId = AppId;
                properties.ContentType = "application/json";
                properties.DeliveryMode = 2; // persistent
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                Channel.BasicPublish(exchange: RefinedExchangeName, routingKey: Refine(routingKey), basicProperties: properties, body: body);
                return Channel.WaitForConfirms(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred in Publish. Message: {ex.Message}");
                return false;
            }
        }
    }
}
