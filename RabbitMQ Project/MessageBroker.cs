using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Project
{
    public class MessageBroker
    {
        private readonly IConnection _connection;
        public MessageBroker()
        {
            _connection = RabbitMQHelper.GetConnection("", "", "");
        }

        public void Send(string queue, string data)
        {
            _connection.Send(queue, data);
        }

        public void Receive(string queue)
        {
            _connection.Receive(queue);
        }
    }
}
