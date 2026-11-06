using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Project
{
    public static class RabbitMQHelper
    {
        // Connection settings come from the environment; the defaults suit a local RabbitMQ server.
        public static ConnectionFactory CreateFactory() => new()
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest",
            DispatchConsumersAsync = true, // required by AsyncEventingBasicConsumer
        };

        public static void Send(this IConnection connection, string queue, string data)
        {
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue, false, false, false, null);
            channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
        }

        public static string? Receive(this IConnection connection, string queue)
        {
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue, false, false, false, null);
            BasicGetResult? result = channel.BasicGet(queue, true);
            return result == null ? null : Encoding.UTF8.GetString(result.Body.ToArray());
        }
    }
}
