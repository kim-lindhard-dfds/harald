FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Harald.WebApi.csproj", "./"]
RUN dotnet restore "Harald.WebApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Harald.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Harald.WebApi.csproj" -c Release -o /app

FROM base AS final

ENV HARALD_KAFKA_SSL_CA_LOCATION=/app/cert.pem

# SSL
RUN curl -o /tmp/rds-combined-ca-bundle.pem https://s3.amazonaws.com/rds-downloads/rds-combined-ca-bundle.pem \
    && mv /tmp/rds-combined-ca-bundle.pem /usr/local/share/ca-certificates/rds-combined-ca-bundle.crt \
    && update-ca-certificates

WORKDIR /app
COPY --from=publish /app .

# OpenSSL cert for Kafka
RUN curl -sS -o /app/cert.pem https://curl.haxx.se/ca/cacert.pem

ENTRYPOINT ["dotnet", "Harald.WebApi.dll"]