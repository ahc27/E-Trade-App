namespace Infrastructure.Messaging
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly RabbitMqConsumer _consumer;

        public RabbitMqConsumerService(RabbitMqConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.StartConsumingAsync();

            // Uygulama kapanana kadar bekle
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

}
