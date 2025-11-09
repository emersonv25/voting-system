# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VotingSystem.Worker/VotingSystem.Worker.csproj", "VotingSystem.Worker/"]
COPY ["VotingSystem.Infra.Ioc/VotingSystem.Infra.Ioc.csproj", "VotingSystem.Infra.Ioc/"]
COPY ["VotingSystem.Application/VotingSystem.Application.csproj", "VotingSystem.Application/"]
COPY ["VotingSystem.Domain/VotingSystem.Domain.csproj", "VotingSystem.Domain/"]
COPY ["VotingSystem.Infra.Data/VotingSystem.Infra.Data.csproj", "VotingSystem.Infra.Data/"]
RUN dotnet restore "./VotingSystem.Worker/VotingSystem.Worker.csproj"
COPY . .
WORKDIR "/src/VotingSystem.Worker"
RUN dotnet build "./VotingSystem.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VotingSystem.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VotingSystem.Worker.dll"]