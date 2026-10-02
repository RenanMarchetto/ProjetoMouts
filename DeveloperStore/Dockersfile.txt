FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/DeveloperStore.Api/DeveloperStore.Api.csproj", "src/DeveloperStore.Api/"]
COPY ["src/DeveloperStore.Application/DeveloperStore.Application.csproj", "src/DeveloperStore.Application/"]
COPY ["src/DeveloperStore.Domain/DeveloperStore.Domain.csproj", "src/DeveloperStore.Domain/"]
COPY ["src/DeveloperStore.Infrastructure/DeveloperStore.Infrastructure.csproj", "src/DeveloperStore.Infrastructure/"]

RUN dotnet restore "src/DeveloperStore.Api/DeveloperStore.Api.csproj"

COPY . .

WORKDIR "/src/src/DeveloperStore.Api"

RUN dotnet publish "DeveloperStore.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DeveloperStore.Api.dll"]