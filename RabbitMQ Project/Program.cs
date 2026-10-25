// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ_Project;
using System.Configuration;

Console.WriteLine("Hello, World!");

MessageBroker messageBroker = new MessageBroker();
messageBroker.Send("IDG", "Hello World!");
messageBroker.Receive("IDG");
Console.ReadLine();