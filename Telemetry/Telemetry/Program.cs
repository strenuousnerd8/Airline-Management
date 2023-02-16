using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.RegularExpressions;
using Telemetry;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("booking", exclusive: false);

Console.WriteLine("Telemetry Service Started");
Console.WriteLine("Listening on Booking ");

string objCap = String.Empty;
int flightNum = 0;

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    objCap = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
    flightNum = Int32.Parse(Regex.Match(objCap, @"\d+").Value);
    Console.WriteLine($"Flight Number of the search:\n{flightNum}");
    Console.WriteLine("Press enter to start RPC Client");
};

channel.BasicConsume(queue: "booking", autoAck: true, consumer: consumer);

Console.ReadKey();

Console.WriteLine("Routing request to RPC Server");

var rpcClient = new RpcClient();

string? n = flightNum.ToString();

Console.WriteLine($"[x] Requesting details of Flight no:\t{n}");
var response = rpcClient.Call(n);

Console.WriteLine("[.] Got\n{0}", response);
rpcClient.Close();

Console.ReadKey();