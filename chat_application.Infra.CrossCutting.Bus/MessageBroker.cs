using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace chat_application.Infra.CrossCutting.Bus
{
    public class MessageBroker: IMessageBroker
    {
        private readonly ConnectionFactory _factory;
        private const string QUEUE_NAME = "messages";

        public MessageBroker()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }
        public void Insert(string message)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage);
                }
            }
        }
    }
}
