FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

# SSL
RUN curl -o /tmp/rds-combined-ca-bundle.pem https://s3.amazonaws.com/rds-downloads/rds-combined-ca-bundle.pem \
    && mv /tmp/rds-combined-ca-bundle.pem /usr/local/share/ca-certificates/rds-combined-ca-bundle.crt \
    && update-ca-certificates

# Datadog APM
ENV TRACER_VERSION=1.13.0
RUN curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && dpkg -i ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json
ENV DD_DOTNET_TRACER_HOME=/opt/datadog

WORKDIR /app
COPY ./output/app ./

# OpenSSL cert for Kafka
RUN curl -sS -o /app/cert.pem https://curl.haxx.se/ca/cacert.pem
ENV HARALD_KAFKA_SSL_CA_LOCATION=/app/cert.pem

ENTRYPOINT [ "dotnet", "Harald.WebApi.dll" ]
