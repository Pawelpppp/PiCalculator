using PiCalculatorClient.RabbitMQ;
using System;

namespace PiCalculatorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var rpcClient = new RpcClient();
            Console.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30");
            Console.WriteLine(" [.] Got '{0}'", response);

            rpcClient.Close();
            Console.Read();
        }
    }
}
