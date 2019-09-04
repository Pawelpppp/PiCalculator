using System;

namespace PiCalculatorClient.MessageModels
{
   public class CalculatePiResponse
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public string Result { get; set; }
        public int PrecisionResult { get; set; }
    }
}
