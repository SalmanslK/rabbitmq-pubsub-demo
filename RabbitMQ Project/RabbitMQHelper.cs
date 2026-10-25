using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ_Project
{
    public static class RabbitMQHelper
    {

        public static IConnection GetConnection(string hostName, string userName, string password)
        {
            ConnectionFactory connectionFactory = new()
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
            return connectionFactory.CreateConnection();
        }

        public static void Send(this IConnection connection, string queue, string data)
        {
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue, false, false, false, null);
            channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
        }

        public static void Receive(this IConnection connection, string queue)
        {
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue, false, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            BasicGetResult result = channel.BasicGet(queue, true);
            if (result != null)
            {
                string data = Encoding.UTF8.GetString(result.Body.ToArray());
                Console.WriteLine(data);
            }
        }
    }
}


//public static void Send(this IConnection connection, string queue, string data)
//{
//    //using (IConnection connection = new ConnectionFactory().CreateConnection())
//    //{
//    using (IModel channel = connection.CreateModel())
//    {
//        channel.QueueDeclare(queue, false, false, false, null);
//        channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
//    }
//    //}
//}

//public static void Receive(this IConnection connection, string queue)
//{
//    //using (IConnection connection = new ConnectionFactory().CreateConnection())
//    //{
//    using (IModel channel = connection.CreateModel())
//    {
//        channel.QueueDeclare(queue, false, false, false, null);
//        var consumer = new EventingBasicConsumer(channel);
//        BasicGetResult result = channel.BasicGet(queue, true);
//        if (result != null)
//        {
//            string data = Encoding.UTF8.GetString(result.Body.ToArray());
//            Console.WriteLine(data);
//        }
//    }
//    //}
//}