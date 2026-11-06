using RabbitMQ.Client;

namespace RabbitMQ_Project
{
    public abstract class RabbitMqClientBase : IDisposable
    {
        protected const string NamePrefix = "MQ";
        protected abstract string Exchange { get; }
        protected abstract string Queue { get; }
        protected abstract string AppId { get; }
        protected abstract string QueueAndExchangeRoutingKey { get; }

        protected IModel Channel { get; private set; } = null!;
        private IConnection? _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _exchangeType;
        private readonly bool _isPublisher;

        protected RabbitMqClientBase(ConnectionFactory connectionFactory, string exchangeType, bool isPublisher)
        {
            _connectionFactory = connectionFactory;
            this._exchangeType = exchangeType;
            this._isPublisher = isPublisher;
            ConnectToRabbitMq();
        }

        protected static string Refine(string name) => $"{NamePrefix}.{name}";

        protected internal string RefinedExchangeName => Refine(Exchange);
        protected internal string RefinedQueueName => Refine(Queue);
        protected internal string RefinedRoutingKey => Refine(QueueAndExchangeRoutingKey);
        private void ConnectToRabbitMq()
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel == null || Channel.IsOpen == false)
            {
                Channel = _connection.CreateModel();
                Channel.ExchangeDeclare(exchange: RefinedExchangeName, type: _exchangeType.ToString(), durable: true, autoDelete: false);
                if (!_isPublisher)
                {
                    Channel.QueueDeclare(queue: RefinedQueueName, durable: true, exclusive: false, autoDelete: false);
                    Channel.QueueBind(queue: RefinedQueueName, exchange: RefinedExchangeName, routingKey: RefinedRoutingKey);
                }
            }
        }

        public void Dispose()
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null!;

            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
    }
}
