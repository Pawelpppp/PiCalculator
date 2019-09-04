namespace PiCalculatorClient.MessageModels
{
    public class MessageModel : MessageBase
    {

        public MessageModel(string taskName, int precisionResult)
        {
            TaskName = taskName;
            PrecisionResult = precisionResult;
        }

        public string TaskName { get; set; }
        public int PrecisionResult { get; set; }
    }
}
