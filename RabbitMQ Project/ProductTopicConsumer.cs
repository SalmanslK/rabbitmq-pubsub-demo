using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace RabbitMQ_Project
{
    public class ProductTopicProducer : ProducerBase<string>
    {
        public ProductTopicProducer(ConnectionFactory connectionFactory) : base(connectionFactory, ExchangeType.Topic) { }

        protected override string Exchange => ProductTopicConsumerService.ExchangeName;
        protected override string Queue => "";
        protected override string AppId => "ProductProducer";
        protected override string QueueAndExchangeRoutingKey => "";
    }

    /// <summary>Receives every message whose routing key matches Product.*</summary>
    public class ProductTopicConsumerService : ConsumerBase<string>, IHostedService
    {
        public const string ExchangeName = "ProductExchangeTopic";

        private readonly Action<string, string> _onMessage;

        public ProductTopicConsumerService(ConnectionFactory connectionFactory, Action<string, string> onMessage)
            : base(connectionFactory, ExchangeType.Topic)
        {
            _onMessage = onMessage;
            Consume();
        }

        protected override string Exchange => ExchangeName;
        protected override string Queue => "Product.Events";
        protected override string AppId => "ProductConsumer";
        protected override string QueueAndExchangeRoutingKey => "Product.*";

        protected override void ReceiveAction(string message, string routingKey) => _onMessage(routingKey, message);

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
