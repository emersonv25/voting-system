using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VotingSystem.Api.Middlewares;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Api.DTOs;
using VotingSystem.Api.Filters;
using VotingSystem.Infra.Ioc;


var builder = WebApplication.CreateBuilder(args);

// Registro dos serviços/repositorios
builder.Services.AddInfrastructure(builder.Configuration);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();
app.Run();