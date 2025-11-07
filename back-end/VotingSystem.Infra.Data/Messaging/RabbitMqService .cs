using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VotingSystem.Domain.Interfaces;

namespace VotingSystem.Application.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMq:HostName"],
                Port = int.Parse(_configuration["RabbitMq:Port"] ?? "5672"),
                UserName = _configuration["RabbitMq:UserName"],
                Password = _configuration["RabbitMq:Password"]
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();

        }

        public void Publish(string queueName, object message)
        {
            // Cria um canal temporário por operação
            using var channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            // Declara a fila
            channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            // Serializa e publica a mensagem
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body).GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.CloseAsync();
        }

    }
}
