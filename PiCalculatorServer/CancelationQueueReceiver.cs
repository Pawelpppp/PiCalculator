using PiCalculatorServer.RabbitMQ;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace PiCalculatorServer
{
    public class CancelationQueueReceiver : IDisposable
    {
        private const string CancelQueueName = "CancelQueue";
        private readonly QueueServer _queueServer;
        private readonly CancelSynchronizedCollection _cancelSynchronizedCollection;

        public CancelationQueueReceiver(CancelSynchronizedCollection cancelSynchronizedCollection)
        {
            _cancelSynchronizedCollection = cancelSynchronizedCollection;
            _queueServer = new QueueServer(CancelQueueName);
            _queueServer.Received += OnRecived;
        }

        private void OnRecived(object sender, BasicDeliverEventArgs ea)
        {
            var deserialized = Encoding.UTF8.GetString(ea.Body);
            Console.WriteLine($" [CancelationQueueReceiver] Recived Cancul Guid {deserialized}");

            _cancelSynchronizedCollection.AddElement(new Guid(deserialized));
        }

        public void Dispose()
        {
            Close();
            _queueServer.Received -= OnRecived;
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }
    }
}
