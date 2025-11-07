using VotingSystem.Infra.Ioc;
using VotingSystem.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Adiciona os serviços da infraestrutura
builder.Services.AddInfrastructure(builder.Configuration);

// Adiciona o serviço de background

builder.Services.AddHostedService<VoteWorker>();

var host = builder.Build();
host.Run();
