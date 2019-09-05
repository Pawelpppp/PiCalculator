using Newtonsoft.Json;
using PiCalculatorServer.MessageModels;
using PiCalculatorServer.RabbitMQ;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace PiCalculatorServer
{
    public class PiCalculatorQueueReceiver : IDisposable
    {
        private RpcServer rpcServer;
        private const string RpcServerName = "rpc_queue";
        private readonly CancelSynchronizedCollection _cancelSynchronizedCollection;

        public PiCalculatorQueueReceiver(CancelSynchronizedCollection cancelSynchronizedCollection)
        {
            rpcServer = new RpcServer(RpcServerName);
            rpcServer.Received += OnReceived;
            _cancelSynchronizedCollection = cancelSynchronizedCollection;
        }
        private void OnReceived(object sender, BasicDeliverEventArgs ea)
        {
            CalculatePiResponse response = null;

            var body = ea.Body;
            var props = ea.BasicProperties;
            var replyProps = rpcServer.Channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var deserialized = JsonConvert.DeserializeObject<CalculatePiMessage>(Encoding.UTF8.GetString(body));
                Console.WriteLine($" [Server] Calculate Pi with precision {deserialized.PrecisionResult}");
                
                //check if the request was canceled
                string pi = _cancelSynchronizedCollection.IsInCollection(deserialized) ? "-1" : PiCalculator.CalculatePi(deserialized.PrecisionResult);
                response = new CalculatePiResponse(deserialized, pi);
            }
            catch (Exception e)
            {
                Console.WriteLine(" [Server] " + e.Message);
                response = null;
            }
            finally
            {
                Console.WriteLine($" [Server] Send response: {response.Result} for { response.PrecisionResult}");
                rpcServer.Channel.BasicPublish(
                    exchange: "",
                    routingKey: props.ReplyTo,
                    mandatory: false,
                    basicProperties: replyProps,
                    body: response.SelfConvertToBytes());
                rpcServer.Channel.BasicAck(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false);
            }
        }

        public void Close()
        {
            rpcServer.Close();
        }

        public void Dispose()
        {
            Close();
            rpcServer.Received -= OnReceived;
        }
    }
}
