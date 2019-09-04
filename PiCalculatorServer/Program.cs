using PiCalculatorServer.RabbitMQ;
using System;

namespace PiCalculatorServer
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Server Start");
            var calculatorQueueReceiver = new PiCalculatorQueueReceiver();
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            calculatorQueueReceiver.Close();
        }
    }
}
