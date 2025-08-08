using classLib;
using classLib.LogDtos;
using LoggingAPI.Business.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;
using System.Text.Json;

public class RabbitMqConsumer
{
    private readonly ILogService _logService;
    private readonly IConnection _connection;

    public RabbitMqConsumer(RabbitMqConnectionManager connectionManager, ILogService logService)
    {
        _connection = connectionManager.GetConnection();
        _logService = logService;
    }

    public async Task StartConsumingAsync()
    {
        var channel = await _connection.CreateChannelAsync();
        await channel.QueueDeclareAsync("auth-log-queue", false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var log = JsonSerializer.Deserialize<Log>(message);

            if (log != null)
            {
                // Burada servis çağrısı yapıyorsun
                await _logService.LogAuthAsync(log);
            }
        };

        await channel.BasicConsumeAsync("auth-log-queue", autoAck: true, consumer: consumer);
    }
}
