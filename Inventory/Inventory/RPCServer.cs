using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Inventory
{
    internal class RPCServer : IRPCServer
    {
        public void Consume()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpcQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpcQueue", autoAck: false, consumer: consumer);
                Console.WriteLine("[x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;
                    var body = ea.Body.ToArray();
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        int n = int.Parse(message);
                        var transformer = new Transformer();
                        var result = transformer.GetValues(n);
                        Console.WriteLine("[.] Got Flight Number: ({0})", message);
                        response = result.ToString();
                    }
                    catch (Exception)
                    {
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();

            }

        }

    }
}