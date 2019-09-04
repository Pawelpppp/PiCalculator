using System;

namespace PiCalculatorServer.MessageModels
{
    public class CalculatePiMessage
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public int PrecisionResult { get; set; }
    }
}
