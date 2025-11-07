using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Interfaces;

namespace VotingSystem.Worker
{
    public class VoteWorker : BackgroundService, IAsyncDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VoteWorker> _logger;
        private readonly IConnection _connection;
        private IChannel? _channel;
        private readonly string _queueName;

        public VoteWorker(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<VoteWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMq:HostName"],
                Port = int.Parse(_configuration["RabbitMq:Port"] ?? "5672"),
                UserName = _configuration["RabbitMq:UserName"],
                Password = _configuration["RabbitMq:Password"]
            };

            _queueName = _configuration["RabbitMq:QueueName"] ?? "vote_queue";
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<VoteMessage>(json);

                if (message == null)
                {
                    _logger.LogWarning("Mensagem recebida inválida: {Json}", json);
                    return;
                }

                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var voteRepository = scope.ServiceProvider.GetRequiredService<IVoteRepository>();
                    var participantRepository = scope.ServiceProvider.GetRequiredService<IParticipantRepository>();

                    var participant = await participantRepository.GetByIdAsync(message.ParticipantId);
                    if (participant == null)
                    {
                        _logger.LogWarning("Participante {ParticipantId} não encontrado.", message.ParticipantId);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                        return;
                    }

                    var vote = new Vote
                    {
                        Id = Guid.NewGuid(),
                        ParticipantId = message.ParticipantId,
                        CreatedAt = message.Timestamp
                    };

                    await voteRepository.AddAsync(vote);
                    _logger.LogInformation("Voto salvo para participante {ParticipantId}", message.ParticipantId);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar voto da mensagem: {Json}", json);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
            _logger.LogInformation("Vote Worker RabbitMQ iniciado e escutando fila {QueueName}", _queueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
                await _channel.CloseAsync();
            await _connection.CloseAsync();
        }

        private class VoteMessage
        {
            public Guid ParticipantId { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}
