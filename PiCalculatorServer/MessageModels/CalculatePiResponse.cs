using System;
using System.Text;
using Newtonsoft.Json;

namespace PiCalculatorServer.MessageModels
{
    public class CalculatePiResponse
    {
        public CalculatePiResponse(CalculatePiMessage calculatePiMessage, string result)
        {
            Id = calculatePiMessage.Id;
            TaskName = calculatePiMessage.TaskName;
            PrecisionResult = calculatePiMessage.PrecisionResult;
            Result = result;
        }
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public string Result { get; set; }
        public int PrecisionResult { get; set; }

        public byte[] SelfConvertToBytes()
        {
            string message = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(message);
        }
    }
}
