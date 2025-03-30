using OGA.DiscordBot.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace OGA.DiscordBot.Services;

public class GameApiService(IConfiguration config): IGameApiService
{
    private readonly Uri apiUrl = new(config.GetConnectionString("GameApi"));
}
