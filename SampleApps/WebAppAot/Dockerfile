FROM mcr.microsoft.com/dotnet/sdk:9.0-noble-amd64 AS build

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       clang zlib1g-dev

WORKDIR /source

COPY --link . .
    
RUN dotnet publish -r linux-amd64 -o /app WebAppAot.csproj
RUN rm /app/*.dbg

# Final stage/image
FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:9.0-noble-chiseled-aot-amd64
WORKDIR /app
COPY --link --from=build /app .

USER $APP_UID
EXPOSE 8080

HEALTHCHECK --interval=60s --timeout=10s --retries=3 --start-period=5s CMD ["./WebAppAot", "--health", "--url", "http://localhost:8080/healthz"]

ENTRYPOINT ["./WebAppAot"]
