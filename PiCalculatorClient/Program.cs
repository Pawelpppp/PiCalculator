using PiCalculatorClient.RabbitMQ;
using System;
using System.Threading;
using PiCalculatorClient.MessageModels;

namespace PiCalculatorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Client starts!");
            var cancelClient = new CancelClient();
            var rpcClient = new RpcClient();
          
            rpcClient.ReceivedResponse += OnReceivedResponse;
            for (int i = 3; i < 145; i++)
            {
                Console.WriteLine($" [Client] Calculate Pi with precision {i}");
                var message = new MessageModel("CalculatePi", i);
                rpcClient.AddToQueue(message);

            }

            cancelClient.SendCancelMessage(Guid.NewGuid().ToString());
            Console.Read();
            rpcClient.Close();

            //todo move to dispose
            rpcClient.ReceivedResponse -= OnReceivedResponse;
        }

        private static void OnReceivedResponse(object sender, ResponseEventArgs eventArgs)
        {
            Console.WriteLine($" [Client] Recived response. Result is {eventArgs.PiResponse.Result} for {eventArgs.PiResponse.PrecisionResult}");
        }

    }
}
