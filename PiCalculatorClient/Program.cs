using PiCalculatorClient.RabbitMQ;
using System;
using System.Threading;
using PiCalculatorClient.MessageModels;

namespace PiCalculatorClient
{
    class Program
    {
        //private RpcClient rpcClient;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Client starts!");
            var rpcClient = new RpcClient();
            rpcClient.ReceivedResponse += OnReceivedResponse;
            for (int i = 15; i < 45; i++)
            {
                Console.WriteLine($" [Client] Calculate Pi with precision {i}");
                var message = new MessageModel("CalculatePi", i);
                rpcClient.AddToQueue(message);

            }
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
