FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-backend

WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet build

# Expose the port
EXPOSE 8080

WORKDIR /app/WebApi/

# Start the application
CMD ["dotnet", "run", "--urls", "http://0.0.0.0:8080"]