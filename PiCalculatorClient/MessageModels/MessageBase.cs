using System;
using System.Text;
using Newtonsoft.Json;

namespace PiCalculatorClient.MessageModels
{
    public abstract class MessageBase
    {
        public Guid Id { get; set; }

        public MessageBase()
        {
            Id = Guid.NewGuid();
        }

        public byte[] SelfConvertToBytes()
        {
            string message = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(message);
        }
    }
}