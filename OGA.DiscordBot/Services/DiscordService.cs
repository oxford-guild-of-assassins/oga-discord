using System.Reflection;
using System.Text;
using OGA.DiscordBot.Services.Interfaces;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace OGA.DiscordBot.Services;

public class DiscordService(IConfiguration config, DiscordSocketClient client, IServiceProvider serviceProvider) : IDiscordService
{
    private readonly string connectionString = config.GetConnectionString("DiscordBot") ?? "";
    private readonly DiscordSocketClient _client = client;
    private readonly InteractionService _interactionService = new(client);
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task ConfigureAsync()
    {
        _client.Log += Log;
        _client.Ready += OnClientReady;
        _client.SlashCommandExecuted += SlashCommandExecuted;

        await _client.LoginAsync(TokenType.Bot, connectionString);
        await _client.StartAsync();
    }

    private Task SlashCommandExecuted(SocketSlashCommand command)
    {
        var sb = new StringBuilder();
        sb.Append($"{command.User} executed {command.CommandName} with the following parameters:");
        foreach (var item in command.Data.Options)
        {
            sb.Append($"\t\n{item.Name}: {item.Value}");
        }

        Console.WriteLine(sb.ToString());
        return Task.CompletedTask;
    }

    private async Task OnClientReady()
    {
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

        var sb = new StringBuilder();
        sb.Append("Loaded commands ");
        foreach (var slashCommand in await _interactionService.RegisterCommandsGloballyAsync())
        {
            sb.Append($"{slashCommand.Name}, ");
        }
        sb.Length -= 2;
        sb.Append('.');
        Console.WriteLine(sb.ToString());

        _client.InteractionCreated += async (x) =>
        {
            var ctx = new SocketInteractionContext(_client, x);
            await _interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
        };
    }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}