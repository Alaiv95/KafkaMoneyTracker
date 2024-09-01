FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-backend

WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet build

EXPOSE 8080

WORKDIR /app/WebApi/

CMD ["dotnet", "run", "--urls", "http://0.0.0.0:8080"]