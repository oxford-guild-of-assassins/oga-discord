FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY ./OGA.DiscordBot ./src/OGA.DiscordBot
COPY ./OGA.DiscordBot.EntityFramework ./src/OGA.DiscordBot.EntityFramework
WORKDIR /src/OGA.DiscordBot
RUN dotnet build -o /app
RUN dotnet publish -o /publish

FROM mcr.microsoft.com/dotnet/runtime:8.0-noble-chiseled AS deploy
COPY --from=build /publish /app
WORKDIR /app
ENTRYPOINT [ "./OGA.DiscordBot" ]