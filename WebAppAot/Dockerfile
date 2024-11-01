FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0-noble-aot AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
COPY --link *.csproj nuget.config ./
RUN dotnet restore -r linux-amd64

# Copy source code and publish app
COPY --link . .
RUN dotnet publish ./HealthConsoleApp/HealthConsoleApp/HealthConsoleApp.csproj -o /healthcheck -r linux-amd64
RUN rm -rf ./HealthConsoleApp
    
RUN dotnet publish -r linux-amd64 --no-restore -o /app WebAotSample.csproj
RUN rm /app/*.dbg /app/*.Development.json

# Final stage/image
FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:9.0-noble-chiseled-aot
WORKDIR /app
COPY --link --from=build /app .
COPY --link --from=build /healthcheck .
USER $APP_UID
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=10s --start-period=5s CMD ["./HealthConsoleApp"]

ENTRYPOINT ["./WebAotSample"]
