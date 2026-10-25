using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Project
{
    public class ProductTopicConsumerService : ConsumerBase, IHostedService
    {
        public ProductTopicConsumerService(ConnectionFactory connectionFactory)
         : base(connectionFactory, ExchangeType.Direct)
        {
            try
            {
                //Consume<ProductCreatedIntegrationEvent>();
                Consume();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error => {ex?.Message}");
            }
        }

        protected override string Exchange => "ProductExchangeTopic5";
        protected override string Queue => "Product.Updated5";
        protected override string AppId => "ProductCreatedConsumer";
        protected override string QueueAndExchangeRoutingKey => "Product.*";


        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
