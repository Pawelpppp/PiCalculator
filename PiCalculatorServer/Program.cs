using System;

namespace PiCalculatorServer
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Server Start");
            var cancelSynchronizedCollection = new CancelSynchronizedCollection();
            var cancelationQueueReceiver = new CancelationQueueReceiver(cancelSynchronizedCollection);
            var calculatorQueueReceiver = new PiCalculatorQueueReceiver(cancelSynchronizedCollection);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            calculatorQueueReceiver.Close();
            cancelationQueueReceiver.Close();
            //cancelSynchronizedCollection.Dispose();
        }
    }
}
