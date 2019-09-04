using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using PiCalculatorServer.MessageModels;

namespace PiCalculatorServer
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [Server] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    CalculatePiResponse response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var deserialized = JsonConvert.DeserializeObject<CalculatePiMessage>(Encoding.UTF8.GetString(body));
                        Console.WriteLine($" [Server] Calculate Pi with precision {deserialized.PrecisionResult}");
                        response = new CalculatePiResponse(deserialized, fib(deserialized.PrecisionResult).ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [Server] " + e.Message);
                        response = null;
                    }
                    finally
                    {
                        Console.WriteLine($" [Server] Send response: {response.Result} for { response.PrecisionResult}");
                        channel.BasicPublish(
                            exchange: "",
                            routingKey: props.ReplyTo,
                            basicProperties: replyProps,
                            body: response.SelfConvertToBytes());
                        channel.BasicAck(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        //todo implement this method
        private static int fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }
    }
}
