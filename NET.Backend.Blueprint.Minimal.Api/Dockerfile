FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra AS base
WORKDIR /app

COPY ./publish/NET.Backend.Blueprint.Minimal.Api /app

EXPOSE 8080

ENTRYPOINT ["dotnet", "NET.Backend.Blueprint.Minimal.Api.dll"]
