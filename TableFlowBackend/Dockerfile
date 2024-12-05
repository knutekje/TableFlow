# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project files
COPY *.sln ./
COPY TableFlowBackend/*.csproj ./TableFlowBackend/
COPY TableFlowBackend.Tests/*.csproj ./TableFlowBackend.Tests/

# Restore dependencies
RUN dotnet restore

# Copy the remaining files and build the app
COPY TableFlowBackend/. ./TableFlowBackend/
COPY TableFlowBackend.Tests/. ./TableFlowBackend.Tests/
WORKDIR /app/TableFlowBackend
RUN dotnet publish -c Release -o out

# Use the official runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/TableFlowBackend/out ./

# Expose the port and run the app
EXPOSE 5000
ENTRYPOINT ["dotnet", "TableFlowBackend.dll"]
