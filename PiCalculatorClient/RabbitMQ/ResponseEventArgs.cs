using System;
using PiCalculatorClient.MessageModels;

namespace PiCalculatorClient.RabbitMQ
{
    public class ResponseEventArgs : EventArgs
    {
        private readonly CalculatePiResponse _piResponse;

        public ResponseEventArgs(CalculatePiResponse response)
        {
            _piResponse = response;
        }

        public CalculatePiResponse PiResponse
        {
            get { return _piResponse; }
        }
    }
}
