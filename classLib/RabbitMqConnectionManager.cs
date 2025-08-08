using RabbitMQ.Client;

namespace classLib
{
    public class RabbitMqConnectionManager
    {
        private readonly IConnection _connection;

        public RabbitMqConnectionManager()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "ali",
                Password = "ali"
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public IConnection GetConnection() => _connection;
    }

}
