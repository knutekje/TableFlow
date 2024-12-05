# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file(s) and restore dependencies
COPY ["TableFlowBackend/TableFlowBackend.csproj", "./"]
RUN dotnet restore "TableFlowBackend.csproj"

# Copy the entire application source code and build it
COPY . .
WORKDIR "/src/TableFlowBackend"
RUN dotnet publish "TableFlowBackend.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose the application's port
EXPOSE 5000

# Set the entry point
ENTRYPOINT ["dotnet", "TableFlowBackend.dll"]
