using Newtonsoft.Json;
using PiCalculatorServer.MessageModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PiCalculatorServer.RabbitMQ
{
    
    public class RpcServer//dispose
    {
        private readonly string queueName;
        private readonly IConnection connection;
        public IModel Channel { get; private set; }
        public event EventHandler<BasicDeliverEventArgs> Received;
        public RpcServer(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            Channel = connection.CreateModel();
            Channel.QueueDeclare(
                queue: queueName, 
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            Channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(Channel);
            Channel.BasicConsume(queue: queueName,
                autoAck: false, consumer: consumer);
            Console.WriteLine(" [Server] Awaiting RPC requests");

            consumer.Received += OnReceived;
            this.queueName = queueName;
        }

        private void OnReceived(object sender, BasicDeliverEventArgs ea)
        {
            Console.WriteLine("[Server] Server retrived a message");
            Received.Invoke(sender, ea);
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
