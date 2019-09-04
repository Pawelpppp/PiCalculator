using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PiCalculatorClient.RabbitMQ
{
    public class CancelClient : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        public CancelClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: "CancelQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async Task SendCancelMessage(string cancelId)
        {
            var body = Encoding.UTF8.GetBytes(cancelId);

            channel.BasicPublish(exchange: "",
                routingKey: "CancelQueue",
                basicProperties: null,
                body: body);
        }
        public void Close()
        {
            connection.Close();
        }

        public void Dispose()
        {
            Close();
            channel.Dispose();//todo implement dispose pattern
        }
    }
}
