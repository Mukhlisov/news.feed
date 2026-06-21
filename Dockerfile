FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY news.feed.sln .
COPY news.feed/news.feed.csproj news.feed/
COPY news.feed.models/news.feed.models.csproj news.feed.models/
COPY configuration.core/configuration.core.csproj configuration.core/
COPY extra/extra.csproj extra/

RUN dotnet restore "news.feed/news.feed.csproj"

COPY . .

WORKDIR /src/news.feed
RUN dotnet publish "news.feed.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final

RUN addgroup -S appgroup && adduser -S appuser -G appgroup

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# The application listens on port 8080 when running in a container
# (see ConfigureKestrel in ConfigurationExtensions.cs)
EXPOSE 8080

USER appuser

ENTRYPOINT ["dotnet", "news.feed.dll"]
