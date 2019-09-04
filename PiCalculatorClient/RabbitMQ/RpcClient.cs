using Newtonsoft.Json;
using PiCalculatorClient.MessageModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PiCalculatorClient.RabbitMQ
{
    public class RpcClient : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly IBasicProperties props;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            props.CorrelationId = Guid.NewGuid().ToString();//todo save this value
            props.ReplyTo = replyQueueName;

            consumer.Received += ReceivedResponseSubscription;
        }

        private void ReceivedResponseSubscription(object sender, BasicDeliverEventArgs ea)
        {
            var response = Encoding.UTF8.GetString(ea.Body);
            var deserialized = JsonConvert.DeserializeObject<CalculatePiResponse>(response);

            ReceivedResponse.Invoke(sender, new ResponseEventArgs(deserialized));
            Console.WriteLine("[ReceivedResponseSubscription] recived response {0}", response);
        }

        public event EventHandler<ResponseEventArgs> ReceivedResponse;

        public async Task AddToQueue(MessageModel messageModel)
        {
            var messageBytes = messageModel.SelfConvertToBytes();
            channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
        }

        public void Close()
        {
            connection.Close();
        }

        public void Dispose()
        {
            Close();
            channel.Dispose();//todo dispose pattern
        }
    }
}
