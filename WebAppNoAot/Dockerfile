# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:9.0.100-rc.2 AS build
WORKDIR /source
ARG TARGETARCH

# Copy project file and restore as distinct layers
# Copy source code and publish app
COPY --link . .
RUN dotnet publish ./HealthConsoleApp/HealthConsoleApp/HealthConsoleApp.csproj -o /healthcheck -r linux-amd64

RUN dotnet publish -r linux-amd64 -o /app


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0.0-rc.2-noble-chiseled-amd64
EXPOSE 8080
WORKDIR /app
COPY --link --from=build /app .


USER $APP_UID

HEALTHCHECK CMD ["./WebAppNoAot"]


ENTRYPOINT ["./WebAppNoAot"]