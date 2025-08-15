using classLib;
using classLib.LogDtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Products.Infrastructures.Messaging
{ 
public class RabbitMqProducer
{
        private readonly IConnection _connection;

        public RabbitMqProducer(RabbitMqConnectionManager connectionManager)
        {
            _connection = connectionManager.GetConnection();
        }

        public async Task SendLogAsync(Log log)
        {
            await using var channel = await _connection.CreateChannelAsync();
            var json = JsonSerializer.Serialize(log);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.QueueDeclareAsync("auth-log-queue", false, false, false, null);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "auth-log-queue",
                body: body);
        }
    }
}