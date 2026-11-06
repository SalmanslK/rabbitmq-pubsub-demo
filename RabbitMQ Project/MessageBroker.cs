using RabbitMQ.Client;

namespace RabbitMQ_Project
{
    public class MessageBroker : IDisposable
    {
        private readonly IConnection _connection;

        public MessageBroker(ConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
        }

        public void Send(string queue, string data) => _connection.Send(queue, data);

        public string? Receive(string queue) => _connection.Receive(queue);

        public void Dispose() => _connection.Dispose();
    }
}
