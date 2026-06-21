FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY news.feed.sln .
COPY news.feed/news.feed.csproj news.feed/
COPY news.feed.models/news.feed.models.csproj news.feed.models/
COPY configuration.core/configuration.core.csproj configuration.core/
COPY extra/extra.csproj extra/
COPY news.feed.Tests/news.feed.Tests.csproj news.feed.Tests/

RUN dotnet restore "news.feed.sln"

COPY . .

WORKDIR /src/news.feed
 
RUN dotnet restore "news.feed.csproj"

RUN dotnet publish "news.feed.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final

RUN addgroup -S appgroup && adduser -S appuser -G appgroup

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true


EXPOSE 8080

USER appuser

ENTRYPOINT ["dotnet", "news.feed.dll"]
