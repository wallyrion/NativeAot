# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source
ARG TARGETARCH

# Copy project file and restore as distinct layers
# Copy source code and publish app
COPY --link . .

RUN dotnet publish -r linux-amd64 -o /app WebAppNoAot.csproj


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble-chiseled-amd64
EXPOSE 8080
WORKDIR /app
COPY --link --from=build /app .

USER $APP_UID

HEALTHCHECK --interval=60s --timeout=10s --retries=3 --start-period=5s CMD ["./WebAppNoAot", "--health", "--url", "http://localhost:8080/healthz"]

ENTRYPOINT ["./WebAppNoAot"]