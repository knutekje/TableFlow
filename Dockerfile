# Use the official .NET runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Install dependencies and build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TableFlowBackend.csproj", "./"]
RUN dotnet restore "TableFlowBackend.csproj"
COPY . .
RUN dotnet publish "TableFlowBackend.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TableFlowBackend.dll"]
