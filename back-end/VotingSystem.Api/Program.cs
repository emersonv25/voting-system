using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;
using VotingSystem.Api.DTOs;
using VotingSystem.Api.Filters;
using VotingSystem.Api.Middlewares;
using VotingSystem.Infra.Ioc;


var builder = WebApplication.CreateBuilder(args);

// Registro dos serviços/repositorios
builder.Services.AddInfrastructure(builder.Configuration);



// Configuração básica: limite por IP
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("PerIpPolicy", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetTokenBucketLimiter(ip, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 5,                   
            TokensPerPeriod = 5,              
            ReplenishmentPeriod = TimeSpan.FromSeconds(10),
            QueueLimit = 0,                   
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            AutoReplenishment = true
        });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        var errorResponse = new ErrorResponseDTO(429, "Aguarde alguns segundos.");
        await context.HttpContext.Response.WriteAsJsonAsync(errorResponse, token);
    };
});


// Adiciona serviços de controle
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ErrorResponseFilter>();

}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors?.Count > 0)
            .SelectMany(e => e.Value?.Errors?.Select(x => x.ErrorMessage) ?? Enumerable.Empty<string>())
            .ToArray();

        var errorResponse = new ErrorResponseDTO(StatusCodes.Status400BadRequest, "Validation failed", errors);

        return new BadRequestObjectResult(errorResponse);
    };
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VotingSystemApi", Version = "v1" });
    c.EnableAnnotations();

});

// Adiciona CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Adiciona o serviço de background

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();

// Ativa o CORS
app.UseCors();

// ativa o rate limit
app.UseRateLimiter();

app.MapControllers();
app.Run();