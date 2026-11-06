using RabbitMQ.Client.Exceptions;
using RabbitMQ_Project;

var factory = RabbitMQHelper.CreateFactory();

Console.WriteLine("1. Queue: send a message and read it back");
try
{
    using var broker = new MessageBroker(factory);
    broker.Send("IDG", "Hello World!");
    Console.WriteLine($"   Received: {broker.Receive("IDG") ?? "(nothing)"}");
}
catch (BrokerUnreachableException)
{
    Console.WriteLine($"Can't connect to RabbitMQ at {factory.HostName}. Is the server running? See the README.");
    return 1;
}

Console.WriteLine("2. Topic exchange: the consumer only subscribes to Product.*");
using var received = new CountdownEvent(2);
using var consumer = new ProductTopicConsumerService(factory, (routingKey, message) =>
{
    Console.WriteLine($"   Received [{routingKey}]: {message}");
    received.Signal();
});
using var producer = new ProductTopicProducer(factory);

foreach (var (routingKey, message) in new[]
{
    ("Product.Created", "Product 1 created"),
    ("Order.Created", "Order 1 created"),
    ("Product.Updated", "Product 1 updated"),
})
{
    Console.WriteLine($"   Sent [{routingKey}]: {message}");
    producer.Publish(message, routingKey);
}

var allArrived = received.Wait(TimeSpan.FromSeconds(10));
Console.WriteLine(allArrived ? "Done. Order.Created didn't match, so the consumer never saw it." : "Timed out waiting for messages.");
return allArrived ? 0 : 1;
