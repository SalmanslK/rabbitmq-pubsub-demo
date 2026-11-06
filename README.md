# RabbitMQ Pub/Sub Demo

A C# console app (.NET 6) showing two ways to use RabbitMQ, with
[RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client).

1. **Queue:** sends a message to a queue and reads it back.
2. **Topic exchange:** publishes three messages. The consumer subscribes to `Product.*`, so it receives
   `Product.Created` and `Product.Updated` but not `Order.Created`.

## Run

Requires the .NET 6 runtime, or a newer SDK with it installed, and a RabbitMQ server. To start one with Docker:

```
docker run -d --name rabbitmq --user rabbitmq -p 5672:5672 rabbitmq:3
```

Give it a few seconds to start, then:

```
dotnet run --project "RabbitMQ Project"
```

```
1. Queue: send a message and read it back
   Received: Hello World!
2. Topic exchange: the consumer only subscribes to Product.*
   Sent [Product.Created]: Product 1 created
   Sent [Order.Created]: Order 1 created
   Sent [Product.Updated]: Product 1 updated
   Received [MQ.Product.Created]: Product 1 created
   Received [MQ.Product.Updated]: Product 1 updated
Done. Order.Created didn't match, so the consumer never saw it.
```

The order of the `Sent` and `Received` lines can vary.

## Connecting elsewhere

It connects to `localhost` as `guest` by default. To use another server, set `RABBITMQ_HOST`, `RABBITMQ_USER` and
`RABBITMQ_PASSWORD`. Don't commit real credentials.

## Code

| File in `RabbitMQ Project/` | What it does |
| --- | --- |
| `RabbitMQHelper.cs`, `MessageBroker.cs` | Connection settings, and simple send and receive on a queue |
| `RabbitMqClientBase.cs` | Connects, then declares the exchange and, for consumers, the bound queue |
| `ProducerBase.cs` | Publishes persistent JSON messages and waits for the server to confirm them |
| `ConsumerBase.cs` | Consumes JSON messages and acknowledges them; a failed message is logged and dropped |
| `ProductTopicConsumer.cs` | The example producer and consumer for the topic exchange |
