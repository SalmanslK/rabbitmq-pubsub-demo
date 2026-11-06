# RabbitMQ Pub/Sub Demo

A C# console app (.NET 6) for trying out RabbitMQ with [RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client).

## What's in it

- **`RabbitMQHelper.cs`, `MessageBroker.cs`:** the part the app runs. It sends a message to the `IDG` queue, reads
  it back and prints it.
- **`RabbitMqClientBase.cs`, `ProducerBase.cs`, `ConsumerBase.cs`:** base classes for publishing JSON messages to an
  exchange and consuming them from a bound queue, with acknowledgements.
- **`ProductTopicConsumer.cs`:** an example consumer written as a hosted service. It's a starting point: `Program.cs`
  doesn't use it or the base classes yet.

## Run

Requires the .NET 6 runtime, or a newer SDK with it installed, and a RabbitMQ server. To start one with Docker:

```
docker run -d --name rabbitmq -p 5672:5672 rabbitmq:3
```

The connection details in `MessageBroker.cs` are blank, so fill them in. For the local server above:

```csharp
_connection = RabbitMQHelper.GetConnection("localhost", "guest", "guest");
```

Then run it. It should print `Hello World!` and wait for you to press Enter:

```
dotnet run --project "RabbitMQ Project"
```

Don't commit real server credentials.
