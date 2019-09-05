using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PiCalculatorServer.RabbitMQ
{
   public class QueueServer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public event EventHandler<BasicDeliverEventArgs> Received;

        public QueueServer(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += OnReceived;

            _channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);
        }

        private void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine("[QueueServer] Recived a message");
            Received.Invoke(sender, e);
        }
        public void Close()
        {
            _connection.Close();
        }
    }

}
