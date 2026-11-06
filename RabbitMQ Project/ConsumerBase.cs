using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ_Project
{
    public abstract class ConsumerBase<T> : RabbitMqClientBase
    {
        protected ConsumerBase(ConnectionFactory connectionFactory, string exchangeType)
            : base(connectionFactory, exchangeType, false)
        {
        }

        protected void Consume()
        {
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += OnEventReceived;
            Channel.BasicConsume(queue: RefinedQueueName, autoAck: false, consumer: consumer);
        }

        private Task OnEventReceived(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                ReceiveAction(JsonConvert.DeserializeObject<T>(body)!, @event.RoutingKey);
                Channel.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                // Not requeued: a message that always fails would otherwise be redelivered forever
                Console.WriteLine($"Error occurred in OnEventReceived. Message: {ex.Message}");
                Channel.BasicNack(@event.DeliveryTag, false, false);
            }
            return Task.CompletedTask;
        }

        protected abstract void ReceiveAction(T message, string routingKey);
    }
}
