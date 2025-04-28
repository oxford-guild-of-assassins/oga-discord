// See https://aka.ms/new-console-template for more information
using OGA.DiscordBot.Services;
using OGA.DiscordBot.Services.Interfaces;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()
    .Build();

serviceCollection
    .AddSingleton<IConfiguration>(_ => config)
    .AddSingleton<DiscordSocketClient>()
    .AddSingleton<IGameApiService, GameApiService>()
    .AddSingleton<IDiscordService, DiscordService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

await serviceProvider.GetRequiredService<IDiscordService>().ConfigureAsync();

await Task.Delay(Timeout.Infinite);