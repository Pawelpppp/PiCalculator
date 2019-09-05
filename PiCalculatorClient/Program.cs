using PiCalculatorClient.MessageModels;
using PiCalculatorClient.RabbitMQ;
using System;

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
            for (int i = 0; i < 300; i++)
            {
                Console.WriteLine($" [Client] Calculate Pi with precision {i}");
                var message = new MessageModel("CalculatePi", i);
                rpcClient.AddToQueue(message);

                // Cancel some request
                if (i > 138 && i < 200)
                {
                    cancelClient.SendCancelMessage(message.Id.ToString());
                }


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
